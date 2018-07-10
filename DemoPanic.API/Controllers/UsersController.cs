namespace DemoPanic.API.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.SqlServer;
    using System.Data.Entity.Validation;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;
    using DemoPanic.API.Helpers;
    using DemoPanic.API.Models;
    using DemoPanic.Domain;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Newtonsoft.Json.Linq;
    using static System.Data.Entity.SqlServer.SqlFunctions;

    [RoutePrefix("api/Users")]
    public class UsersController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }
        
        // GET: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }


        
        [HttpPost]
        [Route("GetUsersByClientType")]
        public async Task<IHttpActionResult> GetUsersByClientType(JObject form)
        {
            int? clientTypeId = null;
            decimal? latitud = null;
            decimal? longitud = null;
            dynamic jsonObject = form;
            try
            {
                clientTypeId = jsonObject.ClientTypeId;
                latitud = jsonObject.Latitud;
                longitud = jsonObject.Longitud;
            }
            catch
            {
                return BadRequest("Missing parameter.");
            }

            var radiusParameter = await db.Parameters.FindAsync(1);
            var rangeParameter = await db.Parameters.FindAsync(2);
            var minimumServicesParameter = await db.Parameters.FindAsync(3);

            if (radiusParameter == null || rangeParameter == null)
            {
                return null;
            }

            CultureInfo cultureInfo = new CultureInfo("en-US");
            decimal pi = Convert.ToDecimal(Math.PI);

            decimal radius = Convert.ToDecimal(radiusParameter.Parameter, cultureInfo);
            decimal range = Convert.ToDecimal(rangeParameter.Parameter, cultureInfo);
            int minimumServices = Convert.ToInt32(minimumServicesParameter.Parameter, cultureInfo);

            decimal? lat1 = latitud - (range * 180 / (6371 * pi)); //lat min
            decimal? lat2 = latitud + (range * 180 / (6371 * pi)); //lat max

            double latT = Math.Asin(Math.Sin((double) (latitud*(pi/180))) /
                Math.Cos((double) (range/6371))); // Latitud central relativa a una aproximacion esferica de la tierra

            decimal lonDiff = (decimal) (Math.Asin(Math.Sin((double)(range / 6371)) /
                Math.Cos((double)(latitud * (pi / 180)))));

            decimal? lon1 = (longitud * (pi / 180) - lonDiff) * (180 / pi); // lon min
            decimal? lon2 = (longitud * (pi / 180) + lonDiff) * (180 / pi); //lon max
            
            //Ayuda matematica
            //https://www.movable-type.co.uk/scripts/latlong.html
            //http://janmatuschek.de/LatitudeLongitudeBoundingCoordinates#AngularRadius
            var user = await db.Users.
                Where(u => u.ClientTypeId == clientTypeId &&
                    u.Latitude > lat1 && u.Latitude < lat2 &&
                    u.Longitude > lon1 && u.Longitude < lon2
                ).
                Select(x => new {
                    UbicationId = x.UserId,
                    Description = x.FirstName,
                    x.Latitude,
                    x.Longitude,
                    Address = x.ClientTypeId,
                    Phone = x.Telephone,
                    distance =
                        6371 * 2 * Asin(SquareRoot(
                        Square(Sin(Math.Abs((decimal)latitud) - Math.Abs((decimal)x.Latitude)) *
                        Pi() / 180 / 2) +
                        Cos(Math.Abs((decimal)latitud) * pi / 180) * Cos(Math.Abs((decimal)x.Latitude) *
                        pi / 180) *
                        Square(Sin(Math.Abs((decimal)longitud) - Math.Abs((decimal)x.Longitude)) *
                        Pi() / 180 / 2)))
                }
                ).
                OrderBy(ux => ux.distance).
                Take(minimumServices).
                ToArrayAsync();

            if (user== null)
            {
                return null;
            }
            return Ok(user);
        }

        [HttpPost]
        [Authorize]
        [Route("GetUserByEmail")]
        public async Task<IHttpActionResult> GetUserByEmail(JObject form)
        {
            var email = string.Empty;
            dynamic jsonObject = form;
            try
            {
                email = jsonObject.Email.Value;
            }
            catch
            {
                return BadRequest("Missing parameter.");
            }

            var user = await db.Users.
                Where(u => u.Email.ToLower() == email.ToLower()).
                FirstOrDefaultAsync();
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [Route("LoginFacebook")]
        public async Task<IHttpActionResult> LoginFacebook(FacebookResponse profile)
        {
            try
            {
                var user = await db.Users.Where(u => u.Email == profile.Id).FirstOrDefaultAsync();
                if (user == null)
                {
                    user = new User
                    {
                        Email = profile.Id,
                        FirstName = profile.FirstName,
                        LastName = profile.LastName,
                        UserTypeId = 2,
                        Telephone = "...",
                    };

                    db.Users.Add(user);
                    UsersHelper.CreateUserASP(profile.Id, "User", profile.Id);
                }
                else
                {
                    user.FirstName = profile.FirstName;
                    user.LastName = profile.LastName;
                    db.Entry(user).State = EntityState.Modified;
                }

                await db.SaveChangesAsync();
                return Ok(true);
            }
            catch (DbEntityValidationException e)
            {
                var message = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    message = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        message += string.Format("\n- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }

                return BadRequest(message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost]
        [Authorize]
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(JObject form)
        {
            var email = string.Empty;
            var currentPassword = string.Empty;
            var newPassword = string.Empty;
            dynamic jsonObject = form;

            try
            {
                email = jsonObject.Email.Value;
                currentPassword = jsonObject.CurrentPassword.Value;
                newPassword = jsonObject.NewPassword.Value;
            }
            catch
            {
                return BadRequest("Incorrect call");
            }

            var userContext = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(userContext));
            var userASP = userManager.FindByEmail(email);

            if (userASP == null)
            {
                return BadRequest("Incorrect call");
            }

            var response = await userManager.ChangePasswordAsync(userASP.Id, currentPassword, newPassword);
            if (!response.Succeeded)
            {
                return BadRequest(response.Errors.FirstOrDefault());
            }

            return Ok("ok");
        }

        // PUT: api/Users/5
        [Authorize]
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutUser(int id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != user.UserId)
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Users
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Users.Add(user);
            await db.SaveChangesAsync();
            UsersHelper.CreateUserASP(user.Email, "User", user.Password);


            return CreatedAtRoute("DefaultApi", new { id = user.UserId }, user);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(int id)
        {
            return db.Users.Count(e => e.UserId == id) > 0;
        }
    }
}