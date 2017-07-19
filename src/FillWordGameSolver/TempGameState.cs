using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections;

namespace FillWordGameSolver
{
    public class TempGameState
    {
        private Stack<GamePoint> gamePoints = new Stack<GamePoint>();

        protected GameInformation GameInformation;

        protected GameState InitialGameState;

        protected GamePoint InitialPoint;

        private GamePoint currentPoint;

        private BitArray gameOccupationField;

        private IWordStateMachine wordStateMachine;

        public TempGameState(GameState initialGameState, GamePoint initialPoint, IWordStateMachine wordStateMachine)
        {
            this.InitialGameState = initialGameState;
            this.gamePoints.Push(initialPoint);
            this.InitialPoint = initialPoint;
            this.currentPoint = initialPoint;
            this.gameOccupationField = new BitArray(initialGameState.OccupationField);
            this.gameOccupationField[currentPoint.Index] = true;
            this.GameInformation = initialGameState.GameInformation;
            this.wordStateMachine = wordStateMachine;
            this.wordStateMachine.ResetToInitialState();
            this.wordStateMachine.NavigateForward(initialPoint.ToChar());
        }

        public bool IsWordExist()
        {
            return wordStateMachine.IsWordExist();
        }

        public bool CanNavigateUp()
        {
            int newIndex = currentPoint.Index - GameInformation.HorizontalDimension;
            return (newIndex >= 0 && !gameOccupationField[newIndex]) 
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public void NavigateUp()
        {
            int newIndex = currentPoint.Index - GameInformation.HorizontalDimension;
            gameOccupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public bool CanNavigateDown()
        {
            int newIndex = currentPoint.Index + GameInformation.HorizontalDimension;
            return (newIndex < GameInformation.FieldLetters.Length && !gameOccupationField[newIndex]) 
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public void NavigateDown()
        {
            int newIndex = currentPoint.Index + GameInformation.HorizontalDimension;
            gameOccupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public bool CanNavigateLeft()
        {
            int newIndex = currentPoint.Index - 1;
            return (currentPoint.Index % GameInformation.HorizontalDimension != 0 && !gameOccupationField[newIndex])
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public void NavigateLeft()
        {
            int newIndex = currentPoint.Index - 1;
            gameOccupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public bool CanNavigateRight()
        {
            int newIndex = currentPoint.Index + 1;
            return (newIndex % GameInformation.HorizontalDimension != 0 && !gameOccupationField[newIndex])
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public void NavigateRight()
        {
            int newIndex = currentPoint.Index + 1;
            gameOccupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public void NavigateBack()
        {
            var prevPoint = gamePoints.Pop();
            currentPoint = gamePoints.Peek();
            gameOccupationField[prevPoint.Index] = false;
            wordStateMachine.NavigateBack();
        }

        public GameState GetNewGameState()
        {
            return new GameState(GameInformation, InitialGameState.Words.Add(GetCurrentWord()));
        }

        private GameWord GetCurrentWord()
        {
            return new GameWord(GameInformation, gamePoints.Reverse());
        }
    }
}
