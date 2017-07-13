using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace FillWordGameSolver
{
    public class GameWord : IComparable
    {
        public ImmutableList<GamePoint> GamePoints { get; private set; }

        public GameInformation GameInformation { get; private set; }

        public int Length
        {
            get { return GamePoints.Count; }
        }

        public GameWord(GameInformation gameInformation, IEnumerable<GamePoint> gamePoints)
        {
            this.GamePoints = gamePoints.ToImmutableList();
            this.GameInformation = gameInformation;
        }

        public override string ToString()
        {
            return new String(GamePoints.Select(t => t.ToChar()).ToArray());
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (var point in GamePoints)
            {
                hash = hash * 23 + point.Index;
            }
            return hash;
        }

        public override bool Equals(object obj)
        {
            var another = obj as GameWord;
            if (another == null || this.GamePoints.Count != another.GamePoints.Count)
            {
                return false;
            }

            bool result = true;
            for (int i = 0; i < this.GamePoints.Count; i++)
            {
                result &= this.GamePoints[i].Index == another.GamePoints[i].Index;
            }
            return result;
        }

        public int CompareTo(object obj)
        {
            var another = obj as GameWord;
            if (another == null)
            {
                return -1;
            }
            var lengthCompareResult = this.GamePoints.Count.CompareTo(another.GamePoints.Count);
            if (lengthCompareResult == 0)
            {
                for (int i = 0; i < this.GamePoints.Count; i++)
                {
                    int compareResult = this.GamePoints[i].Index.CompareTo(another.GamePoints[i].Index);
                    if (compareResult != 0)
                    {
                        return compareResult;
                    }
                }
                return 0;
            }
            else
            {
                return lengthCompareResult;
            }
        }
    }
}
