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
		
		public bool IsMultyApplicable;
		
		public bool IsValid { get; set; }

		public string GeneType;
		
		public ushort Subtype;

		public ushort ApplicantSubtype;

		public ushort BaseDepth;

		public ushort DepthTolerance;

		public float GrowTime;

		public int Generation;
    }

}
