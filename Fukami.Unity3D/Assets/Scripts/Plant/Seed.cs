using UnityEngine;
using System.Collections;
using Fukami.Helpers;

public class Seed : MonoBehaviour
{

		public string Dna;

		void OnSeedDnaStringRequested (Wrap<string> dna)
		{
				dna.Value = Dna;
				dna.ValueSource = "Seed";
		}

}
