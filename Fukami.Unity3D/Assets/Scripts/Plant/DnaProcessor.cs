using UnityEngine;
using System.Collections;
using Fukami.Helpers;
using Fukami.Genes;
using Fukami.Entities;

public class DnaProcessor : MonoBehaviour
{
    #region Fields

    private float _age;
    private int _generation;

    #endregion


    #region Properties

	public string Dna;

    public string DnaString
    {
        get { return Dna; }
        set
        {
            Dna = value;
            ParseDna(value);
        }
    }

    #endregion

    void ParseDna(string dnaString)
    {
        if (string.IsNullOrEmpty(dnaString))
        {
            return;
        }

        var geneStrings = dnaString.Split(GenesManager.GENES_SEPARATORS, System.StringSplitOptions.RemoveEmptyEntries);

        foreach (var gene in geneStrings)
        {
			var geneData = GenesManager.Instance.ParseGene(gene);
			geneData.Generation = _generation;

			if (geneData.IsValid) {
				var geneProcessor = gameObject.AddComponent<GeneProcessor>();
				geneProcessor.Gene = geneData;
			}
        }
    }

    void OnDnaGenRequested(Wrap<int> generation)
    {
        if (generation.IsSet)
        {
            return;
        }

        generation.Value = _generation;
        generation.ValueSource = "DnaProcessor";
    }

    void OnDnaStringRequested(Wrap<string> dna)
    {
        if (dna.IsSet)
        {
            return;
        }

        dna.Value = Dna;
        dna.ValueSource = "DnaProcessor";
    }

    void Start()
    {
        _age = 0.0f;

		var dna = new Wrap<string>();

		if (gameObject.transform.parent != null) {
			var gen = new Wrap<int>();
				
	        gameObject.transform.parent.SendMessage("OnDnaGenRequested", gen);
	        _generation = gen.IsSet ? gen.Value + 1 : 0;

			gameObject.transform.parent.SendMessage("OnDnaStringRequested", dna);
		}
        else {
			SendMessage("OnSeedDnaStringRequested", dna);
		}

		if (dna.IsSet)
		{
			DnaString = dna.Value;
		}

    }

    void Update()
    {
        _age += Time.deltaTime;
    }
}
