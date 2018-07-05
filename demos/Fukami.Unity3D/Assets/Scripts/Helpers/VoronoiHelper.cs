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

		public string TilesTag;
		public int TilesLayer;
		public Vector2 TilesSize;

		public readonly int[] InternalSitesCountRange = new []{200, 500};

		public int HorBorderSitesCnt = 50;
		public int VertBorderSitesCnt = 50;
		public int Resolution = 16;
		public Dictionary<Vector2, BenTools.Data.HashSet<SitesGridGenerator.IntSite>> CachedBorders = new Dictionary<Vector2, BenTools.Data.HashSet<SitesGridGenerator.IntSite>> ();

        #endregion // Properties


        #region Public Methods
				
		public GameObject CreateTileObject(IntPoint tilePosition, int tileSeed, Transform parentTransform, Vector2 offset, Material[] materials)
		{
			var tilePrefab = PrefabsManager.Instance.LoadPrefab("VorMap/VorTile");
            var tile = (GameObject)UnityEngine.Object.Instantiate(tilePrefab);

			tile.transform.parent = parentTransform;
			tile.transform.localPosition = new Vector3(offset.x, offset.y);
			
            tile.tag = TilesTag;
			tile.layer = TilesLayer;
			
			var tileScript = tile.GetComponent<VorTile>();
			
			tileScript.Seed = tileSeed;
			tileScript.MeshMaterials = materials;
            tileScript.Position = tilePosition;

			return tile;
		}
		
		public GameObject CreateCellObject (VoronoiCell cellData, int cellIndex, Transform parentTransform, Material meshMaterial)
		{
			var cellObject = new GameObject (string.Format ("VorCell.{0}", cellIndex)) { tag = TilesTag, layer = TilesLayer};
			
			cellObject.transform.parent = parentTransform;
			cellObject.transform.localPosition = new Vector3 ((float)cellData.Site.X, (float)cellData.Site.Y);
			
			var vorCell = cellObject.AddComponent<VorCell> ();
			
			vorCell.CellData = cellData;
			vorCell.MeshMaterial = meshMaterial;
			
			return cellObject;
		}

		public VoronoiCell[] GenerateTileCells(int internalCellsCount)
        {
			var mapWidth = TilesSize.x;
			var mapHeight = TilesSize.y;

			var cells = new Dictionary<FortuneVoronoi.Common.Point, VoronoiCell>();

			var dx = mapWidth / HorBorderSitesCnt / Resolution;
			var dy = mapHeight / VertBorderSitesCnt / Resolution;

			
			var internalSites = SitesGridGenerator.GenerateInternalSites(HorBorderSitesCnt, VertBorderSitesCnt, Resolution, internalCellsCount,
			                                                             (min, max) => new IntPoint(Random.Range(min,max), Random.Range(min,max)));

			if (!CachedBorders.ContainsKey(TilesSize)) {
				var borderSites = SitesGridGenerator.GenerateTileBorder(HorBorderSitesCnt, VertBorderSitesCnt, Resolution,
                                                                    (min, max) => new IntPoint(Random.Range(min, max), Random.Range(min, max))); // (int)((min + max) * 0.5f)
				CachedBorders.Add(TilesSize, borderSites);
			}

			foreach (var site in internalSites.Concat(CachedBorders[TilesSize]).Distinct()) {
				var x = site.X * dx;
				var y = site.Y * dy;
				var v = new FortuneVoronoi.Common.Point(x,y);

				cells.Add(v, new VoronoiCell{IsVisible = !site.IsBorder, Site = v});
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
