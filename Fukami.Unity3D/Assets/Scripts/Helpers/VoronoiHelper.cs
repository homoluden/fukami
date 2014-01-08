using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenTools.Mathematics;

namespace Assets.Scripts.Helpers
{

    public class Vector2Comparer : IEqualityComparer<Vector2>
    {
        public bool Equals(Vector2 x, Vector2 y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(Vector2 obj)
        {
            return obj.GetHashCode();
        }
    }


    public struct VorCellInfo
    {
        public Vector2 Site;
        public IEnumerable<VoronoiEdge> Edges;
        public bool IsClosed;
    }

    public struct VorEdgeInfo
    {
        public Vector2 Start;
        public Vector2 End;
    }

    public sealed class VoronoiHelper
    {
        #region Properties

        private uint _xBoundVerts = 50;
        public uint XBoundVertsCount
        {
            get { return _xBoundVerts; }
            set { _xBoundVerts = value; }
        }

        private uint _yBoundVerts = 50;
        public uint YBoundVertsCount
        {
            get { return _yBoundVerts; }
            set { _yBoundVerts = value; }
        }


        #endregion // Properties


        #region Public Methods
        public VorCellInfo[] GenerateRectVorMap(Vector2 size, int visibleCellsCount)
        {
            var mapWidth = size.x;
            var mapHeight = size.y;

            var dx = mapWidth / XBoundVertsCount;
            var dy = mapHeight / YBoundVertsCount;
            var xMax = mapWidth * 0.5f;
            var yMax = mapHeight * 0.5f;

            var visibleVerts = new List<Vector2>(visibleCellsCount);
            var boundingVerts = new List<Vector2>();

            for (int i = 0; i < XBoundVertsCount / 2; i++)
            {
                var x = i * dx;
                boundingVerts.AddRange(new[] { new Vector2(x, yMax), new Vector2(-x, yMax), new Vector2(x, -yMax), new Vector2(-x, -yMax) });
            }

            for (int i = 0; i < YBoundVertsCount / 2; i++)
            {
                var y = i * dy;
                boundingVerts.AddRange(new[] { new Vector2(xMax, y), new Vector2(xMax, -y), new Vector2(-xMax, y), new Vector2(-xMax, -y) });
            }

            boundingVerts = boundingVerts.Distinct(new Vector2Comparer()).ToList();

            for (int i = 0; i < visibleCellsCount; i++)
            {
                visibleVerts.Add(new Vector2(Random.Range(-xMax + dx, xMax - dx), Random.Range(-yMax + dy, yMax - dy)));
            }

            var graph = Fortune.ComputeVoronoiGraph(boundingVerts.Concat(visibleVerts).Select(v => new BenTools.Mathematics.Vector(v.x, v.y)));


            var cells = graph.Cells.Select(c =>
                                            new VorCellInfo
                                            {
                                                Site = new Vector2((float)c.Key[0], (float)c.Key[1]),
                                                Edges = c.Value,
                                                IsClosed = !c.Value.Any(ed => ed.IsInfinite || ed.IsPartlyInfinite)
                                            }).ToList();
            cells.RemoveAll(c => boundingVerts.Contains(c.Site));

            return cells.Where(c => c.IsClosed).ToArray();
        }

        #endregion // Public Methods

        #region Singleton implementation

        private static volatile VoronoiHelper _instance;
        private static readonly object SyncRoot = new System.Object();

        private VoronoiHelper()
        {
            
        }

        public static VoronoiHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new VoronoiHelper();
                    }
                }

                return _instance;
            }
        }
        #endregion

    }
}
