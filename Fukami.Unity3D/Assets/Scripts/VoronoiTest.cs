using UnityEngine;
using System.Collections;
using SteeleSky.Voronoi;

public class VoronoiTest : MonoBehaviour
{
    #region Fields

    VoronoiGraph _graph;

    #endregion

    #region Properties

    public Vector2[] Vertices;

    #endregion

    // Use this for initialization
	void Start () {
        Vertices = new Vector2[] { 
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f)),
        new Vector2(Random.Range(-50f, 50f), Random.Range(-50f, 50f))
    };
        _graph = Fortune.GenerateGraph(Vertices);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
