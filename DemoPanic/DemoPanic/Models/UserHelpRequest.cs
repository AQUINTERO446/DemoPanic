using System;
using System.Collections.Generic;
using System.Text;

namespace DemoPanic.Models
{
    public class UserHelpRequest
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Telephone { get; set; }

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
