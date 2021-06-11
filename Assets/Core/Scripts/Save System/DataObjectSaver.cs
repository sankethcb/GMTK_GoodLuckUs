using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Core.SaveSystem
{
    [DisallowMultipleComponent]
    public class DataObjectSaver : MonoBehaviour, ISaveable
    {
        [SerializeField] List<ScriptableObject> saveables = new List<ScriptableObject>();

        Dictionary<string, object> _saveData = new Dictionary<string, object>();

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public object CaptureData()
        {
            foreach (ISaveable saveable in saveables)
            {
                _saveData[saveable.GetType().ToString()] = saveable.CaptureData();
            }
            
            return _saveData;
        }

        public void RestoreData(object saveData)
        {
            _saveData = saveData as Dictionary<string, object>;

            foreach (ISaveable saveable in saveables)
            {
                if (_saveData.TryGetValue(saveable.GetType().ToString(), out object data))
                    saveable.RestoreData(data);
            }
        }

        void OnValidate()
        {
            for (int i = 0; i < saveables.Count; i++)
            {
                if (saveables[i] != null && (saveables[i] as ISaveable) == null)
                {
                    Debug.Log("Data File is not a Saveable!");
                    saveables[i] = null;
                }

            }
        }
    }
}
