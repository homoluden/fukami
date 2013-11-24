using System;
using UnityEngine;
using System.Collections;
using SimpleJSON;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Fukami.Genes
{
    public sealed class GenesManager
    {

        #region Consts

        public static readonly char[] GENE_VALUES_SEPARATORS = ",/:".ToCharArray();
        public static readonly char[] GENES_SEPARATORS = "*| ".ToCharArray();
        public static readonly string GENE_CONDITIONS_PATH = "Json/GeneConditions";
        public static readonly GeneCondition DEFAULT_GENE_CONDITION = new GeneCondition
        {
            idx = 0,
            AgeMax = float.PositiveInfinity,
            AgeMin = 0.0f,
            AncsMax = int.MaxValue,
            AncsMin = 0,
            AncTypes = new List<PartType>{
                PartType.Bone,
                PartType.Core,
                PartType.Node
            },
            distT = float.PositiveInfinity,
            distX = 0.0f,
            distY = 0.0f,
            MassMax = float.PositiveInfinity,
            MassMin = 0.0f,
            NrgMax = float.PositiveInfinity,
            NrgMin = 0.0f,
            SibsMax = int.MaxValue,
            SibsMin = 0
        };
        public static readonly float DEFAULT_GENE_APPLICATION_TIME = 3.0f;

        #endregion


        #region Fields
        private TextAsset _conditionsAsset;
        private GeneCondition[] _conditions;
        private Dictionary<UInt32, GeneCondition> _conditionsCache = new Dictionary<uint, GeneCondition>();

        #endregion


        #region Public Methods

        public GeneCondition GetCondition(UInt32 idx)
        {
            if (_conditionsCache.ContainsKey(idx))
            {
                return _conditionsCache[idx];
            }

            lock (_syncRoot)
            {
                if (_conditions.Length == 1)
                {
                    _conditionsCache.Add(idx, _conditions[0]);
                    return _conditions[0];
                }

                var selectedCondition = DEFAULT_GENE_CONDITION;

                foreach (var cond in _conditions)
                {
                    if (cond.idx > idx)
                    {
                        break;
                    }
                }

                for (int i = 1; i < _conditions.Length; i++)
                {
                    if (_conditions[i].idx > idx)
                    {
                        break;
                    }
                    selectedCondition = _conditions[i];
                }

                _conditionsCache.Add(idx, selectedCondition);
                return selectedCondition;
            }
        }

        #endregion


        #region Singleton implementation

        private static volatile GenesManager _instance;
        private static object _syncRoot = new System.Object();

        private GenesManager()
        {
            var path = GENE_CONDITIONS_PATH;
            _conditionsAsset = Resources.Load<TextAsset>(path);
            //var json = JSON.Parse(_conditionsAsset.text);
            var condWrap = JsonConvert.DeserializeObject<GeneConditionArray>(_conditionsAsset.text);
            _conditions = condWrap.conditions;
        }

        public static GenesManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
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

            switch (geneSubstrings[0])
            {
                case "bone":
                    newGene.GeneType = GeneType.Bone;
                    ParseBone(newGene, geneSubstrings);
                    break;
                case "node":
                    newGene.GeneType = GeneType.Node;
                    ParseNode(newGene, geneSubstrings);
                    break;
                case "joint":
                    newGene.GeneType = GeneType.Joint;
                    ParseJoint(newGene, geneSubstrings);
                    break;
                case "stats":
                    newGene.GeneType = GeneType.Stats;
                    ParseStats(newGene, geneSubstrings);
                    break;
                default:
                    break;
            }

            return newGene;
        }

        private void ParseStats(GeneData newGene, string[] geneSubstrings)
        {
            newGene.IsMultyApplicable = true;
            newGene.Condition = DEFAULT_GENE_CONDITION;
            newGene.ApplicationPeriod = DEFAULT_GENE_APPLICATION_TIME;
        }

        private void ParseJoint(GeneData newGene, string[] geneSubstrings)
        {
            newGene.IsMultyApplicable = false;
            newGene.Condition = DEFAULT_GENE_CONDITION;
            newGene.ApplicationPeriod = DEFAULT_GENE_APPLICATION_TIME;
        }

        private void ParseNode(GeneData newGene, string[] geneSubstrings)
        {
            newGene.IsMultyApplicable = false;
            newGene.Condition = DEFAULT_GENE_CONDITION;
            newGene.ApplicationPeriod = DEFAULT_GENE_APPLICATION_TIME;
        }

        private void ParseBone(GeneData newGene, string[] geneSubstrings)
        {
            newGene.IsMultyApplicable = false;
            newGene.Condition = DEFAULT_GENE_CONDITION;
            newGene.ApplicationPeriod = DEFAULT_GENE_APPLICATION_TIME;
        }
    }

}
