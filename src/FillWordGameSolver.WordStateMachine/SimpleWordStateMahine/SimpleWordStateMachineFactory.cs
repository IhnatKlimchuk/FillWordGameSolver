namespace FillWordGameSolver
{
    public class SimpleWordStateMachineFactory : IWordStateMachineFactory
    {
        private string[] words;

        public SimpleWordStateMachineFactory(string[] words)
        {
            this.words = words;
        }

        public IWordStateMachine GetWordStateMachine()
        {
            return new SimpleWordStateMachineClient(words);
        }
    }
}
