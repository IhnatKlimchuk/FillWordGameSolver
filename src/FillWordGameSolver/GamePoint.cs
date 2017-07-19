namespace FillWordGameSolver
{
    public class GamePoint
    {
        public GameInformation GameInformation { get; private set; }

        public int Index { get; private set; }

        public GamePoint(GameInformation gameInformation, int index)
        {
            this.GameInformation = gameInformation;
            this.Index = index;
        }

        public GamePoint(GameInformation gameInformation, int xHorizontal, int yVertical)
            : this(gameInformation, gameInformation.HorizontalDimension * yVertical + xHorizontal)
        {

        }
        
        public override string ToString()
        {
            return new string(GameInformation.FieldLetters[Index], 1);
        }

        public char ToChar()
        {
            return GameInformation.FieldLetters[Index];
        }
    }
}
