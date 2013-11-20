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
            gameObject.transform.localScale = new Vector3( EndScaleX, EndScaleY, 1.0f );
            gameObject.GetComponent<Rigidbody2D>().mass = _originalMass;
        }

        var t = _age / GrowTime;
        var x = Mathf.Lerp(StartScaleX, EndScaleX, t);
        var y = Mathf.Lerp(StartScaleY, EndScaleY, t);
        var m = Mathf.Lerp(StartMassScale, EndMassScale, t);

        gameObject.transform.localScale = new Vector3(x, y, 1.0f);
        gameObject.GetComponent<Rigidbody2D>().mass = m;
	}
}
