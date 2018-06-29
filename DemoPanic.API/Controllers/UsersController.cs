namespace DemoPanic.API.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
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
        [Authorize]
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

            double radius = (double) Convert.ToDouble(radiusParameter.Parameter);
            double range = (double) Convert.ToDouble(rangeParameter.Parameter);
            double minimumServices = (double)Convert.ToDouble(minimumServicesParameter.Parameter);

            var user = await db.Users.
                Where(u => u.ClientTypeId == clientTypeId).
                    ToArrayAsync();

            if (user== null)
            {
                return null;
            }

            List<User> userOut = new List<User>();
            IEnumerator indexUser = user.GetEnumerator();

            while (userOut.Count < minimumServices && radius < range)
            {
                indexUser.MoveNext();
                if (!indexUser.MoveNext())
                {
                    indexUser.Reset();
                    radius+=radius;
                }
                else
                {
                    User help = (User)indexUser.Current;
                    if (DistanceCalculation.GeoCodeCalc.CalcDistance(
                        (double)help.Latitude,
                        (double)help.Longitude,
                        (double)latitud,
                        (double)longitud,
                        DistanceCalculation.GeoCodeCalcMeasurement.Kilometers) <
                    radius)
                    {
                        userOut.Add(help);
                    }
                }
                

            }
            return Ok(userOut);
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