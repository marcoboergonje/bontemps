﻿using System;
using System.Collections.Generic;
using System.Text;
using BonTemps.Areas.Systeem.Models;
using BonTemps.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BonTemps.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }       
        
        public DbSet<Klant> Klanten { get; set; }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Klantgegevens> Klantgegevens { get; set; }

        //Andere area
        public DbSet<Reservering> Reserveringen { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Consumptie> Consumpties { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
