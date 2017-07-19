using System;
using System.Collections.Generic;
using System.Text;

namespace FillWordGameSolver
{
    public interface IWordStateMachine
    {
        void ResetToInitialState();
        bool IsWordExist();
        bool IsWordPartExist();
        bool CanNavigateForward(char letter);
        void NavigateForward(char letter);
        bool CanNavigateBack();
        void NavigateBack();
    }
}
