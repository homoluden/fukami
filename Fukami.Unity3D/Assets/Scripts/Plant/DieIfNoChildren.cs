using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DieIfNoChildren : MonoBehaviour {
    private List<GameObject> _children = new List<GameObject>();
	private float _age = 0f;

	public float MaxAge = 10f;

	void OnChildAdded(GameObject child){
		_children.Add(child);
	}
	
	void OnChildDestroyed(GameObject child) {
		_children.Remove(child);
	}

	void Start () {
	
	}
	
	void Update () {
        if (_age >= MaxAge)
        {
            _age = 0;
            _children.RemoveAll(ch => ch == null);

            if (_children.Count == 0)
            {
                Destroy(gameObject, 0.5f);
                SendMessageUpwards("OnChildBeforeDestruction", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        _age += Time.deltaTime;

	}

    void OnDestroy()
    {
        gameObject.transform.parent.SendMessage("OnChildDestroyed", this, SendMessageOptions.DontRequireReceiver);
    }

}
