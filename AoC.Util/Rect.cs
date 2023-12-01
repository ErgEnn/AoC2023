using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Util
{
    public class Rect
    {
        public Vertex Vertex0 { get; init; }
        public Vertex Vertex1 { get; init; }

        public Line Top { get; init; }
        public Line Right { get; init; }
        public Line Bottom { get; init; }
        public Line Left { get; init; }

        public static Rect FromCorners(Vertex v0, Vertex v1)
        {
            var v10 = Vertex.FromTuple((v1.X, v0.Y));
            var v01 = Vertex.FromTuple((v0.X, v1.Y));
            return new Rect()
            {
                Vertex0 = v0,
                Vertex1 = v1,
                Top = Line.FromPoints(v0,v10),
                Right = Line.FromPoints(v10,v1),
                Bottom = Line.FromPoints(v1,v01),
                Left = Line.FromPoints(v01,v0)
            };
        }

        public IEnumerable<Line> Lines()
        {
            yield return Top;
            yield return Right;
            yield return Bottom;
            yield return Left;
        }

        public bool Contains(Vertex v)
        {
            return v.X.IsBetween(Vertex0.X, Vertex1.X) && v.Y.IsBetween(Vertex0.Y, Vertex1.Y);
        }

        public Line? Intersect(Line line)
        {
            Vertex? v0 = null;
            Vertex? v1 = null;
            foreach (var rectBorder in Lines())
            {
                if(rectBorder.Intersect(line) is Vertex v)
                    if (v0 == null)
                        v0 = v;
                    else
                    {
                        v1 = v;
                        break;
                    }
            }

            if (v0 == null)
                return null;

            if (v1 == null)
            {
                if(Contains(line.Vertex0))
                    return Line.FromPoints(v0,line.Vertex0);
                return Line.FromPoints(v0,line.Vertex1);
            }
            return Line.FromPoints(v0,v1);
        }
    }
}
