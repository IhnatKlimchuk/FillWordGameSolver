using System;
using System.Collections.Generic;
using System.Text;

namespace FillWordGameSolver
{
    public class GameSolution
    {
        public bool IsSolved { get; private set; }

        public GameState FinalGameState { get; private set; }

        public GameSolution(GameState finalGameState)
        {
            this.FinalGameState = finalGameState;
            this.IsSolved = finalGameState != null;
        }
    }
}
