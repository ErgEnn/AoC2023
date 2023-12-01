using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public class Vertex
    {
        public int X { get; init; }
        public int Y { get; init; }

        public static Vertex FromTuple((int, int) tuple)
        {
            return new Vertex()
            {
                X = tuple.Item1,
                Y = tuple.Item2,
            };
        }
    }
}
