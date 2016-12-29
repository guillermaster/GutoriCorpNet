using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using GutoriCorp.Models;
using GutoriCorp.Data.Models;
using GutoriCorp.Models.GeneralViewModels;
using Microsoft.EntityFrameworkCore;

namespace GutoriCorp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public virtual DbSet<Contract> Contract { get; set; }
        public virtual DbSet<GeneralCatalogValue> GeneralCatalogValues { get; set; }
        public virtual DbSet<Owner> Owner { get; set; }
        public virtual DbSet<Driver> Driver { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }
        public virtual DbSet<VehicleMake> VehicleMake { get; set; }
        public virtual DbSet<VehicleMakeModel> VehicleMakeModel { get; set; }
        public virtual DbSet<SystemUser> SystemUser { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<Ticket> Ticket { get; set; }
    }
}
