using System.Threading;
using System.Threading.Tasks;

namespace FillWordGameSolver
{
    public interface IGameSolver
    {
        Task<GameSolution> SolveGameAsync(GameInformation gameInformation, CancellationToken cancellationToken = default(CancellationToken));
        Task<GameSolution> SolveGameAsync(GameState gameState, CancellationToken cancellationToken = default(CancellationToken));
    }
}
