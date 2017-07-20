using System.Collections.Generic;

namespace FillWordGameSolver
{
    public class SimpleWordStateMachineState
    {
        public SimpleWordStateMachineState(SimpleWordStateMachineState previousState)
        {
            this.previousState = previousState;
        }

        public bool IsWordEnd { get; set; }

        public SimpleWordStateMachineState previousState;

        public Dictionary<char, SimpleWordStateMachineState> letterStateDictionary = new Dictionary<char, SimpleWordStateMachineState>();
    }
}
