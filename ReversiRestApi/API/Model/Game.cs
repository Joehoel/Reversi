using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace API.Model
{
    public class Game : IGame
    {
        private const int boardSize = 8;

        private readonly int[,] direction = new int[8, 2] {
                                {  0,  1 },         // naar rechts
                                {  0, -1 },         // naar links
                                {  1,  0 },         // naar onder
                                { -1,  0 },         // naar boven
                                {  1,  1 },         // naar rechtsonder
                                {  1, -1 },         // naar linksonder
                                { -1,  1 },         // naar rechtsboven
                                { -1, -1 } };       // naar linksboven

        public int ID { get; set; }
        public string Description { get; set; }
        public string Token { get; set; }
        public string Player1Token { get; set; }
        public string Player2Token { get; set; }

        private Color[,] _board;


        public Color[,] Board
        {
            get
            {
                return _board;
            }
            set
            {
                _board = value;
            }
        }
        public Color TurnColor { get; set; }

        public Game()
        {
            Token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            Token = Token.Replace("/", "q");    // slash mijden ivm het opvragen van een spel via een api obv het token
            Token = Token.Replace("+", "r");    // plus mijden ivm het opvragen van een spel via een api obv het token

            Board = new Color[boardSize, boardSize];
            Board[3, 3] = Color.White;
            Board[4, 4] = Color.White;
            Board[3, 4] = Color.Black;
            Board[4, 3] = Color.Black;

            TurnColor = Color.None;
        }

        public void Pass()
        {
            // controleeer of er geen zet mogelijk is voor de speler die wil passen, alvorens van beurt te wisselen.
            if (TurnPossible(TurnColor))
                throw new Exception("Passen mag niet, er is nog een zet mogelijk");
            else
                SwitchTurn();
        }


        public bool GameOver()     // return true als geen van de spelers een zet kan doen
        {
            return !(TurnPossible(Color.White) && TurnPossible(Color.Black));
        }

        public Color DominantColor()
        {
            int whiteCount = 0;
            int blackCount = 0;
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (_board[row, column] == Color.White)
                        whiteCount++;
                    else if (_board[row, column] == Color.Black)
                        blackCount++;
                }
            }
            if (whiteCount > blackCount)
                return Color.White;
            if (blackCount > whiteCount)
                return Color.Black;
            return Color.None;
        }

        public bool TurnPossible(int row, int column)
        {
            if (!PositionWithinBounds(row, column))
                throw new Exception($"Zet ({row},{column}) ligt buiten het bord!");
            return TurnPossible(row, column, TurnColor);
        }

        public void Move(int row, int column)
        {
            if (!TurnPossible(row, column))
            {
                throw new Exception($"Zet ({row},{column}) is niet mogelijk!");
            }


            for (int i = 0; i < 8; i++)
            {
                TurnStonesWhenEnclosed(row, column, TurnColor, direction[i, 0], direction[i, 1]);
            }

            Board[row, column] = TurnColor;

            TurnColor = GetOpponentColor(TurnColor);
        }

        private static Color GetOpponentColor(Color kleur)
        {
            if (kleur == Color.White)
                return Color.Black;
            else if (kleur == Color.Black)
                return Color.White;
            else
                return Color.None;
        }

        private bool TurnPossible(Color color)
        {
            if (color == Color.None)
                throw new Exception("Kleur mag niet gelijk aan Geen zijn!");
            // controleeer of er een zet mogelijk is voor kleur
            for (int row = 0; row < boardSize; row++)
            {
                for (int column = 0; column < boardSize; column++)
                {
                    if (TurnPossible(row, column, color))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TurnPossible(int row, int column, Color color)
        {
            // Check of er een richting is waarin een zet mogelijk is. Als dat zo is, return dan true.
            for (int i = 0; i < 8; i++)
            {
                {
                    if (EnclosableInDirection(row, column, color, direction[i, 0], direction[i, 1]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private Color SwitchTurn()
        {
            return TurnColor == Color.White ? TurnColor = Color.Black : TurnColor = Color.White;
        }

        private static bool PositionWithinBounds(int row, int column)
        {
            return (row >= 0 && row < boardSize &&
                    column >= 0 && column < boardSize);
        }

        private bool PutOnBoardAndFree(int row, int column)
        {
            // Als op het bord gezet wordt, en veld nog vrij, dan return true, anders false
            return (PositionWithinBounds(row, column) && Board[row, column] == Color.None);
        }

        private bool EnclosableInDirection(int row, int column, Color colorTurn, int rowDirection, int columnDirection)
        {
            int rij, kolom;
            Color opponentColor = GetOpponentColor(colorTurn);
            if (!PutOnBoardAndFree(row, column))
            {
                return false;
            }

            // Zet rij en kolom op de index voor het eerst vakje naast de zet.
            rij = row + rowDirection;
            kolom = column + columnDirection;

            int aantalNaastGelegenStenenVanTegenstander = 0;

            // Zolang Bord[rij,kolom] niet buiten de bordgrenzen ligt, en je in het volgende vakje
            // steeds de kleur van de tegenstander treft, ga je nog een vakje verder kijken.
            // Bord[rij, kolom] ligt uiteindelijk buiten de bordgrenzen, of heeft niet meer de
            // de kleur van de tegenstander.
            // N.b.: deel achter && wordt alleen uitgevoerd als conditie daarvoor true is.
            while (PositionWithinBounds(rij, kolom) && Board[rij, kolom] == opponentColor)
            {
                rij += rowDirection;
                kolom += columnDirection;
                aantalNaastGelegenStenenVanTegenstander++;
            }

            // Nu kijk je hoe je geeindigt bent met bovenstaande loop. Alleen
            // als alle drie onderstaande condities waar zijn, zijn er in de
            // opgegeven richting stenen in te sluiten.
            return (PositionWithinBounds(rij, kolom) &&
                    Board[rij, kolom] == colorTurn &&
                    aantalNaastGelegenStenenVanTegenstander > 0);
        }

        private bool TurnStonesWhenEnclosed(int row, int column, Color colorTurn, int rijRichting, int kolomRichting)
        {
            int rij, kolom;
            Color kleurTegenstander = GetOpponentColor(colorTurn);
            bool stonesTurned = false;

            if (EnclosableInDirection(row, column, colorTurn, rijRichting, kolomRichting))
            {
                rij = row + rijRichting;
                kolom = column + kolomRichting;

                // N.b.: je weet zeker dat je niet buiten het bord belandt,
                // omdat de stenen van de tegenstander ingesloten zijn door
                // een steen van degene die de zet doet.
                while (Board[rij, kolom] == kleurTegenstander)
                {
                    Board[rij, kolom] = colorTurn;
                    rij += rijRichting;
                    kolom += kolomRichting;
                }
                stonesTurned = true;
            }
            return stonesTurned;
        }


    }

}
