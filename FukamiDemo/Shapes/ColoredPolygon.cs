using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Shapes.Abstract
{
    public class ColoredPolygon : Polygon
    {
        public Vector4 Fill { get; private set; }
        public Vector4 Border { get; private set; }

        public ColoredPolygon(IList<Vector2> vertices, Vector4 fill, Vector4 border)
            :base(vertices)
        {
            Fill = fill;
            Border = border;
        }
    }
}
