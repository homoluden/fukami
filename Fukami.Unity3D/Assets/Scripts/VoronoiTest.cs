using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenTools.Mathematics;

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


public struct VorSiteInfo
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

public class VoronoiTest : MonoBehaviour
{
    #region Fields

    VoronoiGraph _graph;
    float _timeLeft;
    List<GameObject> _vorCells = new List<GameObject>();

    #endregion

    #region Properties

    public float RegeneratePeriod = 3f;
    public Material[] MeshMaterials;

    public float MapWidth = 700f;
    public float MapHeight = 300f;

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

        var verts = new List<Vector2>();
        var boundingVerts = new List<Vector2>();

        var dx = MapWidth / XBoundVertsCount;
        var dy = MapHeight / YBoundVertsCount;
        var xMax = MapWidth * 0.5f;
        var yMax = MapHeight * 0.5f;

        for (int i = 0; i < XBoundVertsCount / 2; i++)
        {
            var x = i * dx;
            boundingVerts.AddRange(new []{ new Vector2(x, yMax), new Vector2(-x, yMax), new Vector2(x, -yMax), new Vector2(-x, -yMax)});
        }

        for (int i = 0; i < YBoundVertsCount / 2; i++)
        {
            var y = i * dy;
            boundingVerts.AddRange(new[] { new Vector2(xMax, y), new Vector2(xMax, -y), new Vector2(-xMax, y), new Vector2(-xMax, -y) });
        }

        boundingVerts = boundingVerts.Distinct(new Vector2Comparer()).ToList();

        for (int i = 0; i < CellsCount; i++)
        {
            verts.Add(new Vector2(Random.Range(-xMax + dx, xMax - dx), Random.Range(-yMax + dy, yMax - dy)));
        }

        _graph = Fortune.ComputeVoronoiGraph(boundingVerts.Concat(verts).Select(v => new BenTools.Mathematics.Vector(v.x, v.y)));


        var cells = _graph.Cells.Select(c =>
                                        new VorSiteInfo
                                        {
                                            Site = new Vector2((float)c.Key[0], (float)c.Key[1]),
                                            Edges = c.Value,
                                            IsClosed = !c.Value.Any(ed => ed.IsInfinite || ed.IsPartlyInfinite)
                                        }).ToList();
        cells.RemoveAll(c => boundingVerts.Contains(c.Site));

        int j = 0;
        foreach (var cell in cells.Where(c => c.IsClosed))
        {
            AddSiteObject(cell, j++);
        }
    }

    private void AddSiteObject(VorSiteInfo siteInfo, int cellIndex)
    {
        //if (siteInfo.Edges.Any(e =>
        //{
        //    var a = e.VVertexA;
        //    var b = e.VVertexB;

        //    if (a[0] > 700f || a[0] < -700f || a[1] > 300f || a[1] < -300f)
        //    {
        //        return true;
        //    }
        //    if (b[0] > 700f || b[0] < -700f || b[1] > 300f || b[1] < -300f)
        //    {
        //        return true;
        //    }

        //    return false;
        //}))
        //{
        //    return;
        //}
        var cellObject = new GameObject { name = string.Format("VorCell_{0}", cellIndex) };
        //cellObject.SetActive (false);

        var meshFilter = cellObject.AddComponent<MeshFilter>();

        var site = new BenTools.Mathematics.Vector(siteInfo.Site.x, siteInfo.Site.y);

        cellObject.transform.position = new Vector3((float)site[0], (float)site[1]);
        cellObject.transform.parent = gameObject.transform;

        _vorCells.Add(cellObject);

        var edges = siteInfo.Edges.Select(e =>
        {
            var vA = new Vector2((float)e.VVertexA[0], (float)e.VVertexA[1]);
            var vB = new Vector2((float)e.VVertexB[0], (float)e.VVertexB[1]);
            var dA = vA - siteInfo.Site;
            var dB = vB - siteInfo.Site;
            var atanA = Mathf.Atan2(dA.y, dA.x);
            var atanB = Mathf.Atan2(dB.y, dB.x);


            VorEdgeInfo edge;
            if (atanA > 0f)
            {
                if (atanA < atanB)
                {
                    edge = new VorEdgeInfo { Start = vB - siteInfo.Site, End = vA - siteInfo.Site };
                }
                else
                {
                    if (atanA - atanB > Mathf.PI)
                    {
                        edge = new VorEdgeInfo { Start = vB - siteInfo.Site, End = vA - siteInfo.Site };
                    }
                    else
                    {
                        edge = new VorEdgeInfo { Start = vA - siteInfo.Site, End = vB - siteInfo.Site };
                    }
                }
            }
            else
            {
                if (atanA > atanB)
                {
                    edge = new VorEdgeInfo { Start = vA - siteInfo.Site, End = vB - siteInfo.Site };
                }
                else
                {
                    if (atanB - atanA > Mathf.PI)
                    {
                        edge = new VorEdgeInfo { Start = vA - siteInfo.Site, End = vB - siteInfo.Site };
                    }
                    else
                    {
                        edge = new VorEdgeInfo { Start = vB - siteInfo.Site, End = vA - siteInfo.Site };
                    }
                }
            }

            return edge;
        }).ToArray();

        var maxDistFromCenter = edges.Max(e => Mathf.Max(e.Start.magnitude, e.End.magnitude));
        if (edges[0].Start.magnitude > maxDistFromCenter)
        {
            maxDistFromCenter = edges[0].Start.magnitude;
        }
        maxDistFromCenter *= 2; // To make sure that UV coords will be in [-0.5; 0.5] range

        Mesh mesh = new Mesh();
        mesh.name = "cellMesh";

        var verts = new Vector3[edges.Length * 2 + 1];
        var triangles = new int[edges.Length * 3];
        var uvs = new Vector2[edges.Length * 2 + 1];

        verts[0] = Vector3.zero;   // Placing Cell Center at Origin of new GameObjects

        //verts [1] = new Vector3 (edges [0].Start.x, edges [0].Start.y, 0f);

        var shift = new Vector2(0.5f, 0.5f); // Shifting texture coords origin 
        uvs[0] = shift;
        uvs[1] = (edges[0].Start + shift) / maxDistFromCenter;

        for (int i = 0; i < edges.Length; i++)
        {
            var edge = edges[i];

            verts[2 * i + 1] = new Vector3(edges[i].Start.x, edges[i].Start.y, 0f);
            verts[2 * i + 2] = new Vector3(edges[i].End.x, edges[i].End.y, 0f);

            var uvStart = (edge.Start) / maxDistFromCenter + shift;
            var uvEnd = (edge.End) / maxDistFromCenter + shift;
            uvs[2 * i + 1] = new Vector3(uvStart.x, uvStart.y, 0f);
            uvs[2 * i + 2] = new Vector3(uvEnd.x, uvEnd.y, 0f);

            triangles[3 * i] = 0;
            triangles[3 * i + 1] = 2 * i + 1;
            triangles[3 * i + 2] = 2 * i + 2;
        }

        mesh.vertices = verts;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();

        meshFilter.sharedMesh = mesh;

        var meshRenderer = cellObject.AddComponent<MeshRenderer>();

        if (MeshMaterials.Length == 0)
        {
            meshRenderer.material.color = Color.green;
        }
        else
        {
            meshRenderer.material = MeshMaterials[Random.Range(0, MeshMaterials.Length - 1)];
        }

        cellObject.AddComponent<MeshCollider>();
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
