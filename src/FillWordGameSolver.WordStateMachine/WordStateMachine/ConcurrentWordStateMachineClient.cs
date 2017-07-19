using System;
using System.Collections.Generic;
using System.Text;

namespace FillWordGameSolver.WordStateMachine.WordStateMachine
{
    public class ConcurrentWordStateMachineClient
    {
        private ConcurrentWordStateMachine stateMachine;

        private ConcurrentWordStateMachineState currentState;

        public ConcurrentWordStateMachineClient(ConcurrentWordStateMachine stateMachine)
        {
            stateMachine.StateMachineLock.EnterReadLock();

            this.stateMachine = stateMachine;
            this.currentState = stateMachine.InitialState;
        }

        public void Reset()
        {
            currentState = stateMachine.InitialState;
        }

        public bool CanNavigate(char letter)
        {
            return currentState.LetterDictionary.ContainsKey(letter);
        }

        public void NavigateForward(char letter)
        {
            currentState = currentState.LetterDictionary[letter];
        }

        public bool CanNavigateBack()
        {
            return currentState.PreviousState != null;
        }

        public void NavigateBack()
        {
            currentState = currentState.PreviousState;
        }
    }
}
