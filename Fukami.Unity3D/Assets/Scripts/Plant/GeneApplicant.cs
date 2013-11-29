using UnityEngine;
using System.Collections;
using Fukami.Helpers;
using Fukami.Genes;
using System.Linq;
using Fukami.Entities;
using System;

public class GeneApplicant : MonoBehaviour {

	public string ApplicantType;
	public ushort Subtype;
	public string Slots;

    void OnApplicantTypeRequested(Wrap<string> type)
    {
        if (type.IsSet)
        {
            return;
        }

		if (!string.IsNullOrEmpty(ApplicantType))
        {
			type.Value = ApplicantType.ToLower();
			type.ValueSource = "GeneApplicant";
        }
    }
	
	void OnApplyGene(GeneData gene)
	{
		if (GetIsApplicable(gene)) {
			switch (gene.GeneType.ToLower())
			{
			case "node":
				AddNode(gene);
				break;
			case "bone":
				AddBone(gene);
				break;
			case "joint":
				AddJoint(gene);
				break;
			case "stats":
				AddStats(gene);
				break;
			default:
				break;
			}
		}
	}
	
	private void AddStats(GeneData gene)
	{
		throw new System.NotImplementedException();
	}
	
	private void AddJoint(GeneData gene)
	{
		//        float x, y;
		//        gene.FloatModifiers.TryGetValue("X", out x);
		//        gene.FloatModifiers.TryGetValue("Y", out y);
		//
		//        var slot = new ChildSlot { X = x, Y = y };
		//
		//        var newBody = (GameObject)Instantiate(BonePrefab,
		//                  gameObject.transform.position + new Vector3(slot.X, slot.Y),
		//                  gameObject.transform.rotation);
		//        newBody.transform.parent = gameObject.transform;
		//        newBody.SetActive(true);
		//
		//        var spring = newBody.AddComponent<SpringJoint2D>();
		//        spring.connectedBody = gameObject.GetComponent<Rigidbody2D>();
		//        spring.distance = 0.1f;
		//        spring.dampingRatio = 0.01f;
		//        spring.frequency = 10.0f;
		//
		//		gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.DontRequireReceiver);    
		
	}
	
	private void AddBone(GeneData gene)
	{
//		float angle;
//		gene.FloatModifiers.TryGetValue("Angle", out angle);
//		
//		var newBody = (GameObject)Instantiate(BonePrefab,
//		                                      gameObject.transform.position,
//		                                      gameObject.transform.rotation *
//		                                      Quaternion.AngleAxis(angle, Vector3.forward));
//		newBody.transform.parent = gameObject.transform;
//		newBody.SetActive(true);
//		
//		//var slide = newBody.AddComponent<SliderJoint2D>();
//		var slide = newBody.AddComponent<HingeJoint2D>();
//		slide.connectedBody = gameObject.GetComponent<Rigidbody2D>();
//		slide.connectedAnchor = new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f));
//		//slide.limits = new JointTranslationLimits2D { min = -0.1f, max = 0.1f };
//		slide.limits = new JointAngleLimits2D { min = -1.0f, max = 1.0f };
//		slide.useLimits = true;
//		
//		gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.RequireReceiver);
	}
	
	private void AddNode(GeneData gene)
	{
//		float angle;
//		gene.FloatModifiers.TryGetValue("Angle", out angle);
//		
//		float x, y;
//		gene.FloatModifiers.TryGetValue("X", out x);
//		gene.FloatModifiers.TryGetValue("Y", out y);
//		
//		var slot = new ChildSlot { X = x, Y = y, Angle = angle };
//		
//		var newBody = (GameObject)Instantiate(NodePrefab,
//		                                      gameObject.transform.position ,
//		                                      Quaternion.FromToRotation(Vector3.right, Vector3.up) *
//		                                      //Quaternion.AngleAxis(Random.Range(-slot.Angle, slot.Angle), Vector3.forward));
//		                                      Quaternion.AngleAxis(slot.Angle, Vector3.forward));
//		
//		newBody.transform.parent = gameObject.transform;
//		newBody.SetActive(true);
//		
//		var hinge = newBody.AddComponent<HingeJoint2D>();
//		hinge.connectedBody = gameObject.GetComponent<Rigidbody2D>();
//		hinge.connectedAnchor = new Vector2(slot.X, slot.Y);
//		hinge.limits = new JointAngleLimits2D { min = -1.0f, max = 1.0f };
//		hinge.motor = new JointMotor2D { motorSpeed = -1000.0f, maxMotorTorque = 10000.0f };
//		hinge.useLimits = true;
//		hinge.useMotor = false;
//		
//		newBody.AddComponent<HingeSmoothPos>();
//		
//		gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.DontRequireReceiver);
	}
	
	bool GetIsApplicable(GeneData gene)
	{
		// Part Type check
		switch (gene.GeneType)
		{
		case "node":
			if (ApplicantType != "bone" && ApplicantType != "core")
			{
				return false;
			}
			break;
		case "joint":
		case "bone":
			if (ApplicantType != "node")
			{
				return false;
			}
			break;		
		case "stats":                    
		default:                    
			break;
		}    

		// Subtype check
		if (gene.Subtype != 0 && gene.ApplicantSubtype != Subtype) {
			return false;
		}

		// Generation check
		var gen = gene.Generation;        
		if (gen != gene.BaseDepth)
		{
			var actDistance = Mathf.Abs(gen - gene.BaseDepth);
			if (UnityEngine.Random.Range(0f, 1f) < actDistance / (gene.DepthTolerance + 1)) {
				return false;
			}
		}

		return true;
	}

	void ParseSlots(){
		// Slots example:   FFFF,FFFF,FFFF*[X],[Y],[Angle]

		var slotStrings = Slots.Split (GenesManager.GENES_SEPARATORS, System.StringSplitOptions.RemoveEmptyEntries);

		var slots = slotStrings.Select(str => {
			var slotVals = str.Split(GenesManager.GENE_VALUES_SEPARATORS, System.StringSplitOptions.RemoveEmptyEntries);
			return new ChildSlot{
				X = Convert.ToInt16(slotVals[0], 16) * 0.01f,
				Y = Convert.ToInt16(slotVals[1], 16) * 0.01f,
				Angle = Convert.ToInt16(slotVals[0], 16) * 5.493164E-3f
			};
		});
	}


}
