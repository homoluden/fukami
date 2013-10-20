using AdvanceMath;
using Physics2DDotNet.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shapes
{
    public class Polygon
    {
        public IList<Vector2D> Vertices { get; private set; }

        public Polygon(IList<Vector2D> vertices)
        {
            if (vertices == null) { throw new ArgumentNullException("vertices"); }

            Vertices = vertices;
        }
    }
}
