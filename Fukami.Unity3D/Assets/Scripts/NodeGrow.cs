using UnityEngine;
using System.Collections;
using Fukami.Entities;

public class NodeGrow : MonoBehaviour {

	#region Fields
	
	private float _growTimeLeft = 3.0f;
	private float _growTime = 3.0f;
	private float _lifeTimeLeft = 10.0f;
	private float _lifeTime = 10.0f;
	private bool _growed = false;
	
	#endregion
	
	#region Properties

	public float DeathDelay = 1.0f;

	public float LifeTime {
		get {return _lifeTime;}
		set {
			_lifeTime = value;
			_lifeTimeLeft = value;
		}
	}

	public float GrowTime {
		get {return _growTime;}
		set {
			_growTime = value;
			_growTimeLeft = value;
		}
	}

	public GameObject PrefabToCreate;
	
	public int Generation = 0;
	
	public int GenerationsMax = 3;
	
	public ChildSlot ChildSlot = new ChildSlot{ X = 0.0f, Y = 0.0f, Angle = 45.0f};
	
	#endregion
	
	#region Private Methods
	
	void Start () {
		
	}
	
	void Update () {
		if (_lifeTimeLeft <= 0.0f) {
			if (gameObject.GetComponent<Rigidbody2D>().transform.childCount == 0) {
				Destroy(gameObject, DeathDelay);
			}
			else {
				_lifeTimeLeft = _lifeTime;
			}
		}
		_lifeTimeLeft -= Time.deltaTime;

		if (_growed) {
			return;
		}

		_growTimeLeft -= Time.deltaTime;
		
		if (Generation < GenerationsMax && _growTimeLeft <= 0.0f) 
		{
			var slot = ChildSlot;
			
			var newBody = (GameObject)Instantiate(PrefabToCreate, 
                      gameObject.transform.position,
                      Quaternion.identity);
			newBody.transform.parent = gameObject.transform;
			newBody.transform.localPosition = new Vector3(slot.X, slot.Y);
			newBody.transform.localRotation = Quaternion.AngleAxis(slot.Angle, new Vector3(0.0f, 0.0f, 1.0f));

			var slide = newBody.AddComponent<SliderJoint2D>();
			slide.connectedBody = gameObject.GetComponent<Rigidbody2D>();
			slide.connectedAnchor = new Vector2(slot.X, slot.Y);
			slide.limits = new JointTranslationLimits2D{min = -0.1f, max = 0.1f};
			slide.useLimits = true;

//			var dist = newBody.AddComponent<DistanceJoint2D>();
//			dist.connectedBody = gameObject.GetComponent<Rigidbody2D>();
//			dist.distance = 0.01f;
			
			var newGrowUp = newBody.GetComponent<BoneGrow>();
			if (newGrowUp != null) {
				newGrowUp.GenerationsMax = GenerationsMax;
				newGrowUp.Generation = Generation;
			}

            var constScale = gameObject.GetComponent<ConstantScale>().enabled = true;

			_growed = true;
		}
	}
	
	#endregion
}
