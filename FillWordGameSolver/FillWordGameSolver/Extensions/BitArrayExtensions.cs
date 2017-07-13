using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace FillWordGameSolver.Extensions
{
    public static class BitArrayExtensions
    {
        static FieldInfo _internalArrayGetter = GetInternalArrayGetter();

        static FieldInfo GetInternalArrayGetter()
        {
            return typeof(BitArray).GetField("m_array", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        static int[] GetInternalArray(BitArray array)
        {
            return (int[])_internalArrayGetter.GetValue(array);
        }

        public static IEnumerable<int> GetInternalValues(this BitArray array)
        {
            return GetInternalArray(array);
        }
    }
}
