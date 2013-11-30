using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Helpers
{
    public sealed class PrefabsManager
    {
        private Dictionary<string, GameObject> _prefabs = new Dictionary<string, GameObject>();
        

        public GameObject LoadPrefab(string name)
        {
			if (_prefabs.ContainsKey(name)) {
				return _prefabs[name];
			}

			lock (SyncRoot) {
				var prefab = (GameObject)Resources.Load(string.Format("Prefabs/{0}", name));
				if (prefab != null)
				{
					_prefabs.Add(name, prefab);
				}
				
				return prefab;
			}            
        }

        #region Singleton implementation

        private static volatile PrefabsManager _instance;
        private static readonly object SyncRoot = new System.Object();

        private PrefabsManager()
        {
            
        }

        public static PrefabsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                            _instance = new PrefabsManager();
                    }
                }

                return _instance;
            }
        }
        #endregion


    }
}
