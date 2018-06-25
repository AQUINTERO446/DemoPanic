namespace DemoPanic.Backend.Models
{
    using Domain;

    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<DemoPanic.Domain.User> Users { get; set; }

        public System.Data.Entity.DbSet<DemoPanic.Domain.UserType> UserTypes { get; set; }

        public System.Data.Entity.DbSet<DemoPanic.Domain.ClientType> ClientTypes { get; set; }
    }
}