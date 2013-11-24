using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Fukami.Genes
{
    public enum PartType{
        Core,
        Node,
        Bone
    }

    public class GeneConditionArray
    {
        public GeneCondition[] conditions { get; set; }
    }

    public class GeneCondition
    {
        public UInt32 idx;
        public float distX;
        public float distY;
		public float distT;
		public int AncsMin;
		public int AncsMax;
		public List<PartType> AncTypes;
		public int SibsMin;
		public int SibsMax;
		public float MassMin;
		public float MassMax;
		public float AgeMin;
		public float AgeMax;
        public float NrgMin;
		public float NrgMax;
    }
}
