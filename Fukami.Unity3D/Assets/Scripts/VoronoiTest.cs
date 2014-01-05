using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BenTools.Mathematics;

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

    #endregion

    #region Properties

		public Vector2[] Vertices;
		public Material[] MeshMaterials;

    #endregion

		// Use this for initialization
		void Start ()
		{
				Vertices = new Vector2[] { 
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
		        new Vector2 (Random.Range (-250f, 250f), Random.Range (-50f, 50f)),
				new Vector2 (-300f, 0f),
				new Vector2 (300f, 0f),
				new Vector2 (0f, 300f),
				new Vector2 (0f, -300f),
				
				new Vector2 (-300f, -300f),
				new Vector2 (300f, -300f),
				new Vector2 (-300f, 300f),
				new Vector2 (300f, 300f)
		    };
				_graph = Fortune.ComputeVoronoiGraph (Vertices.Select (v => new BenTools.Mathematics.Vector (v.x, v.y)));

				var cells = Vertices.Select (v =>
				{
						var edges = _graph.Edges.Where (edge => {
								var site = new BenTools.Mathematics.Vector (v.x, v.y);
								return edge.LeftData.Equals (site) || edge.RightData.Equals (site);
						}).ToArray ();
	
						var cell = new VorSiteInfo { 
					Site = v, 
					Edges = edges, 
					IsClosed = !edges.Any(ed => ed.IsInfinite || ed.IsPartlyInfinite)
				};
			
						return cell;
				}).ToArray ();
		
				int i = 0;
				foreach (var cell in cells.Where(c => c.IsClosed)) {
						AddSiteObject (cell, i++);
				}
		}

		private void AddSiteObject (VorSiteInfo siteInfo, int cellIndex)
		{
				var cellObject = new GameObject{name = string.Format("VorCell_{0}", cellIndex)};

				var meshFilter = cellObject.AddComponent<MeshFilter> ();

				var site = new BenTools.Mathematics.Vector (siteInfo.Site.x, siteInfo.Site.y);

				cellObject.transform.position = new Vector3 ((float)site [0], (float)site [1]);
				cellObject.transform.parent = gameObject.transform;

				var edges = siteInfo.Edges.Select (e => {
						var vA = new Vector2 ((float)e.VVertexA [0], (float)e.VVertexA [1]);
						var vB = new Vector2 ((float)e.VVertexB [0], (float)e.VVertexB [1]);
			var dA = vA - siteInfo.Site;
			var dB = vB - siteInfo.Site;
			var atanA = Mathf.Atan2(dA.y, dA.x);
			var atanB = Mathf.Atan2(dB.y, dB.x);


						VorEdgeInfo edge;
			if (atanA > 0f) {
				if (atanA < atanB) {
					edge = new VorEdgeInfo{Start = vB - siteInfo.Site, End = vA - siteInfo.Site};
				}
				else {
					if (atanA - atanB > Mathf.PI) {
						edge = new VorEdgeInfo{Start = vB - siteInfo.Site, End = vA - siteInfo.Site};
					}
					else {
						edge = new VorEdgeInfo{Start = vA - siteInfo.Site, End = vB - siteInfo.Site};
					}
				}
			}
			else {
				if (atanA > atanB) {
					edge = new VorEdgeInfo{Start = vA - siteInfo.Site, End = vB - siteInfo.Site};
				}
				else {
					if (atanB - atanA > Mathf.PI) {
						edge = new VorEdgeInfo{Start = vA - siteInfo.Site, End = vB - siteInfo.Site};
					}
					else {
						edge = new VorEdgeInfo{Start = vB - siteInfo.Site, End = vA - siteInfo.Site};
					}
				}
			}
						
						return edge;
				}).ToArray ();

				var maxDistFromCenter = edges.Max (e => Mathf.Max(e.Start.magnitude, e.End.magnitude));
				if (edges [0].Start.magnitude > maxDistFromCenter) {
						maxDistFromCenter = edges [0].Start.magnitude;
				}
				maxDistFromCenter *= 2; // To make sure that UV coords will be in [-0.5; 0.5] range

				Mesh mesh = new Mesh ();		
				mesh.name = "cellMesh";
				
				var verts = new Vector3[edges.Length*2 + 1];
				var triangles = new int[edges.Length * 3];
				var uvs = new Vector2[edges.Length * 2 + 1];

				verts [0] = Vector3.zero;   // Placing Cell Center at Origin of new GameObjects

				//verts [1] = new Vector3 (edges [0].Start.x, edges [0].Start.y, 0f);

				var shift = new Vector2 (0.5f, 0.5f); // Shifting texture coords origin 
				uvs [0] = shift;
				uvs [1] = (edges [0].Start + shift) / maxDistFromCenter;

				int i = 0;
				for (; i < edges.Length; i++) { 
						var edge = edges [i];

						verts [2*i + 1] = new Vector3 (edges [i].Start.x, edges [i].Start.y, 0f);
						verts [2*i + 2] = new Vector3 (edges [i].End.x, edges [i].End.y, 0f);
 
			var uvStart = (edge.Start) / maxDistFromCenter + shift;
			var uvEnd = (edge.End) / maxDistFromCenter + shift;	
			uvs [2*i + 1] = new Vector3 (uvStart.x, uvStart.y, 0f);
			uvs [2*i + 2] = new Vector3 (uvEnd.x, uvEnd.y, 0f);

						triangles [3 * i] = 0;
						triangles [3 * i + 1] = 2*i + 1;
						triangles [3 * i + 2] = 2*i + 2;
				}

//		triangles [3 * i] = 0;
//		triangles [3 * i + 1] = 1;
//		triangles [3 * i + 2] = 2*i - 3;

				mesh.vertices = verts;
				mesh.uv = uvs;
				mesh.triangles = triangles;

				mesh.RecalculateNormals ();
				mesh.RecalculateBounds();
				mesh.Optimize();
		
				meshFilter.sharedMesh = mesh;

				var meshRenderer = cellObject.AddComponent<MeshRenderer> ();
				
				if (MeshMaterials.Length == 0) {
						meshRenderer.material.color = Color.green;
				} else {
						meshRenderer.material = MeshMaterials [Random.Range (0, MeshMaterials.Length - 1)];
				}				

				cellObject.AddComponent<MeshCollider> ();
		}

		// Update is called once per frame
		void Update ()
		{
	
		}
}
