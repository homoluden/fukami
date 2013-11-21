using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fukami.Entities;

public class BoneGrow : MonoBehaviour {

	#region Fields

	private uint _currCycle = 0;
	private float _growTimeLeft = 3.0f;
	private float _growTime = 3.0f;
    private GameObject _child;
    private SmoothGrow _smoothGrow;
	#endregion
	
	#region Properties

	public float GrowTime {
		get {return _growTime;}
		set {
			_growTime = value;
			_growTimeLeft = value;
		}
	}

	public int CyclesCount = 1;
	
	public GameObject PrefabToCreate;
	
	public int Generation = 0;
	
	public int GenerationsMax = 3;
	
	public List<ChildSlot> ChildSlots = new List<ChildSlot>{
		new ChildSlot{ X = 2.5f, Y = 0.0f, Angle = 45.0f}
	};
	
	#endregion
	
	#region Private Methods
	
	void Start () {
        _smoothGrow = gameObject.GetComponent<SmoothGrow>();
	}
	
	void Update () {
		
		_growTimeLeft -= Time.deltaTime;
		
		if (_growTimeLeft <= 0.0f) 
		{
			_growTimeLeft = GrowTime;
			
			if (Generation < GenerationsMax && _currCycle < CyclesCount && _currCycle < ChildSlots.Count) {
				
				var slot = ChildSlots[(int)_currCycle];
				
				var newBody = (GameObject)Instantiate(PrefabToCreate,
                              gameObject.transform.position,// + new Vector3(slot.X, slot.Y),
                              gameObject.transform.rotation * Quaternion.AngleAxis(slot.Angle, new Vector3(0.0f, 0.0f, 1.0f)));

                _child = newBody;
                if (_smoothGrow != null)
                {
                    _smoothGrow.AddChild(_child, new Vector2(slot.X, slot.Y));
                }

				var hinge = newBody.AddComponent<HingeJoint2D>();
				hinge.connectedBody = gameObject.GetComponent<Rigidbody2D>();
				hinge.connectedAnchor = new Vector2(slot.X, slot.Y);
				hinge.limits = new JointAngleLimits2D{min = -0.1f, max = 0.1f};
				hinge.motor = new JointMotor2D{motorSpeed = -1000.0f, maxMotorTorque = 10000.0f};
				hinge.useLimits = true;
				hinge.useMotor = false;
							
				var newGrowUp = newBody.GetComponent<NodeGrow>();
				if (newGrowUp != null) {
					newGrowUp.GenerationsMax = GenerationsMax;
					newGrowUp.Generation = Generation + 1;
				}
				_currCycle++;
			}
		}
	}
	
	#endregion
}
