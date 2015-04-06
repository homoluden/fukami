using FarseerPhysics.Common;

namespace Shapes.Abstract
{
	public class Polygon
    {
        public Vertices Vertices { get; private set; }

        public Polygon(Vertices vertices)
        {
            Vertices = vertices;
        }
    }
}
