﻿namespace DemoPanic.Backend.Models
{
    using Domain;

    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<DemoPanic.Domain.User> Users { get; set; }
    }
}