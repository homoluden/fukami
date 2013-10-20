using AdvanceMath;
using Shapes.Colors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes.Abstract
{
    public class ColoredPolygon : Polygon
    {
        public IList<ScalarColor3> Colors { get; private set; }

        public ColoredPolygon(IList<Vector2D> vertices, IList<ScalarColor3> colors)
            :base(vertices)
        {
            if (colors == null) { throw new ArgumentNullException("colors"); }
            if (colors.Count != vertices.Count) { throw new ArgumentException("Lengths of vertices and colors is not equal"); }

            Colors = colors;
        }
    }
}
