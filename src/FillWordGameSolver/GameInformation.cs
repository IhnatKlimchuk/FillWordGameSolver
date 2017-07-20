using System;
using System.Collections.Immutable;

namespace FillWordGameSolver
{
    public class GameInformation
    {
        public GameInformation(char[] letters, int horizontalDimension, int verticalDimension)
        {
            if (horizontalDimension < 1)
            {
                throw new ArgumentException();
            }
            if (verticalDimension < 1)
            {
                throw new ArgumentException();
            }
            if (horizontalDimension * verticalDimension != letters.Length)
            {
                throw new ArgumentException();
            }

            this.FieldLetters = letters.ToImmutableArray();
            this.HorizontalDimension = horizontalDimension;
            this.VerticalDimension = verticalDimension;
        }

        public ImmutableArray<char> FieldLetters { get; private set; }

        public int HorizontalDimension { get; private set; }

        public int VerticalDimension { get; private set; }
    }
}
