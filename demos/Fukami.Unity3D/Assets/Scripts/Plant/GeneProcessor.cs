using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Fukami.Helpers;
using Fukami.Genes;

public class GeneProcessor : MonoBehaviour
{
    #region Fields

    private float _age;

    #endregion


    #region Properties

	public GeneData Gene;

    #endregion


	void Start () {
	}
	
	void Update () {
        
		if (Gene != null)
        {
            _age += Time.deltaTime;
        }
        
		if (_age >= Gene.GrowTime)
        {
            enabled = false;
            
			SendMessage("OnApplyGene", Gene);

            return;
        }
    }
}
