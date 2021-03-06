﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BonTemps.Models
{
    public class Klant : IdentityUser
    {
        public Klant() : base()
        {

        }
        public string KlantNaam { get; set; }
        [ForeignKey("RolId")]
        public Rol Rol { get; set; }
        public string Rolnaam { get; set; }
        public Klantgegevens Klantgegevens { get; set; }

        public DateTime Aanmaakdatum { get; set; }
    }
}
