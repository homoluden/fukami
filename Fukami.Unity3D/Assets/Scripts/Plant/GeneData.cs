using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fukami.Genes
{

    public enum GeneType
    {
        Node,
        Bone,
        Joint,
        Stats
    }


    public class GeneData
    {
        public Guid Id = Guid.NewGuid();

        private GeneType _geneType;
        public GeneType GeneType
        {
            get { return _geneType; }
            set 
            { 
                _geneType = value;
                GeneTypeString = value.ToString();
            }
        }

		public string GeneTypeString;

        public GeneCondition Condition { get; set; }

        public float ApplicationPeriod { get; set; }

        public bool IsMultyApplicable { get; set; }

        public Dictionary<string, float> FloatModifiers = new Dictionary<string, float>();
        public Dictionary<string, Int32> IntegerModifiers = new Dictionary<string, Int32>();

        public bool IsValid { get; set; }
    }

}
