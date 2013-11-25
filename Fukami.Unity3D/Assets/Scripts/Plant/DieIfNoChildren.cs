using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DieIfNoChildren : MonoBehaviour {
    private List<GameObject> _children = new List<GameObject>();
    private ushort _childrenCleanupPeriod = 10000;
    private ushort _currentIteration = 0;

	void OnChildAdded(GameObject child){
		_children.Add(child);
	}
	
	void OnChildDestroyed(GameObject child) {
		_children.Remove(child);
	}

	void Start () {
	
	}
	
	void Update () {
        if (_currentIteration >= _childrenCleanupPeriod)
        {
            _currentIteration = 0;
            _children.RemoveAll(ch => ch == null);

            if (_children.Count == 0)
            {
                Destroy(gameObject, 0.5f);
                SendMessageUpwards("OnChildBeforeDestruction", this, SendMessageOptions.DontRequireReceiver);
            }
        }
        _currentIteration++;

	}

    void OnDestroy()
    {
        gameObject.transform.parent.SendMessage("OnChildDestroyed", this, SendMessageOptions.DontRequireReceiver);
    }

}
