using UnityEngine;
using System.Collections;
using Fukami.Helpers;

public class Seed : MonoBehaviour
{

		public string Dna;

	void Start(){
		var dnaAsset = Resources.Load<TextAsset>("dna");
		Dna = dnaAsset.text.Replace("\n","*");
	}

		void OnSeedDnaStringRequested (Wrap<string> dna)
		{
				dna.Value = Dna;
				dna.ValueSource = "Seed";
		}

}
