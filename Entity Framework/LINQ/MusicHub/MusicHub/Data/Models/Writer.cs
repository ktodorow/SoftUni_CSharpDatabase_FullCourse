﻿using System.ComponentModel.DataAnnotations;

namespace MusicHub.Data.Models
{
    public class Writer
    {
        public Writer()
        {
            Songs = new HashSet<Song>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        public string? Pseudonym { get; set; }

        //collection of Songs
        public virtual ICollection<Song> Songs { get; set; } 
    }
}
