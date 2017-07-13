using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace FillWordGameSolver
{
    public class GameInformation
    {
        public int HorizontalDimension { get; private set; }

        public int VerticalDimension { get; private set; }

        public ImmutableArray<char> FieldLetters { get; private set; }
                
        public GameInformation(char[] letters, int horizontalDimension, int verticalDimension)
        {
            this.FieldLetters = letters.ToImmutableArray();
            this.HorizontalDimension = horizontalDimension;
            this.VerticalDimension = verticalDimension;
        }
    }
}
