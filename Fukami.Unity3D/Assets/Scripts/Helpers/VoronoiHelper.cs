using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using FortuneVoronoi;
using FortuneVoronoi.Common;
using FortuneVoronoi.Tools;

namespace Assets.Scripts.Helpers
{

    public sealed class VoronoiHelper
    {
        #region Properties
		
		public int HorBorderSitesCnt = 50;
		public int VertBorderSitesCnt = 50;
		public int Resolution = 16;

        #endregion // Properties


        #region Public Methods
		public VoronoiCell[] GenerateRectVorMap(Vector2 size, int internalCellsCount)
        {
            var mapWidth = size.x;
            var mapHeight = size.y;

			var cells = new Dictionary<FortuneVoronoi.Common.Point, VoronoiCell>();

			var dx = mapWidth / HorBorderSitesCnt / Resolution;
			var dy = mapHeight / VertBorderSitesCnt / Resolution;
			
			var borderSites = SitesGridGenerator.GenerateTileBorder(HorBorderSitesCnt, VertBorderSitesCnt, Resolution,
			                                                        (min, max) => new IntPoint(Random.Range(min,max), Random.Range(min,max)));
			
			var internalSites = SitesGridGenerator.GenerateInternalSites(HorBorderSitesCnt, VertBorderSitesCnt, Resolution, internalCellsCount,
			                                                             (min, max) => new IntPoint(Random.Range(min,max), Random.Range(min,max)));
			
			foreach (var site in borderSites.Concat(internalSites))
			{
				var x = site.X*dx;
				var y = site.Y*dy;
				var v = new FortuneVoronoi.Common.Point(x, y);
				
				if (cells.ContainsKey(v))
				{
					continue;
				}
				
				cells.Add(v, new VoronoiCell{ IsVisible = !site.IsBorder, Site = new FortuneVoronoi.Common.Point(x, y)});
			}
			
			var graph = Fortune.ComputeVoronoiGraph(cells);

			return cells.Values.Where(c => c.IsVisible && c.IsClosed).ToArray();
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
