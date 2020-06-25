using System;
using System.Collections.Generic;
using System.Text;
using DonLEonFilms.Data.Entitys;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DonLEonFilms.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options )
            : base( options )
        {
        }

        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            base.OnModelCreating( modelBuilder );

            modelBuilder.Entity<Film>()
                        .HasOne( x => x.ApplicationUser )
                        .WithMany( x => x.Films );
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Film> Films
        {
            get;
            set;
        }
    }
}
