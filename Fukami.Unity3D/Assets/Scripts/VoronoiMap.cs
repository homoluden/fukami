using UnityEngine;
using Assets.Scripts.Helpers;
using BenTools.Mathematics;
using System.Collections.Generic;
using System.Linq;

public class VoronoiMap : MonoBehaviour
{
    #region Fields

    VoronoiGraph _graph;
    float _timeLeft;
    List<GameObject> _vorCells = new List<GameObject>();

    #endregion

    #region Properties

    public float RegeneratePeriod = 3f;
    public Material[] MeshMaterials;

    public Vector2 MapSize = new Vector2(700f, 300f);

    public int CellsCount = 500;

    // The count of vertices on horizontal and vertical borders. Border vertices will not be represented in Voronoi Map
    public int XBoundVertsCount = 60;
    public int YBoundVertsCount = 60;

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

    private void AddSiteObject(VorCellInfo cellData, int cellIndex)
    {
        var cellObject = new GameObject { name = string.Format("VorCell_{0}", cellIndex) };

        cellObject.transform.position = new Vector3(cellData.Site.x, cellData.Site.y);
        cellObject.transform.parent = gameObject.transform;

        var vorCell = cellObject.AddComponent<VorCell>();

        vorCell.CellData = cellData;

        if (MeshMaterials.Length > 0)
        {
            vorCell.MeshMaterial = MeshMaterials[Random.Range(0, MeshMaterials.Length - 1)];
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
