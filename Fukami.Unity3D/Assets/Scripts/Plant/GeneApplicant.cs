using UnityEngine;
using System.Collections;
using Fukami.Helpers;
using Fukami.Genes;

public class GeneApplicant : MonoBehaviour {

    public string ApplicantType;

    void OnApplicantTypeRequested(Wrap<PartType> type)
    {
        if (type.IsSet)
        {
            return;
        }

        if (!string.IsNullOrEmpty(ApplicantType))
        {
            if (ApplicantType == "Bone")
            {
                type.Value = PartType.Bone;
            }
            else if (ApplicantType == "Node")
            {
                type.Value = PartType.Node;
            }
            else if (ApplicantType == "Core")
            {
                type.Value = PartType.Core;
            }
        }
    }

}
