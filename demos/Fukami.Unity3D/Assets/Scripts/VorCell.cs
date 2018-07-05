using UnityEngine;
using System.Collections;
using Assets.Scripts.Helpers;
using System.Linq;

using FortuneVoronoi;

public class VorCell : MonoBehaviour
{

    #region Properties

	public VoronoiCell CellData;
    public Material MeshMaterial;

    #endregion


	void Start () {

        var meshFilter = gameObject.AddComponent<MeshFilter>();

        var edges = CellData.Edges.Select(e =>
        {
            var vA = e.VVertexA;
            var vB = e.VVertexB;
            var dA = vA - CellData.Site;
            var dB = vB - CellData.Site;
			var atanA = Mathf.Atan2((float)dA.Y, (float)dA.X);
			var atanB = Mathf.Atan2((float)dB.Y, (float)dB.X);


            VoronoiEdge edge;
            if (atanA > 0f)
            {
                if (atanA < atanB)
                {
					edge = new VoronoiEdge { VVertexA = vB - CellData.Site, VVertexB = vA - CellData.Site };
                }
                else
                {
                    if (atanA - atanB > Mathf.PI)
                    {
						edge = new VoronoiEdge { VVertexA = vB - CellData.Site, VVertexB = vA - CellData.Site };
                    }
                    else
                    {
						edge = new VoronoiEdge { VVertexA = vA - CellData.Site, VVertexB = vB - CellData.Site };
                    }
                }
            }
            else
            {
                if (atanA > atanB)
                {
					edge = new VoronoiEdge { VVertexA = vA - CellData.Site, VVertexB = vB - CellData.Site };
                }
                else
                {
                    if (atanB - atanA > Mathf.PI)
                    {
						edge = new VoronoiEdge { VVertexA = vA - CellData.Site, VVertexB = vB - CellData.Site };
                    }
                    else
                    {
						edge = new VoronoiEdge { VVertexA = vB - CellData.Site, VVertexB = vA - CellData.Site };
                    }
                }
            }

            return edge;
        }).ToArray();

		var maxDistFromCenter = edges.Max(e => Mathf.Max((float)e.VVertexA.Length, (float)e.VVertexB.Length));
		if (edges[0].VVertexA.Length > maxDistFromCenter)
        {
			maxDistFromCenter = (float)edges[0].VVertexA.Length;
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
		uvs[1] = new Vector2(((float)edges[0].VVertexA.X + shift.x) / maxDistFromCenter, ((float)edges[0].VVertexA.Y + shift.y) / maxDistFromCenter);

        for (int i = 0; i < edges.Length; i++)
        {
            var edge = edges[i];

			cellCorners[2 * i + 1] = new Vector3((float)edges[i].VVertexA.X, (float)edges[i].VVertexA.Y, 0f);
			cellCorners[2 * i + 2] = new Vector3((float)edges[i].VVertexB.X, (float)edges[i].VVertexB.Y, 0f);

			var uvStart = shift + new Vector2((float)edge.VVertexA.X / maxDistFromCenter, (float)edge.VVertexA.Y / maxDistFromCenter);
			var uvEnd = shift + new Vector2((float)edge.VVertexB.X / maxDistFromCenter, (float)edge.VVertexB.Y / maxDistFromCenter);
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

        polyCollider.points = cellCorners.Skip(1).Select(c => new Vector2(c.x, c.y)).ToArray();
	}
	
	void Update () {
	
	}

}
