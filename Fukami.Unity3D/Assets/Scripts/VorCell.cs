using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using System.Linq;

public class VorCell : MonoBehaviour
{

    #region Properties

    public VorCellInfo CellData;
    public Material MeshMaterial;

    #endregion


	void Start () {

        var meshFilter = gameObject.AddComponent<MeshFilter>();

        var edges = CellData.Edges.Select(e =>
        {
            var vA = new Vector2((float)e.VVertexA[0], (float)e.VVertexA[1]);
            var vB = new Vector2((float)e.VVertexB[0], (float)e.VVertexB[1]);
            var dA = vA - CellData.Site;
            var dB = vB - CellData.Site;
            var atanA = Mathf.Atan2(dA.y, dA.x);
            var atanB = Mathf.Atan2(dB.y, dB.x);


            VorEdgeInfo edge;
            if (atanA > 0f)
            {
                if (atanA < atanB)
                {
                    edge = new VorEdgeInfo { Start = vB - CellData.Site, End = vA - CellData.Site };
                }
                else
                {
                    if (atanA - atanB > Mathf.PI)
                    {
                        edge = new VorEdgeInfo { Start = vB - CellData.Site, End = vA - CellData.Site };
                    }
                    else
                    {
                        edge = new VorEdgeInfo { Start = vA - CellData.Site, End = vB - CellData.Site };
                    }
                }
            }
            else
            {
                if (atanA > atanB)
                {
                    edge = new VorEdgeInfo { Start = vA - CellData.Site, End = vB - CellData.Site };
                }
                else
                {
                    if (atanB - atanA > Mathf.PI)
                    {
                        edge = new VorEdgeInfo { Start = vA - CellData.Site, End = vB - CellData.Site };
                    }
                    else
                    {
                        edge = new VorEdgeInfo { Start = vB - CellData.Site, End = vA - CellData.Site };
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

        var cellCorners = new Vector3[edges.Length * 2 + 1];
        var triangles = new int[edges.Length * 3];
        var uvs = new Vector2[edges.Length * 2 + 1];

        cellCorners[0] = Vector3.zero;   // Placing Cell Center at Origin of new GameObjects

        //verts [1] = new Vector3 (edges [0].Start.x, edges [0].Start.y, 0f);

        var shift = new Vector2(0.5f, 0.5f); // Shifting texture coords origin 
        uvs[0] = shift;
        uvs[1] = (edges[0].Start + shift) / maxDistFromCenter;

        for (int i = 0; i < edges.Length; i++)
        {
            var edge = edges[i];

            cellCorners[2 * i + 1] = new Vector3(edges[i].Start.x, edges[i].Start.y, 0f);
            cellCorners[2 * i + 2] = new Vector3(edges[i].End.x, edges[i].End.y, 0f);

            var uvStart = (edge.Start) / maxDistFromCenter + shift;
            var uvEnd = (edge.End) / maxDistFromCenter + shift;
            uvs[2 * i + 1] = new Vector3(uvStart.x, uvStart.y, 0f);
            uvs[2 * i + 2] = new Vector3(uvEnd.x, uvEnd.y, 0f);

            triangles[3 * i] = 0;
            triangles[3 * i + 1] = 2 * i + 1;
            triangles[3 * i + 2] = 2 * i + 2;
        }

        mesh.vertices = cellCorners;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.RecalculateTangents();

        meshFilter.sharedMesh = mesh;

        var meshRenderer = gameObject.AddComponent<MeshRenderer>();

        if (MeshMaterial == null)
        {
            meshRenderer.material.color = Color.green;
        }
        else
        {
            meshRenderer.material = MeshMaterial;
        }

        var polyCollider = gameObject.AddComponent<EdgeCollider2D>();

        polyCollider.points = cellCorners.Skip(1).Select(c => new Vector2(c.x, c.y)).ToArray();//(0, cellCorners.Select(c => new Vector2(c.x, c.y)).ToArray());
	}
	
	void Update () {
	
	}

}
