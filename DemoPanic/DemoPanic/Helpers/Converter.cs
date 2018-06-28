namespace DemoPanic.Helpers
{
    using System;
    using System.Globalization;
    using DemoPanic.Domain;
    using DemoPanic.Models;

    public static class Converter
    {
        public static UserLocal ToUserLocal(User user)
        {
            return new UserLocal
            {
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                ClientTypeId = user.ClientTypeId,
                UserId = user.UserId,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Telephone = user.Telephone,
                UserTypeId = user.UserTypeId
            };

        }

        public static User ToUserDomain(UserLocal user)
        {
            return new User
            {
                UserId = user.UserId,
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                Telephone = user.Telephone,
                UserTypeId = user.UserTypeId.Value,
                ClientTypeId = user.ClientTypeId.Value
            };
        }

        public static Ubication ToUserUbication(UserHelpRequest user)
        {
            NumberFormatInfo provider = new NumberFormatInfo();
            provider.NumberDecimalSeparator = ".";

            return new Ubication
            {
                UbicationId = user.UserId,
                Description = user.FullName,
                Latitude = Convert.ToDouble(user.Latitude, provider),
                Longitude = Convert.ToDouble(user.Longitude, provider),
                Phone = user.Telephone

            };
        }
    }
}
