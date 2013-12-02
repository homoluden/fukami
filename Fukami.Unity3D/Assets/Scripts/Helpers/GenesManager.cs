using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Fukami.Genes
{
    public sealed class GenesManager
    {

        #region Consts

        public static readonly char[] GENE_VALUES_SEPARATORS = ",/:".ToCharArray();
        public static readonly char[] GENES_SEPARATORS = "*| \n".ToCharArray();
        public static readonly string GENE_CONDITIONS_PATH = "Json/GeneConditions";        
        public static readonly float DEFAULT_GENE_APPLICATION_TIME = 3.0f;

        #endregion


        #region Fields
                
        #endregion


        #region Public Methods

        #endregion


        #region Singleton implementation

        private static volatile GenesManager _instance;
        private static readonly object SyncRoot = new System.Object();

        private GenesManager()
        {
            
        }

        public static GenesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new GenesManager();
                    }
                }

                return _instance;
            }
        }
        #endregion


        public GeneData ParseGene(string geneString)
        {
            var geneSubstrings = geneString.Split(GENE_VALUES_SEPARATORS, StringSplitOptions.RemoveEmptyEntries);

            var newGene = new GeneData();

			ParseGene(ref newGene, geneSubstrings);

            return newGene;
        }

        private void ParseGene(ref GeneData newGene, string[] geneSubstrings)
        {
			// node,[Node type],[Bone Type],[Base depth],[Depth tolerance],[Grow Time]

            if (geneSubstrings.Length != 6)
            {
                newGene.IsValid = false;
                return;
            }
            try {
				newGene.IsMultyApplicable = geneSubstrings[0] == "stats";
				newGene.GeneType = geneSubstrings[0];
				newGene.Subtype = Convert.ToUInt16(geneSubstrings[1],16);
				newGene.ApplicantSubtype = Convert.ToUInt16(geneSubstrings[2],16);
				newGene.BaseDepth = Convert.ToUInt16(geneSubstrings[3],16);
				newGene.DepthTolerance = Convert.ToUInt16(geneSubstrings[4],16);
				newGene.GrowTime = Convert.ToUInt32(geneSubstrings[5],16) * 0.01f;
			} catch (Exception ex) {
				Debug.LogError(ex.Message);

				newGene.IsValid = false;
				return;
			}            

            newGene.IsValid = true;
        }

    }

}
