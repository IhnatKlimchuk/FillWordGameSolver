using System;

namespace FillWordGameSolver
{
    public class GamePoint
    {
        public GamePoint(GameInformation gameInformation, int index)
        {
            if (index < 0)
            {
                throw new ArgumentException();
            }
            if (index >= gameInformation.FieldLetters.Length)
            {
                throw new ArgumentException();
            }

            this.GameInformation = gameInformation;
            this.Index = index;
        }

        public GamePoint(GameInformation gameInformation, int xHorizontal, int yVertical)
        {
            if (xHorizontal < 0)
            {
                throw new ArgumentException();
            }
            if (xHorizontal >= gameInformation.HorizontalDimension)
            {
                throw new ArgumentException();
            }
            if (yVertical < 0)
            {
                throw new ArgumentException();
            }
            if (yVertical >= gameInformation.VerticalDimension)
            {
                throw new ArgumentException();
            }

            this.GameInformation = gameInformation;
            this.Index = gameInformation.HorizontalDimension * yVertical + xHorizontal;
        }

        public GameInformation GameInformation { get; private set; }

        public int Index { get; private set; }

        public char ToChar()
        {
            return GameInformation.FieldLetters[Index];
        }

        public override string ToString()
        {
            return new string(GameInformation.FieldLetters[Index], 1);
        }
    }
}
