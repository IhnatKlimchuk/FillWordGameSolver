namespace FillWordGameSolver
{
    public class ConcurrentWordStateMachineClient : IWordStateMachine
    {
        private ConcurrentWordStateMachineState currentState;
        private ConcurrentWordStateMachine stateMachine;

        public ConcurrentWordStateMachineClient(ConcurrentWordStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
            this.currentState = stateMachine.InitialState;
        }

        public bool CanNavigateBack()
        {
            return currentState.PreviousState != null;
        }

        public bool CanNavigateForward(char letter)
        {
            return currentState.LetterDictionary.ContainsKey(letter);
        }

        public bool IsWordExist()
        {
            return currentState.IsWordEnd;
        }

        public bool IsWordPartExist()
        {
            return currentState.LetterDictionary.Count > 0;
        }

        public void NavigateBack()
        {
            currentState = currentState.PreviousState;
        }

        public void NavigateForward(char letter)
        {
            currentState = currentState.LetterDictionary[letter];
        }

        public void Reset()
        {
            currentState = stateMachine.InitialState;
        }
    }
}
