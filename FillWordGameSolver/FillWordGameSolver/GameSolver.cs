using System;
using System.Collections.Generic;
using System.Text;

namespace FillWordGameSolver
{
    public class GameSolver
    {
        private IWordStateMachine wordStateMachine;

        public GameInformation GameInformation { get; private set; }
        
        public GameSolver(IWordStateMachine wordStateMachine, GameInformation gameInformation)
        {
            this.wordStateMachine = wordStateMachine;
            this.GameInformation = gameInformation;
        }
        
        public GameSolution SolveGame()
        {
            return new GameSolution(null);
        }
    }
}
