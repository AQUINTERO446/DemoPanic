using DemoPanic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DemoPanic.Backend.Models
{
    public class LocalDataContext : DataContext
    {
        public System.Data.Entity.DbSet<DemoPanic.Domain.User> Users { get; set; }
    }
}