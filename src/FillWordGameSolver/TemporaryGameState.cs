using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FillWordGameSolver
{
    internal class TemporaryGameState
    {
        private GamePoint currentPoint;

        private Stack<GamePoint> gamePoints;

        private BitArray occupationField;

        private IWordStateMachine wordStateMachine;

        public TemporaryGameState(GameState initialGameState, GamePoint initialPoint, IWordStateMachine wordStateMachine)
        {
            this.wordStateMachine = wordStateMachine;
            this.wordStateMachine.Reset();
            this.wordStateMachine.NavigateForward(initialPoint.ToChar());

            this.gamePoints = new Stack<GamePoint>();
            this.gamePoints.Push(initialPoint);
            
            this.GameInformation = initialGameState.GameInformation;
            
            this.GameState = initialGameState;

            this.InitialPoint = initialPoint;
            this.currentPoint = initialPoint;

            this.occupationField = new BitArray(initialGameState.OccupationField)
            {
                [currentPoint.Index] = true
            };
        }

        public GameInformation GameInformation { get; private set; }

        public GameState GameState { get; private set; }

        public GamePoint InitialPoint { get; private set; }

        public bool CanNavigate(NavigateDirection navigateDirection)
        {
            switch (navigateDirection)
            {
                case NavigateDirection.Left:
                    return CanNavigateLeft();
                case NavigateDirection.Up:
                    return CanNavigateUp();
                case NavigateDirection.Right:
                    return CanNavigateRight();
                case NavigateDirection.Down:
                    return CanNavigateDown();
                default:
                    throw new NotSupportedException($"{navigateDirection} is not suppoted.");
            }
        }

        private bool CanNavigateDown()
        {
            int newIndex = currentPoint.Index + GameInformation.HorizontalDimension;
            return (newIndex < GameInformation.FieldLetters.Length && !occupationField[newIndex])
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private bool CanNavigateLeft()
        {
            int newIndex = currentPoint.Index - 1;
            return (currentPoint.Index % GameInformation.HorizontalDimension != 0 && !occupationField[newIndex])
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private bool CanNavigateRight()
        {
            int newIndex = currentPoint.Index + 1;
            return (newIndex % GameInformation.HorizontalDimension != 0 && !occupationField[newIndex])
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private bool CanNavigateUp()
        {
            int newIndex = currentPoint.Index - GameInformation.HorizontalDimension;
            return (newIndex >= 0 && !occupationField[newIndex])
                && wordStateMachine.CanNavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        public GameState GetNewGameState()
        {
            return new GameState(GameInformation, GameState.Words.Add(GetCurrentWord()));
        }

        public bool IsWordExist()
        {
            return wordStateMachine.IsWordExist();
        }

        public void Navigate(NavigateDirection navigateDirection)
        {
            switch (navigateDirection)
            {
                case NavigateDirection.Left:
                    NavigateLeft();
                    break;
                case NavigateDirection.Up:
                    NavigateUp();
                    break;
                case NavigateDirection.Right:
                    NavigateRight();
                    break;
                case NavigateDirection.Down:
                    NavigateDown();
                    break;
                default:
                    throw new NotSupportedException($"{navigateDirection} is not suppoted.");
            }
        }

        public void NavigateBack()
        {
            var prevPoint = gamePoints.Pop();
            currentPoint = gamePoints.Peek();
            occupationField[prevPoint.Index] = false;
            wordStateMachine.NavigateBack();
        }

        private void NavigateDown()
        {
            int newIndex = currentPoint.Index + GameInformation.HorizontalDimension;
            occupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private void NavigateLeft()
        {
            int newIndex = currentPoint.Index - 1;
            occupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private void NavigateRight()
        {
            int newIndex = currentPoint.Index + 1;
            occupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private void NavigateUp()
        {
            int newIndex = currentPoint.Index - GameInformation.HorizontalDimension;
            occupationField[newIndex] = true;
            var newPoint = new GamePoint(GameInformation, newIndex);
            gamePoints.Push(newPoint);
            currentPoint = newPoint;
            wordStateMachine.NavigateForward(GameInformation.FieldLetters[newIndex]);
        }

        private GameWord GetCurrentWord()
        {
            return new GameWord(this.GameInformation, gamePoints.Reverse());
        }
    }
}
