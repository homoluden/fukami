using UnityEngine;
using System.Collections;

public class ConstantScale : MonoBehaviour {

    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public float Tolerance = 0.3f;

	void Update () {
        var scaleDiff = (gameObject.transform.localScale.x + gameObject.transform.localScale.y) * 0.5;
        if (scaleDiff > Tolerance)
        {
            gameObject.transform.localScale = new Vector3(ScaleX, ScaleY, 1.0f);
        }        
	}
}
