using System.Collections.Generic;
using System.Linq;

namespace FillWordGameSolver
{
    public class GameSolution
    {
        public GameSolution(IEnumerable<GameState> finalGameStates)
        {
            if (finalGameStates == null)
            {
                this.GameStates = Enumerable.Empty<GameState>();
            }
            else
            {
                this.GameStates = finalGameStates.Where(t => t != null).ToList();
            }
            this.IsSolved = this.GameStates.Any();
        }

        public IEnumerable<GameState> GameStates { get; private set; }

        public bool IsSolved { get; private set; }
    }
}
