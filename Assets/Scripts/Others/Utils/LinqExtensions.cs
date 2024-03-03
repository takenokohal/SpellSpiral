using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Others.Utils
{
    public static class LinqExtensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> origin)
        {
            return origin.OrderBy(_ => Random.Range(0, int.MaxValue));
        }

        public static T GetRandomValue<T>(this IEnumerable<T> origin)
        {
            return origin.Shuffle().FirstOrDefault();
        }
    }
}