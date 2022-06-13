using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReversiMvcApp.Models
{
    public class Player
    {
        [Key]
        public string Guid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }   
        [DefaultValue(Color.None)]
        public Color Color { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Draws { get; set; }

    }
}
