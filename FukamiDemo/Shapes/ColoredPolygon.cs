using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using Shapes.Abstract;

namespace Shapes
{
    public class ColoredPolygon : Polygon
    {
        public Vector4 Fill { get; private set; }
        public Vector4 Border { get; private set; }

        public ColoredPolygon(Vertices vertices, Vector4 fill, Vector4 border)
            :base(vertices)
        {
            Fill = fill;
            Border = border;
        }
    }
}
