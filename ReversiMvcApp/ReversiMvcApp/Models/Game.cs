
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
        public Color? Winner { get; set; }
        public int BlackCount { get; set; }
        public int WhiteCount { get; set; }
        public override string ToString()
        {
            return $"{nameof(Game)}: {Token} - {Player1Token} - {Player2Token}";
        }
    }
}

