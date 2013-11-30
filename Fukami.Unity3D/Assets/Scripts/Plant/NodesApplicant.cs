using UnityEngine;
using System.Collections;
using Fukami.Helpers;
using Fukami.Genes;
using System.Linq;
using Fukami.Entities;
using System;
using System.Collections.Generic;
using Assets.Scripts.Helpers;

public class NodesApplicant : MonoBehaviour {

	public string ApplicantType = "bone";
	public ushort Subtype;
    public List<ChildSlot> Slots = new List<ChildSlot>
        {
            new ChildSlot(),
            new ChildSlot(),
            new ChildSlot()
        }; 
    public Int32 Generation;

    void OnApplicantTypeRequested(Wrap<string> type)
    {
        if (type.IsSet || string.IsNullOrEmpty(ApplicantType)) return;

        type.Value = ApplicantType.ToLower();
        type.ValueSource = "BonesApplicant";
    }
	
	void OnApplyGene(GeneData gene)
	{
	    if (!GetIsApplicable(gene)) return;

	    switch (gene.GeneType.ToLower())
	    {
	        case "node":
	            AddNode(gene);
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
	
	private void AddNode(GeneData gene)
	{
	    var slot = Slots.FirstOrDefault(s => !s.IsOccupied);

	    if (slot == null) return;

	    slot.IsOccupied = true;

	    var nodePrefab = PrefabsManager.Instance.LoadPrefab(string.Format("{0}{1}", gene.GeneType, gene.Subtype));
        
        var newBody = (GameObject)Instantiate(nodePrefab,
                                              gameObject.transform.position,
                                              Quaternion.FromToRotation(Vector3.right, Vector3.up) *
            //Quaternion.AngleAxis(Random.Range(-slot.Angle, slot.Angle), Vector3.forward));
                                              Quaternion.AngleAxis(slot.Angle, Vector3.forward));

        newBody.transform.parent = gameObject.transform;
        newBody.SetActive(true);

        var hinge = newBody.AddComponent<HingeJoint2D>();
        hinge.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        hinge.connectedAnchor = new Vector2(slot.X, slot.Y);
        hinge.limits = new JointAngleLimits2D { min = -1.0f, max = 1.0f };
        hinge.useLimits = true;

        newBody.AddComponent<HingeSmoothPos>();

        gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.DontRequireReceiver);

	}
	
	bool GetIsApplicable(GeneData gene)
	{
		// Part Type check
	    if (gene.GeneType != "node" && gene.GeneType != "stats")
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
