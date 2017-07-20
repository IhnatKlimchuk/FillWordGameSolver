using System;
using System.Threading;
using System.IO;

namespace FillWordGameSolver
{
    public class ConcurrentWordStateMachine : IWordStateMachineFactory
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
                return AddWordInternal(word);
            }
            finally
            {
                StateMachineLock.ExitWriteLock();
            }
        }

        private bool AddWordInternal(string word)
        {
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

        public void AddWords(string filePath)
        {
            try
            {
                StateMachineLock.EnterWriteLock();

                string line;
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader streamReader = new StreamReader(fileStream))
                    {
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            AddWordInternal(line);
                        }
                    }
                }
            }
            finally
            {
                StateMachineLock.ExitWriteLock();
            }
        }

        public IWordStateMachine GetWordStateMachine()
        {
            return new ConcurrentWordStateMachineClient(this);
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
