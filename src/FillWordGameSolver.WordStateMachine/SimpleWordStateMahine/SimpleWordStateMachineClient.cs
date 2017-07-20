namespace FillWordGameSolver
{
    public class SimpleWordStateMachineClient : IWordStateMachine
    {
        private SimpleWordStateMachineState initialState;
        private SimpleWordStateMachineState currentState;

        public SimpleWordStateMachineClient(string[] words)
        {
            initialState = new SimpleWordStateMachineState(null);
            currentState = initialState;
            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    if (currentState.letterStateDictionary.ContainsKey(letter))
                    {
                        currentState = currentState.letterStateDictionary[letter];
                    }
                    else
                    {
                        var newState = new SimpleWordStateMachineState(currentState);
                        currentState.letterStateDictionary.Add(letter, newState);
                        currentState = newState;
                    }
                }
                currentState.IsWordEnd = true;
                currentState = initialState;
            }
        }

        public bool CanNavigateBack()
        {
            return currentState.previousState != null;
        }

        public bool CanNavigateForward(char letter)
        {
            return currentState.letterStateDictionary.ContainsKey(letter);
        }

        public bool IsWordExist()
        {
            return currentState.IsWordEnd;
        }

        public bool IsWordPartExist()
        {
            return currentState.letterStateDictionary.Count > 0;
        }

        public void NavigateBack()
        {
            currentState = currentState.previousState;
        }

        public void NavigateForward(char letter)
        {
            currentState = currentState.letterStateDictionary[letter];
        }

        public void Reset()
        {
            currentState = initialState;
        }
    }
}
