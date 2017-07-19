using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FillWordGameSolver.Tests
{
    public class SimpleGameTests : TestBase
    {
        public SimpleGameTests() : base()
        {
            
        }

        [Fact]
        public void SimpleGameCanBeSolved()
        {
            string[] definedWords = new string[] { "word", "field", "work", "rock", "road" };
            char[] gameField = new char[]
            {
                'w', 'o', 'r',
                'i', 'f', 'k',
                'e', 'l', 'd'
            };
            GameInformation gameInfo = new GameInformation(gameField, 3, 3);
            SimpleWordStateMachine wordStateMachine = new SimpleWordStateMachine(definedWords);
            GameSolver gameSolver = new GameSolver(wordStateMachine, gameInfo);
            GameSolution solution = gameSolver.SolveGame();
            Assert.True(solution.IsSolved);

            GameWord[] gameWords = new GameWord[]
            {
                //work
                new GameWord(gameInfo, new GamePoint[]
                {
                    new GamePoint(gameInfo, 0, 0),
                    new GamePoint(gameInfo, 1, 0),
                    new GamePoint(gameInfo, 2, 0),
                    new GamePoint(gameInfo, 2, 1)
                }),
                //field
                new GameWord(gameInfo, new GamePoint[]
                {
                    new GamePoint(gameInfo, 1, 1),
                    new GamePoint(gameInfo, 0, 1),
                    new GamePoint(gameInfo, 0, 2),
                    new GamePoint(gameInfo, 1, 2),
                    new GamePoint(gameInfo, 2, 2)
                })
            };
            
            GameState expectedGameState = new GameState(gameInfo, gameWords);
            Assert.True(expectedGameState.Equals(solution.FinalGameState));
        }
    }
}
