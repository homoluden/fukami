using UnityEngine;
using System.Collections;

public class QuatNormalize : MonoBehaviour {

	void Update () {
		float angle;
		Vector3 axis;
		gameObject.transform.rotation.ToAngleAxis(out angle, out axis);
				
		gameObject.transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0.0f, 0.0f, 1.0f));
	}
}
