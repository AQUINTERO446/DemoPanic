namespace DemoPanic.Helpers
{
    using System;
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
                Email = user.Email,
                LastName = user.LastName,
                FirstName = user.FirstName,
                ClientTypeId = user.ClientTypeId,
                UserId = user.UserId,
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Telephone = user.Telephone,
                UserTypeId = user.UserTypeId.Value
            };
        }
    }
}
