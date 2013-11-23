using UnityEngine;
using System.Collections;

public class SmoothGrow : MonoBehaviour
{

    #region Fields
    private float _originalMass;
    private float _age;
    private bool _growingCompleted;
    
    #endregion

    #region Properties
    public float StartMassScale = 0.1f;
    public float EndMassScale = 1.0f;
    public float StartScaleX = 0.1f;
    public float StartScaleY = 0.1f;
    public float EndScaleX = 1.0f;
    public float EndScaleY = 1.0f;
    public float GrowTime = 3.0f;
    private GameObject _child;
    private Vector2 _hingeAnchor;
    #endregion

    // Use this for initialization
	void Start () {
        _originalMass = gameObject.GetComponent<Rigidbody2D>().mass;
        _age = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
        if (_growingCompleted)
        {
            return;
        }

        _age += Time.deltaTime;
        if (_age >= GrowTime)
        {
            _growingCompleted = true;

            if (_child != null)
            {
                _child.GetComponent<HingeJoint2D>().connectedAnchor = _hingeAnchor;
            }
            
            gameObject.transform.localScale = new Vector3( EndScaleX, EndScaleY, 1.0f );
            gameObject.GetComponent<Rigidbody2D>().mass = _originalMass;
        }

        var t = _age / GrowTime;
        var x = Mathf.Lerp(StartScaleX, EndScaleX, t);
        var y = Mathf.Lerp(StartScaleY, EndScaleY, t);
        var m = Mathf.Lerp(StartMassScale, EndMassScale, t);

        if (_child != null)
        {
            _child.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(_hingeAnchor.x * t, _hingeAnchor.y * t);
        }        

        gameObject.transform.localScale = new Vector3(x, y, 1.0f);
        gameObject.GetComponent<Rigidbody2D>().mass = m;
	}

    public void AddChild(GameObject child, Vector2 hingeAnchor) {
        _child = child;
        _hingeAnchor = hingeAnchor;
    }
}
