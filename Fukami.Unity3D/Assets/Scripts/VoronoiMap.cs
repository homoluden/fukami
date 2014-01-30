using UnityEngine;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;

using FortuneVoronoi;

public class VoronoiMap : MonoBehaviour
{
    #region Fields

    List<GameObject> _vorTiles = new List<GameObject>();

    #endregion

    #region Properties

    public Material[] MeshMaterials;

    public Vector2 TileSize = new Vector2(300f, 100f);
	public int[] TileGridDimensions = new []{
		150, // Major X Ticks count
		50,  // Major Y Ticks count
		16   // Minor Ticks resolution (count of ticks between Major ticks)
	};
	public int MapWidth = 10;
	public int MapHeight = 1;
	public int MapSeed = 120683; // Just a magic number

    public int MaxCellsCount = 500;

    #endregion

    // Use this for initialization
    void Start()
    {
        if (MaxCellsCount < 2)
        {
            MaxCellsCount = 500;
        }
		MapWidth = Mathf.Abs (MapWidth); // Make sure it is positive
		MapHeight = Mathf.Abs (MapHeight); // Make sure it is positive

        GenerateVorMap();
    }

    private void GenerateVorMap()
    {
        foreach (var cell in _vorTiles)
        {
            Destroy(cell, Time.fixedDeltaTime);
        }
        _vorTiles.Clear();

		VoronoiHelper.Instance.TilesTag = gameObject.tag;
		VoronoiHelper.Instance.TilesLayer = gameObject.layer;
		VoronoiHelper.Instance.TilesSize = TileSize;
		VoronoiHelper.Instance.InternalSitesCountRange [0] = MaxCellsCount / 2;
		VoronoiHelper.Instance.InternalSitesCountRange [1] = MaxCellsCount;

		Random.seed = MapSeed;

		var dx = TileSize.x / TileGridDimensions [0] / TileGridDimensions [2];
		var dy = TileSize.y / TileGridDimensions [1] / TileGridDimensions [2];

		for (int i = 0; i < MapWidth; i++) {
			var xOffset = TileSize.x*i + dx;

			for (int j = 0; j < MapHeight; j++) {
				var yOffset = TileSize.y * j + dy;
				var baseIndex = MapWidth*i + j;

				var newTile0 = VoronoiHelper.Instance.CreateTileObject(baseIndex, Random.Range(int.MinValue, int.MaxValue),
				                                                       gameObject.transform, new Vector2(xOffset, yOffset), MeshMaterials);
				var newTile1 = VoronoiHelper.Instance.CreateTileObject(baseIndex + 1, Random.Range(int.MinValue, int.MaxValue),
				                                                      gameObject.transform, new Vector2(xOffset, yOffset), MeshMaterials);
				var newTile2 = VoronoiHelper.Instance.CreateTileObject(baseIndex + 2, Random.Range(int.MinValue, int.MaxValue),
				                                                      gameObject.transform, new Vector2(xOffset, yOffset), MeshMaterials);
				var newTile3 = VoronoiHelper.Instance.CreateTileObject(baseIndex + 3, Random.Range(int.MinValue, int.MaxValue),
				                                                      gameObject.transform, new Vector2(xOffset, yOffset), MeshMaterials);

				_vorTiles.AddRange(new []{newTile0,newTile1,newTile2,newTile3});
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
