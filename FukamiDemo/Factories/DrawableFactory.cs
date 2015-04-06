using System;
using System.Collections.Concurrent;
using Drawables;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;

namespace Factories
{
    public struct CircleSpec
    {
        public float Radius;

        public ushort SegmentsCount;

        public Vector4 Fill;

        public Vector4 Border;
    }

    public struct PolygonSpec
    {
        public Vertices Vertices;

        public Vector4 Fill;

        public Vector4 Border;
    }

    public static class DrawableFactory
    {
        #region Fields

        private static readonly ConcurrentDictionary<CircleSpec, ColoredPolygon> Circles = new ConcurrentDictionary<CircleSpec, ColoredPolygon>();

        private static readonly ConcurrentDictionary<PolygonSpec, ColoredPolygon> Polygons = new ConcurrentDictionary<PolygonSpec, ColoredPolygon>();

        #endregion

        public static ColoredPolygon GetOrCreateCircleDrawable(CircleSpec spec)
        {
            return Circles.GetOrAdd(spec, CreateCirclePolygon);
        }

        private static ColoredPolygon CreateCirclePolygon(CircleSpec spec)
        {
            var increment = (float)(Math.PI * 2.0 / spec.SegmentsCount);
            var theta = 0.0f;
            var verts = new Vertices(spec.SegmentsCount);

            for (var i = 0; i < spec.SegmentsCount; i++)
            {
                var vertex = spec.Radius * new Vector2((float)Math.Cos(theta), (float)Math.Sin(theta));

                verts.Add(vertex);

                theta += increment;
            }

            return new ColoredPolygon(verts, spec.Fill, spec.Border);
        }

        public static ColoredPolygon GetOrCreateColoredPolygonDrawable(PolygonSpec spec)
        {
            return Polygons.GetOrAdd(spec, CreateGenericPolygon);
        }

        private static ColoredPolygon CreateGenericPolygon(PolygonSpec spec)
        {
            return new ColoredPolygon(spec.Vertices, spec.Fill, spec.Border);
        }

        static DrawableFactory ()
        {
        }
    }
}
