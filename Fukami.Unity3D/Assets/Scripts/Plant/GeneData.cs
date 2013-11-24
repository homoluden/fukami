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
        public GeneType GeneType { get; set; }

        public GeneCondition Condition { get; set; }

        public float ApplicationPeriod { get; set; }

        public bool IsMultyApplicable { get; set; }

        public Dictionary<string, float> FloatModifiers = new Dictionary<string, float>();
        public Dictionary<string, Int32> IntegerModifiers = new Dictionary<string, Int32>();
    }

}
