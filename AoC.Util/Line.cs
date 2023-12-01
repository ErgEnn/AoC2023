using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public class Line
    {
        public Vertex Vertex0 { get; init; }
        public Vertex Vertex1 { get; init; }

        public static Line FromPoints(Vertex vertex0, Vertex vertex1)
        {
            return new Line()
            {
                Vertex0 = vertex0,
                Vertex1 = vertex1,
            };
        }

        public static Line FromTuples((int, int) vertex0, (int, int) vertex1)
        {
            return FromPoints(Vertex.FromTuple(vertex0), Vertex.FromTuple(vertex1));
        }

        public Vertex? Intersect(Line line)
        {
            var (x1, y1, x2, y2) = this;
            var (x3, y3, x4, y4) = line;
            var div = (((x1 - x2) * (y3 - y4)) - ((y1 - y2) * (x3 - x4)));
            if (div == 0)
                return null;
            var t = (((x1 - x3)*(y3 - y4)) - ((y1 - y3)*(x3 - x4))) / div;
            var u = (((x1 - x3) * (y1 - y2)) - ((y1 - y3) * (x1 - x2))) /div;
            if (t.IsBetween(0, 1) && u.IsBetween(0, 1))
            {
                var x = x1 + t * (x2 - x1);
                var y = y1 + t * (y2 - y1);
                return Vertex.FromTuple((x,y));
            }

            return null;
        }

        public void Deconstruct(out int x1, out int y1, out int x2, out int y2)
        {
            x1 = Vertex0.X;
            y1 = Vertex0.Y;
            x2 = Vertex1.X;
            y2 = Vertex1.Y;
        }
    }
}
