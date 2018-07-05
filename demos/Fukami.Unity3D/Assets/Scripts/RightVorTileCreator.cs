using UnityEngine;
using System.Collections;

public class RightVorTileCreator : MonoBehaviour {

    private VorTile _tileScript;

    public GameObject Parent;

	// Use this for initialization
	void Start () {
        _tileScript = Parent.GetComponent<VorTile>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        Parent.SendMessageUpwards("OnRightTileTriggerEnter", Parent, SendMessageOptions.RequireReceiver);
    }
}
