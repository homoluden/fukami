using UnityEngine;
using System.Collections;
using Fukami.Entities;
using System.Collections.Generic;

public class NodeGrow : MonoBehaviour {

	#region Fields
	
	private float _growTimeLeft = 3.0f;
	private float _growTime = 3.0f;
	private float _lifeTimeLeft = 10.0f;
	private float _lifeTime = 10.0f;
	private bool _growed = false;
    private List<GameObject> _children = new List<GameObject>();
	
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
            
            _children.RemoveAll(ch => ch == null); // Overloaded comparer. Returns true if object destroyed

			if (_children.Count == 0) {
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
                      gameObject.transform.position + new Vector3(slot.X, slot.Y),
                      gameObject.transform.rotation * Quaternion.AngleAxis(slot.Angle, new Vector3(0.0f, 0.0f, 1.0f)));
            _children.Add(newBody);
            
			var slide = newBody.AddComponent<SliderJoint2D>();
			slide.connectedBody = gameObject.GetComponent<Rigidbody2D>();
			slide.connectedAnchor = new Vector2(slot.X, slot.Y);
			slide.limits = new JointTranslationLimits2D{min = -0.1f, max = 0.1f};
			slide.useLimits = true;

			var newGrowUp = newBody.GetComponent<BoneGrow>();
			if (newGrowUp != null) {
				newGrowUp.GenerationsMax = GenerationsMax;
				newGrowUp.Generation = Generation;
			}

            //var constScale = gameObject.GetComponent<ConstantScale>().enabled = true;

			_growed = true;
		}
	}
	
	#endregion
}
