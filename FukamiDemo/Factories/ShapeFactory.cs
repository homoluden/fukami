
#if UseDouble
using Scalar = System.Double;
#else
using Scalar = System.Single;
#endif


using AdvanceMath;
using System.Linq;
using System.Collections.Concurrent;
using Shapes.Abstract;
using FarseerPhysics.Collision.Shapes;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;

namespace Factories
{
	public static class ShapeFactory
    {
        #region Consts

        public static readonly Scalar VERTICES_TOLERANCE = 0.1f;

        #endregion

        #region Fields

        internal static ConcurrentDictionary<ColoredPolygon, Shape> _colShapes = new ConcurrentDictionary<ColoredPolygon, Shape>();

        #endregion

        /// <summary>
        /// Creates the array of colors.
        /// </summary>
        /// <param name="first">The color of first item in array</param>
        /// <param name="rest">Color of all array items except first one</param>
        /// <param name="count">The length of array</param>
        /// <returns>The array of colors created.</returns>
        public static Vector3[] CreateColor3Array(Vector3 first, Vector3 rest, int count)
        {
            var result = new Vector3[count];
            result[0] = first;
            for (int index = 1; index < count; ++index)
            {
                result[index] = rest;
            }
            return result;
        }


        public static Vector3[] CreateColor3Array(int count)
        {
            return CreateColor3Array(new Vector3(1, .5f, 0), new Vector3(1, 1, 1), count);
        }

        public static PolygonShape CreatePolygonShape(Polygon polygon, Scalar gridSpacing)
        {
            return new PolygonShape(polygon.Vertices, gridSpacing);
        }

        public static Shape GetOrCreateColoredPolygonShape(Vertices vertices, Scalar gridSpacing)
        {
            var reduced = new Path(vertices);
            
            var polygon = new ColoredPolygon(reduced, CreateColor3Array(reduced.Length));

            var shape = _colShapes.GetOrAdd(polygon, p => CreatePolygonShape(p, gridSpacing));

            shape.Tag = polygon;

            return shape;
        }

        public static CircleShape CreateColoredCircle(Scalar radius, ushort vertexCount)
        {
            CircleShape shape = new CircleShape(radius, vertexCount);
            var reduced = VertexHelper.Reduce(shape.Vertexes);
            var polygon = new ColoredPolygon(reduced, CreateColor3Array(reduced.Length));

            shape.Tag = DrawableFactory.GetOrCreateColoredPolygonDrawable(polygon, Drawables.ShapeType.Mesh);
            return shape;
        }

    }
}
