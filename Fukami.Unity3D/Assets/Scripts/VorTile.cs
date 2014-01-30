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
	public Vector2 TileSize;

	// Use this for initialization
	void Start ()
	{
		Random.seed = Seed;

		_cells = VoronoiHelper.Instance.GenerateTileCells (Random.Range (VoronoiHelper.Instance.InternalSitesCountRange [0],
                                                                		 VoronoiHelper.Instance.InternalSitesCountRange [1]));

		for (int i = 0; i < _cells.Length; i++) {
			var newCell = VoronoiHelper.Instance.CreateCellObject(_cells[i], i, gameObject.transform, MeshMaterials[Random.Range(0, MeshMaterials.Length)]);

			_cellObjects.Add(newCell);
		}
	}

	// Update is called once per frame
	void Update ()
	{

	}
}
