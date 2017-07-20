namespace FillWordGameSolver
{
    public interface IWordStateMachine
    {
        bool CanNavigateBack();

        bool CanNavigateForward(char letter);

        bool IsWordExist();

        bool IsWordPartExist();

        void NavigateBack();

        void NavigateForward(char letter);

        void Reset();
    }
}
