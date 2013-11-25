using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Fukami.Helpers;
using Fukami.Genes;

public class GeneProcessor : MonoBehaviour
{
    #region Fields

    private string _geneStr;
    private float _age;
    private GeneData _gene;
    private float _mass;

    #endregion


    #region Properties

    public string GeneString
    {
        get { return _geneStr; }
        set 
        { 
            _geneStr = value;
			_gene = ParseGene(value);
            if (!_gene.IsValid)
            {
                Destroy(this);
            }
		}
	}

    #endregion


    bool GetIsApplicable(GeneData gene)
    {
        // Generation check
        var gen = new Wrap<int>();
        SendMessage("OnDnaGenRequested", gen);
        if (!gen.IsSet || gen.Value < gene.Condition.AncsMin || gen.Value > gene.Condition.AncsMax)
        {
            return false;
        }

        // Mass check
        if (_mass < gene.Condition.MassMin || _mass > gene.Condition.MassMax)
        {
            return false;
        }

        // Age check
        var age = new Wrap<float>();
        SendMessage("OnDnaAgeRequested", age);

        if (!age.IsSet || age.Value < gene.Condition.AgeMin || age.Value > gene.Condition.AgeMax)
        {
            return false;
        }

        // Part Type check
        
        var type = new Wrap<PartType>();
        SendMessage("OnApplicantTypeRequested", type);
        
        if (type.IsSet)
        {
            switch (gene.GeneType)
            {
                case GeneType.Node:
                    if (type.Value != PartType.Bone && type.Value != PartType.Core)
                    {
                        return false;
                    }
                    break;
                case GeneType.Bone:
                    if (type.Value != PartType.Node)
                    {
                        return false;
                    }
                    break;
                case GeneType.Joint:
                    if (type.Value != PartType.Node)
                    {
                        return false;
                    }
                    break;
                case GeneType.Stats:                    
                default:
                    if (!gene.Condition.AncTypes.Contains(type.Value))
                    {
                        return false;
                    }
                    break;
            }            
        }
		else { // !type.IsSet
			return false;
		}
        
        // Positions check
        var posDiff = new Vector2(gameObject.transform.position.x - gene.Condition.distX, gameObject.transform.position.y - gene.Condition.distY);
        var scalarDiff = posDiff.magnitude;
        if (scalarDiff > gene.Condition.distT)
        {
            return false;
        }

		if (gameObject.transform.childCount < gene.Condition.SibsMin || 
		    gameObject.transform.childCount > gene.Condition.SibsMax) 
		{
			return false;
		}

        // TODO: Check NRG and Siblings

        return true;
    }

	void Start () {
        var rigid = gameObject.GetComponent<Rigidbody2D>();
        _mass = rigid.mass;
	}
	
	void Update () {
        
        if (_gene != null)
        {
            _age += Time.deltaTime;
        }
        
        if (_age >= _gene.ApplicationPeriod)
        {
            enabled = false;
            
            if (GetIsApplicable(_gene))
            {
                SendMessage("OnApplyGene", _gene);
            }
            else
            {
                Destroy(this, 1.0f);
            }
            return;
        }
    }


	public static GeneData ParseGene(string geneString)
	{
		var gene = GenesManager.Instance.ParseGene(geneString);
        print(string.Format("Gene ({1}) '{0}' parsed", geneString, gene.Id));

        return gene;
    }

}
