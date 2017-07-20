using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

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
            HashSet<GameState> allStates = new HashSet<GameState>();
            Queue<GameState> statesToProcess = new Queue<GameState>();
            var initialGameState = new GameState(GameInformation);

            allStates.Add(initialGameState);
            statesToProcess.Enqueue(initialGameState);
            while (statesToProcess.Count != 0)
            {
                var gameState = statesToProcess.Dequeue();
                var newStates = CreateGameStates(gameState);
                foreach (var state in newStates)
                {
                    if (allStates.Add(state))
                    {
                        statesToProcess.Enqueue(state);
                    }
                }
            }
            var solutionState = allStates.FirstOrDefault(t => t.IsFinal);
            return new GameSolution(solutionState);
        }

        public List<GameState> CreateGameStates(GameState gameState)
        {
            bool filled = true;
            List<GameState> newGameStates = new List<GameState>();
            for (int i = 0; i < GameInformation.HorizontalDimension; i++)
            {
                for (int j = 0; j < GameInformation.VerticalDimension; j++)
                {
                    GamePoint startGamePoint = new GamePoint(GameInformation, i, j);
                    filled &= gameState.OccupationField[startGamePoint.Index];
                    wordStateMachine.Reset();
                    if (wordStateMachine.CanNavigateForward(startGamePoint.ToChar()) && !gameState.OccupationField[startGamePoint.Index])
                    {
                        TempGameState tempGameState = new TempGameState(gameState, startGamePoint, wordStateMachine);
                        newGameStates.AddRange(CreateGameStates(tempGameState));
                    }
                }
            }
            gameState.IsFinal = filled;
            return newGameStates;
        }

        private List<GameState> CreateGameStates(TempGameState tempGameState)
        {
            List<GameState> newGameStates = new List<GameState>();
            if (tempGameState.CanNavigateLeft())
            {
                tempGameState.NavigateLeft();
                newGameStates.AddRange(CreateGameStates(tempGameState));
                tempGameState.NavigateBack();
            }
            if (tempGameState.CanNavigateUp())
            {
                tempGameState.NavigateUp();
                newGameStates.AddRange(CreateGameStates(tempGameState));
                tempGameState.NavigateBack();
            }
            if (tempGameState.CanNavigateRight())
            {
                tempGameState.NavigateRight();
                newGameStates.AddRange(CreateGameStates(tempGameState));
                tempGameState.NavigateBack();
            }
            if (tempGameState.CanNavigateDown())
            {
                tempGameState.NavigateDown();
                newGameStates.AddRange(CreateGameStates(tempGameState));
                tempGameState.NavigateBack();
            }

            if (tempGameState.IsWordExist())
            {
                newGameStates.Add(tempGameState.GetNewGameState());
            }

            return newGameStates;
        }
    }
}
