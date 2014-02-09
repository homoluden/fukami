using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

using FortuneVoronoi;

public class VorTile : MonoBehaviour
{

	private FortuneVoronoi.VoronoiCell[] _cells;
	private List<GameObject> _cellObjects = new List<GameObject> ();

	public int Seed;
	public Material[] MeshMaterials;
    public GameObject LeftTileTrigger;
    public GameObject RightTileTrigger;

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
            var leftCollider = LeftTileTrigger.GetComponent<BoxCollider2D>();
            LeftTileTrigger.transform.localPosition = new Vector2(tileSize.x * 0.5f, 0f);
            leftCollider.size = new Vector2(tileSize.x * 0.25f, tileSize.y * 1.5f);
            leftCollider.isTrigger = true;
        }

        if (RightTileTrigger != null)
        {
            var rightCollider = RightTileTrigger.GetComponent<BoxCollider2D>();
            RightTileTrigger.transform.localPosition = new Vector2(-tileSize.x * 0.5f, 0f);
            rightCollider.size = new Vector2(tileSize.x * 0.25f, tileSize.y * 1.5f);
            rightCollider.isTrigger = true;
        }
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
