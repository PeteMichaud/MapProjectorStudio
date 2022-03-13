using System.Collections.Generic;

namespace MapProjectorLib.Extensions
{
    public static class ListExtensions
    {
        public static void RemoveAdjacentDuplicates<T>(this List<T> List, IComparer<T> Comparer)
        {
            int NumUnique = 0;
            for (int i = 0; i < List.Count; i++)
                if ((i == 0) || (Comparer.Compare(List[NumUnique - 1], List[i]) != 0))
                    List[NumUnique++] = List[i];
            List.RemoveRange(NumUnique, List.Count - NumUnique);
        }
    }
}
