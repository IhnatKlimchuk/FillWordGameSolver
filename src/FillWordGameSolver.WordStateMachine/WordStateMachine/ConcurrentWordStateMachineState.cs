using System.Collections.Generic;

namespace FillWordGameSolver
{
    internal class ConcurrentWordStateMachineState
    {
        public ConcurrentWordStateMachineState(ConcurrentWordStateMachineState previousState, char letter)
        {
            this.PreviousState = previousState;
            this.LetterDictionary = new Dictionary<char, ConcurrentWordStateMachineState>();
            this.Letter = letter;
        }

        public char Letter { get; set; }

        public bool IsWordEnd { get; set; }

        public ConcurrentWordStateMachineState PreviousState { get; private set; }

        public Dictionary<char, ConcurrentWordStateMachineState> LetterDictionary { get; private set; }
    }
}
