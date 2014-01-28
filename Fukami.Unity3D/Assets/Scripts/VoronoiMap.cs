using UnityEngine;
using Assets.Scripts.Helpers;
using System.Collections.Generic;
using System.Linq;

using FortuneVoronoi;

public class VoronoiMap : MonoBehaviour
{
    #region Fields

    float _timeLeft;
    List<GameObject> _vorCells = new List<GameObject>();

    #endregion

    #region Properties

    public float RegeneratePeriod = 3f;
    public Material[] MeshMaterials;

    public Vector2 MapSize = new Vector2(700f, 300f);

    public int CellsCount = 500;

    #endregion

    // Use this for initialization
    void Start()
    {
        if (CellsCount < 1)
        {
            CellsCount = 500;
        }

        _timeLeft = RegeneratePeriod;
        GenerateVorMap();
    }

    private void GenerateVorMap()
    {
        foreach (var cell in _vorCells)
        {
            Destroy(cell, Time.fixedDeltaTime);
        }
        _vorCells.Clear();

        var cells = VoronoiHelper.Instance.GenerateRectVorMap(MapSize, CellsCount);

        int j = 0;
        foreach (var cell in cells)
        {
            AddSiteObject(cell, j++);
        }
    }

	private void AddSiteObject(VoronoiCell cellData, int cellIndex)
    {
        var cellObject = new GameObject(string.Format("VorCell_{0}", cellIndex)) { tag = gameObject.tag, layer = gameObject.layer};

        cellObject.transform.parent = gameObject.transform;
		cellObject.transform.localPosition = new Vector3((float)cellData.Site.X, (float)cellData.Site.Y);

        var vorCell = cellObject.AddComponent<VorCell>();

        vorCell.CellData = cellData;

        if (MeshMaterials.Length > 0)
        {
            vorCell.MeshMaterial = MeshMaterials[Random.Range(0, MeshMaterials.Length )];
        }

        _vorCells.Add(cellObject);

    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;
        if (_timeLeft < 0f)
        {
            _timeLeft = RegeneratePeriod;
            GenerateVorMap();
        }
    }
}
