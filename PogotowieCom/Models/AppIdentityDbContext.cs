using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PogotowieCom.Models
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {

        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {

        }

        public AppIdentityDbContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PatientAppointment>().HasKey(sc => new { sc.PatientId, sc.AppointmentId });
            modelBuilder.Entity<DoctorSpecialization>().HasKey(sc => new { sc.DoctorId, sc.SpecializationId });
            modelBuilder.Entity<TagSpecialization>().HasKey(sc => new { sc.SpecializationId, sc.TagId });
        }



        public DbSet<Place> Places { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagSpecialization> TagSpecializations { get; set; }
        public DbSet<Comment> Comments { get; set; }





    }
}
