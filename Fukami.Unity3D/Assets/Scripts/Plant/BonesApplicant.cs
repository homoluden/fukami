using UnityEngine;
using System.Collections;
using Fukami.Helpers;
using Fukami.Genes;
using System.Linq;
using Fukami.Entities;
using System;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

public class BonesApplicant : MonoBehaviour {

	public string ApplicantType = "node";
	public ushort Subtype;
	public List<ChildSlot> Slots = new List<ChildSlot>{
		new ChildSlot{Angle = 0f},
		new ChildSlot{Angle = -30f},
		new ChildSlot{Angle = 30f}
	};
    public Int32 Generation;

    void OnApplicantTypeRequested(Wrap<string> type)
    {
        if (type.IsSet)
        {
            return;
        }

		if (!string.IsNullOrEmpty(ApplicantType))
        {
			type.Value = ApplicantType.ToLower();
			type.ValueSource = "BonesApplicant";
        }
    }
	
	void OnApplyGene(GeneData gene)
	{
	    if (!GetIsApplicable(gene)) return;

	    switch (gene.GeneType.ToLower())
	    {
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
	    var slot = Slots.FirstOrDefault(s => !s.IsOccupied);
	    if (slot == null)
	    {
	        return;
	    }

	    slot.IsOccupied = true;

	    var bonePrefab = PrefabsManager.Instance.LoadPrefab(string.Format("{0}{1}", gene.GeneType, gene.Subtype));
        var newBody = (GameObject)Instantiate(bonePrefab,
                                              gameObject.transform.position,
                                              gameObject.transform.rotation *
                                              Quaternion.AngleAxis(slot.Angle, Vector3.forward));
        newBody.transform.parent = gameObject.transform;
        newBody.SetActive(true);

        //var slide = newBody.AddComponent<SliderJoint2D>();
        var slide = newBody.AddComponent<HingeJoint2D>();
        slide.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        slide.connectedAnchor = new Vector2(slot.X, slot.Y);
        //slide.limits = new JointTranslationLimits2D { min = -0.1f, max = 0.1f };
        slide.limits = new JointAngleLimits2D { min = -1.0f, max = 1.0f };
        slide.useLimits = true;

        //gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.DontRequireReceiver);
	}
	
	bool GetIsApplicable(GeneData gene)
	{
		// Part Type check
	    if (gene.GeneType != "bone" && gene.GeneType != "stats" && gene.GeneType != "joint")
	    {
	        return false;
	    }

		// Subtype check
		if (gene.ApplicantSubtype != 0 && gene.ApplicantSubtype != Subtype) {
			return false;
		}

		// Generation check
		var gen = Generation;        
		if (gen != gene.BaseDepth)
		{
			var actDistance = Mathf.Abs(gen - gene.BaseDepth);
			if (UnityEngine.Random.Range(0f, 1f) < actDistance / (gene.DepthTolerance + 1)) {
				return false;
			}
		}

		return true;
	}

    private void Start()
    {
        var genWrap = new Wrap<Int32>();
        gameObject.SendMessage("OnDnaGenRequested", genWrap, SendMessageOptions.DontRequireReceiver);
        Generation = genWrap.IsSet ? genWrap.Value : 0;
    }
}
