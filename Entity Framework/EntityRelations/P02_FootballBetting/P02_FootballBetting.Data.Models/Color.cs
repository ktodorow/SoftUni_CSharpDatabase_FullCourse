﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P02_FootballBetting.Data.Models
{
    public class Color
    {
        public Color()
        {
            PrimaryKitTeams = new HashSet<Team>();
            SecondaryKitTeams = new HashSet<Team>();
        }

        [Key]
        public int ColorId { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public virtual ICollection<Team> PrimaryKitTeams { get; set; }
        public virtual ICollection<Team> SecondaryKitTeams { get; set; }

    }
}
