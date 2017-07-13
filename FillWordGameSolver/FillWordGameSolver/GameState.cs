using FillWordGameSolver.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace FillWordGameSolver
{
    public class GameState
    {
        public GameInformation GameInformation { get; private set; }

        public ImmutableSortedSet<GameWord> Words { get; private set; }

        private BitArray OccupationField { get; set; }

        public GameState(GameInformation gameInformation) : this(gameInformation, null)
        {
        }

        public GameState(GameInformation gameInformation, IEnumerable<GameWord> words)
        {
            this.OccupationField = new BitArray(gameInformation.HorizontalDimension * gameInformation.VerticalDimension, false);
            this.GameInformation = gameInformation;
            if (words != null)
            {
                var listBuilder = ImmutableSortedSet.CreateBuilder<GameWord>();
                foreach (var word in words)
                {
                    listBuilder.Add(word);
                    foreach (var letter in word.GamePoints)
                    {
                        OccupationField[letter.Index] = true;
                    }
                }
                this.Words = listBuilder.ToImmutable();
            }
            else
            {
                this.Words = ImmutableSortedSet<GameWord>.Empty;
            }
        }
        
        public override int GetHashCode()
        {
            UInt32 hash = 17;
            int bitsRemaining = OccupationField.Length;
            foreach (int value in OccupationField.GetInternalValues())
            {
                UInt32 cleanValue = (UInt32)value;
                if (bitsRemaining < 32)
                {
                    //clear any bits that are beyond the end of the array
                    int bitsToWipe = 32 - bitsRemaining;
                    cleanValue <<= bitsToWipe;
                    cleanValue >>= bitsToWipe;
                }

                hash = hash * 23 + cleanValue;
                bitsRemaining -= 32;
            }
            return (int)hash;
        }

        public override bool Equals(object obj)
        {
            var another = obj as GameState;
            if (another == null || this.Words.Count != another.Words.Count)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < this.Words.Count; i++)
            {
                result &= this.Words[i].Equals(another.Words[i]);
            }
            return result;
        }
    }
}
