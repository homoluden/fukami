using UnityEngine;
using System.Collections;
using Fukami.Helpers;
using Fukami.Genes;
using Fukami.Entities;

public class DnaProcessor : MonoBehaviour
{
    #region Fields

    private string _dnaStr;
    private float _age;
    private int _generation;

    #endregion


    #region Properties

    public GameObject BonePrefab;
    public GameObject NodePrefab;
    public GameObject JointPrefab;

    public string DnaString
    {
        get { return _dnaStr; }
        set
        {
            _dnaStr = value;
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
            var geneProcessor = gameObject.AddComponent<GeneProcessor>();
            geneProcessor.GeneString = gene;
        }
    }

    void OnDnaAgeRequested(Wrap<float> age)
    {
        if (age.IsSet)
        {
            return;
        }

        age.Value = _age;
        age.ValueSource = "DnaProcessor";
    }

    void OnApplyGene(GeneData gene)
    {
        switch (gene.GeneType)
        {
            case GeneType.Node:
                AddNode(gene);
                break;
            case GeneType.Bone:
                AddBone(gene);
                break;
            case GeneType.Joint:
                AddJoint(gene);
                break;
            case GeneType.Stats:
                AddStats(gene);
                break;
            default:
                break;
        }
    }

    private void AddStats(GeneData gene)
    {
        throw new System.NotImplementedException();
    }

    private void AddJoint(GeneData gene)
    {
        float x, y;
        gene.FloatModifiers.TryGetValue("X", out x);
        gene.FloatModifiers.TryGetValue("Y", out y);

        var slot = new ChildSlot { X = x, Y = y };

        var newBody = (GameObject)Instantiate(BonePrefab,
                  gameObject.transform.position + new Vector3(slot.X, slot.Y),
                  gameObject.transform.rotation);
        newBody.transform.parent = gameObject.transform;
        newBody.SetActive(true);

        var spring = newBody.AddComponent<SpringJoint2D>();
        spring.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        spring.distance = 0.1f;
        spring.dampingRatio = 0.01f;
        spring.frequency = 10.0f;

		gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.DontRequireReceiver);
    }

    private void AddBone(GeneData gene)
    {
        float angle;
        gene.FloatModifiers.TryGetValue("Angle", out angle);

        var newBody = (GameObject)Instantiate(BonePrefab,
                  gameObject.transform.position,
                  gameObject.transform.rotation *
		          Quaternion.AngleAxis(angle, Vector3.forward));
        newBody.transform.parent = gameObject.transform;
        newBody.SetActive(true);

        //var slide = newBody.AddComponent<SliderJoint2D>();
		var slide = newBody.AddComponent<HingeJoint2D>();
        slide.connectedBody = gameObject.GetComponent<Rigidbody2D>();
		slide.connectedAnchor = new Vector2(Random.Range(-0.15f, 0.15f), Random.Range(-0.15f, 0.15f));
        //slide.limits = new JointTranslationLimits2D { min = -0.1f, max = 0.1f };
		slide.limits = new JointAngleLimits2D { min = -1.0f, max = 1.0f };
        slide.useLimits = true;

        gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.RequireReceiver);
    }

    private void AddNode(GeneData gene)
    {
        float angle;
        gene.FloatModifiers.TryGetValue("Angle", out angle);

        float x, y;
        gene.FloatModifiers.TryGetValue("X", out x);
        gene.FloatModifiers.TryGetValue("Y", out y);

        var slot = new ChildSlot { X = x, Y = y, Angle = angle };

        var newBody = (GameObject)Instantiate(NodePrefab,
                                                gameObject.transform.position ,
                                                Quaternion.FromToRotation(Vector3.right, Vector3.up) *
                                                //Quaternion.AngleAxis(Random.Range(-slot.Angle, slot.Angle), Vector3.forward));
		                                      Quaternion.AngleAxis(slot.Angle, Vector3.forward));

        newBody.transform.parent = gameObject.transform;
        newBody.SetActive(true);

        var hinge = newBody.AddComponent<HingeJoint2D>();
        hinge.connectedBody = gameObject.GetComponent<Rigidbody2D>();
        hinge.connectedAnchor = new Vector2(slot.X, slot.Y);
        hinge.limits = new JointAngleLimits2D { min = -1.0f, max = 1.0f };
        hinge.motor = new JointMotor2D { motorSpeed = -1000.0f, maxMotorTorque = 10000.0f };
        hinge.useLimits = true;
        hinge.useMotor = false;

        newBody.AddComponent<HingeSmoothPos>();

		gameObject.SendMessage("OnChildAdded", newBody, SendMessageOptions.DontRequireReceiver);
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

        dna.Value = _dnaStr;
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
