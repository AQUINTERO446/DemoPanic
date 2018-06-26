namespace DemoPanic.Models
{
    using SQLite.Net.Attributes;

    public class UserLocal
    {
        [PrimaryKey]
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Telephone { get; set; }

        public int? UserTypeId { get; set; }

        public int? ClientTypeId { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        public override int GetHashCode()
        {
            return UserId;
        }
    }
}
