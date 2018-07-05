using UnityEngine;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;

using FortuneVoronoi;
using FortuneVoronoi.Common;

public class VoronoiMap : MonoBehaviour
{
    #region Fields

    GameObject _seedTile;

    #endregion

    #region Properties

    public Material[] MeshMaterials;

    public Vector2 TileSize;
	public int[] TileGridDimensions;
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

		TileGridDimensions = new []{
			150, // Major X Ticks count
			50,  // Major Y Ticks count
			16   // Minor Ticks resolution (count of ticks between Major ticks)
		};

        GenerateVorMap();
    }

    private void GenerateVorMap()
    {
        VoronoiHelper.Instance.TilesTag = gameObject.tag;
		VoronoiHelper.Instance.TilesLayer = gameObject.layer;
		VoronoiHelper.Instance.TilesSize = TileSize;
		VoronoiHelper.Instance.InternalSitesCountRange [0] = MaxCellsCount / 2;
		VoronoiHelper.Instance.InternalSitesCountRange [1] = MaxCellsCount;

		Random.seed = MapSeed;

        _seedTile = VoronoiHelper.Instance.CreateTileObject(new IntPoint(0, 0), 
                                                            Random.Range(int.MinValue, int.MaxValue),
                                                            gameObject.transform, 
                                                            new Vector2(0,0), 
                                                            MeshMaterials);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnLeftTileTriggerEnter(VorTile tileScript)
    {
        var tilePos = tileScript.Position;

        int i = tilePos.X;
        int j = tilePos.Y;

        var dx = 3 * i * TileSize.x / TileGridDimensions[0];
        var dy = 3 * j * TileSize.y / TileGridDimensions[1];

        var xOffset = TileSize.x * i;
        var yOffset = TileSize.y * j;

        var newTile = VoronoiHelper.Instance.CreateTileObject(new IntPoint(0, 0),
                                                            Random.Range(int.MinValue, int.MaxValue),
                                                            gameObject.transform,
                                                            new Vector2(xOffset - dx, yOffset - dy),
                                                            MeshMaterials);
    }

    void OnRightTileTriggerEnter(VorTile tileScript)
    {

    }
}
