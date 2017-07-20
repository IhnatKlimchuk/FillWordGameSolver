using System.Collections.Generic;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FillWordGameSolver
{
    public class ParallelGameSolver
    {
        private GameState initialState;

        private IWordStateMachineFactory wordStateMachineFactory;
        
        public ParallelGameSolver(IWordStateMachineFactory wordStateMachineFactory, GameInformation gameInformation)
        {
            this.wordStateMachineFactory = wordStateMachineFactory;
            this.GameInformation = gameInformation;
            this.initialState = CreateInitialGameState(gameInformation);
        }

        public ParallelGameSolver(IWordStateMachineFactory wordStateMachineFactory, GameState initialState)
        {
            this.wordStateMachineFactory = wordStateMachineFactory;
            this.GameInformation = initialState.GameInformation;
            this.initialState = initialState;
        }

        public GameInformation GameInformation { get; private set; }

        private IEnumerable<GameState> GenerateNewGameStates(GameState gameState, IWordStateMachineFactory wordStateMachineFactory)
        {
            IWordStateMachine wordStateMachine = wordStateMachineFactory.GetWordStateMachine();
            for (int index = 0; index < GameInformation.FieldLetters.Length; index++)
            {
                GamePoint startGamePoint = new GamePoint(GameInformation, index);
                wordStateMachine.Reset();
                if (wordStateMachine.CanNavigateForward(startGamePoint.ToChar()) && !gameState.OccupationField[startGamePoint.Index])
                {
                    TempGameState tempGameState = new TempGameState(gameState, startGamePoint, wordStateMachine);
                    foreach (var newGamestate in CreateGameStates(tempGameState))
                    {
                        yield return newGamestate;
                    }
                }
            }
        }

        public GameSolution SolveGame(CancellationToken cancellationToken)
        {
            var initialGameState = this.initialState ?? CreateInitialGameState(this.GameInformation);
            return SolveGameParallel(initialGameState, this.wordStateMachineFactory, cancellationToken);
        }

        private GameSolution SolveGameParallel(GameState initialGameState, IWordStateMachineFactory wordStateMachineFactory, CancellationToken cancellationToken)
        {
            ConcurrentDictionary<GameState, bool> gameStates = new ConcurrentDictionary<GameState, bool>();

            gameStates.TryAdd(initialGameState, false);
            //Task.Run(() => ProcessGameState(initialGameState, gameStates, wordStateMachineFactory, cancellationToken), cancellationToken);
            ProcessGameState(initialGameState, gameStates, wordStateMachineFactory, cancellationToken);
            
            var results = gameStates.Where(t => IsGameStateFinal(t.Key)).ToList();
            return null;
        }

        private void ProcessGameState(GameState gameState, ConcurrentDictionary<GameState, bool> gameStates, IWordStateMachineFactory wordStateMachineFactory, CancellationToken cancellationToken)
        {
            //List<Task> tasks = new List<Task>();
            foreach (var newGameState in GenerateNewGameStates(gameState, wordStateMachineFactory))
            {
                if (gameStates.TryAdd(newGameState, false))
                {
                    //tasks.Add(
                    //Task.Run(() => ProcessGameState(newGameState, gameStates, wordStateMachineFactory, cancellationToken), cancellationToken);
                    ProcessGameState(newGameState, gameStates, wordStateMachineFactory, cancellationToken);
                    //);
                }
            }
            //Task.WaitAll(tasks.ToArray());
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

        private IEnumerable<GameState> CreateGameStates(TempGameState tempGameState)
        {
            if (tempGameState.CanNavigateLeft())
            {
                tempGameState.NavigateLeft();
                foreach (var newGameState in CreateGameStates(tempGameState))
                {
                    yield return newGameState;
                }
                tempGameState.NavigateBack();
            }
            if (tempGameState.CanNavigateUp())
            {
                tempGameState.NavigateUp();
                foreach (var newGameState in CreateGameStates(tempGameState))
                {
                    yield return newGameState;
                }
                tempGameState.NavigateBack();
            }
            if (tempGameState.CanNavigateRight())
            {
                tempGameState.NavigateRight();
                foreach (var newGameState in CreateGameStates(tempGameState))
                {
                    yield return newGameState;
                }
                tempGameState.NavigateBack();
            }
            if (tempGameState.CanNavigateDown())
            {
                tempGameState.NavigateDown();
                foreach (var newGameState in CreateGameStates(tempGameState))
                {
                    yield return newGameState;
                }
                tempGameState.NavigateBack();
            }

            if (tempGameState.IsWordExist())
            {
                yield return tempGameState.GetNewGameState();
            }
        }

        private GameState CreateInitialGameState(GameInformation gameInformation)
        {
            return new GameState(gameInformation);
        }
    }
}
