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

        public ConcurrentWordStateMachineState(ConcurrentWordStateMachineState previousState)
        {
            this.PreviousState = previousState;
            this.LetterDictionary = new Dictionary<char, ConcurrentWordStateMachineState>();
        }

        public bool IsWordEnd { get; set; }
        public char Letter { get; private set; }
        public Dictionary<char, ConcurrentWordStateMachineState> LetterDictionary { get; private set; }
        public ConcurrentWordStateMachineState PreviousState { get; private set; }
    }
}
