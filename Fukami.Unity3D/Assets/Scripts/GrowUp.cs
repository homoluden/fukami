using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Fukami.Entities;

public class GrowUp : MonoBehaviour {

	#region Fields
	
	private float _timeLeft = 3f;
	private uint _currCycle = 0;

	#endregion

	#region Properties
	
	// The amount of seconds between new parts additions
	public float CycleTime = 3f;
	
	public int CyclesCount = 1;
	
	public GameObject PrefabToCreate;
	
	public int Generation = 0;
	
	public int GenerationsMax = 3;

	public List<ChildSlot> ChildSlots = new List<ChildSlot>{
		new ChildSlot{ X = 1.25f, Y = 0.25f, Angle = -45.0f}, 
		new ChildSlot{ X = 1.25f, Y = -0.25f, Angle = 45.0f}
	};

	#endregion

	#region Private Methods
	
	void Start () {
		
	}
	
	void Update () {

		_timeLeft -= Time.deltaTime;
		
		if (_timeLeft <= 0.0f) 
		{
			_timeLeft = CycleTime;
			
			if (Generation < GenerationsMax && _currCycle < CyclesCount && _currCycle < ChildSlots.Count) {

				var slot = ChildSlots[(int)_currCycle];

				var newBody = (GameObject)Instantiate(PrefabToCreate);

				newBody.name = gameObject.name + string.Format(".{0}", _currCycle);

				newBody.transform.parent = gameObject.transform;
				newBody.transform.localPosition = new Vector3(slot.X, slot.Y);
				newBody.transform.localRotation = Quaternion.AngleAxis(slot.Angle, new Vector3(0.0f, 0.0f, 1.0f));

				var hinge = newBody.GetComponent<HingeJoint2D>();
				if (hinge != null) {
					hinge.connectedBody = gameObject.GetComponent<Rigidbody2D>();
					hinge.connectedAnchor = new Vector2(slot.X, slot.Y);
				}

				var newGrowUp = newBody.GetComponent<GrowUp>();
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
