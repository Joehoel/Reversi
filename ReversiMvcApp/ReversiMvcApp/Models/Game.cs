
using System.ComponentModel.DataAnnotations;

namespace ReversiMvcApp.Models
{
    public enum Color { None, White, Black };
    public class Game
    {

        [Key]
        public string Token { get; set; }
        public string Description { get; set; }
        public string Player1Token { get; set; }
        public string Player2Token { get; set; }
        public Color[,] Board { get; set; }
        public Color TurnColor { get; set; }

    }
}

