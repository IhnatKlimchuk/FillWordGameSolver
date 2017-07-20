using System;
using System.Collections.Generic;
using System.Text;

namespace FillWordGameSolver
{
    public interface IWordStateMachineFactory
    {
        IWordStateMachine GetWordStateMachine();
    }
}
