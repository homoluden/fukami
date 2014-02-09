using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

using FortuneVoronoi;

public class VorTile : MonoBehaviour
{
    #region Fields

    private FortuneVoronoi.VoronoiCell[] _cells;
    private List<GameObject> _cellObjects = new List<GameObject>();

    private BoxCollider2D _leftCollider;
    private BoxCollider2D _rightCollider;

    #endregion // Fields


    #region Properties

    public int Seed;
    public Material[] MeshMaterials;

    public GameObject LeftTileTrigger;

    public GameObject RightTileTrigger;

    public FortuneVoronoi.Common.IntPoint Position;

    #endregion // Properties

    // Use this for initialization
	void Start ()
	{

		Random.seed = Seed;

		_cells = VoronoiHelper.Instance.GenerateTileCells (Random.Range (VoronoiHelper.Instance.InternalSitesCountRange [0],
                                                                		 VoronoiHelper.Instance.InternalSitesCountRange [1]));

		for (int i = 0; i < _cells.Length; i++) {
			var newCell = VoronoiHelper.Instance.CreateCellObject(_cells[i], 
			                                                      i, 
			                                                      gameObject.transform, 
			                                                      MeshMaterials[Random.Range(0, MeshMaterials.Length)]);

			_cellObjects.Add(newCell);
		}

        var tileSize = VoronoiHelper.Instance.TilesSize;

        if (LeftTileTrigger != null)
        {
            _leftCollider = LeftTileTrigger.GetComponent<BoxCollider2D>();
            LeftTileTrigger.transform.localPosition = new Vector2(tileSize.x * 0.5f, 0f);
            _leftCollider.size = new Vector2(tileSize.x * 0.25f, tileSize.y * 1.5f);
            _leftCollider.isTrigger = true;            
        }

        if (RightTileTrigger != null)
        {
            _rightCollider = RightTileTrigger.GetComponent<BoxCollider2D>();
            RightTileTrigger.transform.localPosition = new Vector2(-tileSize.x * 0.5f, 0f);
            _rightCollider.size = new Vector2(tileSize.x * 0.25f, tileSize.y * 1.5f);
            _rightCollider.isTrigger = true;            
        }
	}

	// Update is called once per frame
	void Update ()
	{

	}

}
