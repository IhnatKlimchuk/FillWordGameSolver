using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FillWordGameSolver
{
    public class ConcurrentWordStateMachine
    {
        public ConcurrentWordStateMachine()
        {
            InitialState = new ConcurrentWordStateMachineState(null);
            StateMachineLock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
        }

        internal ConcurrentWordStateMachineState InitialState { get; private set; }

        internal ReaderWriterLockSlim StateMachineLock { get; private set; }

        public bool AddWord(string word)
        {
            try
            {
                StateMachineLock.EnterWriteLock();
                ConcurrentWordStateMachineState currentState = InitialState;
                foreach (var letter in FormatWord(word))
                {
                    if (currentState.LetterDictionary.ContainsKey(letter))
                    {
                        currentState = currentState.LetterDictionary[letter];
                    }
                    else
                    {
                        ConcurrentWordStateMachineState newState = new ConcurrentWordStateMachineState(currentState, letter);
                        currentState.LetterDictionary[letter] = newState;
                        currentState = newState;
                    }
                }
                if (currentState.IsWordEnd)
                {
                    return false;
                }
                else
                {
                    currentState.IsWordEnd = true;
                    return true;
                }
            }
            finally
            {
                StateMachineLock.ExitWriteLock();
            }
        }

        public bool RemoveWord(string word)
        {
            try
            {
                StateMachineLock.EnterWriteLock();
                ConcurrentWordStateMachineState currentState = InitialState;
                foreach (var letter in FormatWord(word))
                {
                    if (currentState.LetterDictionary.ContainsKey(letter))
                    {
                        currentState = currentState.LetterDictionary[letter];
                    }
                    else
                    {
                        return false;
                    }
                }
                if (currentState.LetterDictionary.Count == 0)
                {
                    while (currentState.PreviousState.LetterDictionary.Count == 1)
                    {
                        currentState = currentState.PreviousState;
                    }
                    currentState.PreviousState.LetterDictionary.Remove(currentState.Letter);
                }
                else
                {
                    currentState.IsWordEnd = false;
                }
                return true;
            }
            finally
            {
                StateMachineLock.ExitWriteLock();
            }
        }
        
        private string FormatWord(string word)
        {
            return word.ToLowerInvariant();
        }
    }
}
