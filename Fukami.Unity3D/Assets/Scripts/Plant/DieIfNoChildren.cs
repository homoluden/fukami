using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DieIfNoChildren : MonoBehaviour {
    private List<GameObject> _children = new List<GameObject>();
    private ushort _childrenCleanupPeriod = 100;
    private ushort _currentIteration = 0;

	void Start () {
	
	}
	
	void Update () {
        if (_currentIteration >= _childrenCleanupPeriod)
        {
            _currentIteration = 0;
            _children.RemoveAll(ch => ch == null);

            if (_children.Count == 0)
            {
                Destroy(this, 0.5f);
                SendMessageUpwards("OnChildBeforeDestruction", this);
            }
        }
        _currentIteration++;

	}

    void OnDestroy()
    {
        SendMessageUpwards("OnChildDestroyed", this, SendMessageOptions.RequireReceiver);
    }

    void OnChildAdded(GameObject child) {
        _children.Add(child);
    }
}
