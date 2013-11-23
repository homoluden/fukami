using UnityEngine;
using System.Collections;

public class HingeSmoothPos : MonoBehaviour {

	public float AnchorX = 0.0f;
	public float AnchorY = 0.0f;
	public float Duration = 3.0f;

	private HingeJoint2D _hinge;
	private float _age = Time.fixedDeltaTime;

	// Use this for initialization
	void Start () {
		_hinge = gameObject.GetComponent<HingeJoint2D>();
		if (_hinge == null) {
			enabled = false;
		}
		else {
			AnchorX = _hinge.connectedAnchor.x;
			AnchorY = _hinge.connectedAnchor.y;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (_age >= Duration) {
			enabled = false;
			_age = 0.0f;
			_hinge.connectedAnchor = new Vector2(AnchorX, AnchorY);
			return;
		}

		_age += Time.deltaTime;

		var t = _age / Duration;

		var anchor = new Vector2(Mathf.Lerp(0.0f, AnchorX, t), Mathf.Lerp(0.0f, AnchorY, t));

		_hinge.connectedAnchor = anchor;
	}
}
