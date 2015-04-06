using FarseerPhysics.Common;

namespace Drawables
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
