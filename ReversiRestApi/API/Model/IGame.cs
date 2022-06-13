namespace API.Model
{
    public enum Color { None, White, Black };
    public interface IGame
    {
        int ID { get; set; }
        string Description { get; set; }

        //het unieke token van het spel
        string Token { get; set; }
        string Player1Token { get; set; }
        string Player2Token { get; set; }

        Color[,] Board { get; set; }
        Color TurnColor { get; set; }
        void Pass();

        //welke kleur het meest voorkomend op het speelbord
        Color DominantColor();

        //controle of op een bepaalde positie een zet mogelijk is
        bool TurnPossible(int row, int column);
        void Move(int row, int column);

        bool HasPlayer(string playerToken);
        bool isFull();
        bool HasEnded();
    }
}
