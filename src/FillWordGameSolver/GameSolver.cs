using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FillWordGameSolver
{
    public class GameSolver : IGameSolver
    {
        private IWordStateMachineFactory wordStateMachineFactory;
        
        public GameSolver(IWordStateMachineFactory wordStateMachineFactory)
        {
            this.wordStateMachineFactory = wordStateMachineFactory;
        }

        public async Task<GameSolution> SolveGameAsync(GameInformation gameInformation, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await SolveGameAsync(CreateInitialGameState(gameInformation), cancellationToken);
        }

        public async Task<GameSolution> SolveGameAsync(GameState initialGameState, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await Task.Run(() =>
            {
                HashSet<GameState> gameStates = new HashSet<GameState>();
                Queue<GameState> gameStatesToProcess = new Queue<GameState>();
                IWordStateMachine wordStateMachine = this.wordStateMachineFactory.GetWordStateMachine();

                gameStates.Add(initialGameState);
                gameStatesToProcess.Enqueue(initialGameState);

                while (gameStatesToProcess.Count != 0)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    var currentGameState = gameStatesToProcess.Dequeue();
                    foreach (var newGameState in CreateGameStates(currentGameState, wordStateMachine))
                    {
                        if (gameStates.Add(newGameState))
                        {
                            gameStatesToProcess.Enqueue(newGameState);
                        }
                    }
                }
                return new GameSolution(gameStates.Where(gameState => IsGameStateFinal(gameState)));
            });
        }

        private IEnumerable<GameState> CreateGameStates(GameState gameState, IWordStateMachine wordStateMachine)
        {
            for (int index = 0; index < gameState.OccupationField.Length; index++)
            {
                GamePoint startGamePoint = new GamePoint(gameState.GameInformation, index);
                wordStateMachine.Reset();
                if (wordStateMachine.CanNavigateForward(startGamePoint.ToChar()) && !gameState.OccupationField[startGamePoint.Index])
                {
                    TemporaryGameState temporaryGameState = new TemporaryGameState(gameState, startGamePoint, wordStateMachine);
                    foreach (var newGameState in CreateGameStates(temporaryGameState))
                    {
                        yield return newGameState;
                    }
                }
            }
        }

        private IEnumerable<GameState> CreateGameStates(TemporaryGameState temporaryGameState)
        {
            foreach (NavigateDirection navigateDirection in Enum.GetValues(typeof(NavigateDirection)))
            {
                if (temporaryGameState.CanNavigate(navigateDirection))
                {
                    temporaryGameState.Navigate(navigateDirection);
                    foreach (var newGameState in CreateGameStates(temporaryGameState))
                    {
                        yield return newGameState;
                    }
                    temporaryGameState.NavigateBack();
                }
            }

            if (temporaryGameState.IsWordExist())
            {
                yield return temporaryGameState.GetNewGameState();
            }
        }

        private GameState CreateInitialGameState(GameInformation gameInformation)
        {
            return new GameState(gameInformation);
        }

        private bool IsGameStateFinal(GameState gameState)
        {
            bool result = true;
            for (int i = 0; i < gameState.OccupationField.Length; i++)
            {
                result &= gameState.OccupationField[i];
            }
            return result;
        }
    }
}
