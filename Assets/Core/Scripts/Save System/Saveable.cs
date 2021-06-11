using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Core.SaveSystem
{
    public interface ISaveable
    {
        object CaptureData();
        void RestoreData(object data);
    }

    [DisallowMultipleComponent]
    public class Saveable : MonoBehaviour
    {
        [ReadOnly] [SerializeField] string _id = string.Empty;

        byte[] _guid = null;

        public string ID => _id;
        public byte[] GUID => _guid;

        Dictionary<string, object> _saveData = new Dictionary<string, object>();

        [ContextMenu("Generate Unique ID")]
        void GenerateID()
        {
            _guid = System.Guid.NewGuid().ToByteArray();
            _id = new System.Guid(_guid).ToString();
        }

        void Awake()
        {
            SaveHandler.AddSaveable(this);
        }

        void OnDestroy()
        {
            SaveHandler.RemoveSaveable(this);
        }

        public object CaptureData()
        {
            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                _saveData[saveable.GetType().ToString()] = saveable.CaptureData();
            }

            return _saveData;
        }

        public void RestoreData(object saveData)
        {
            _saveData = saveData as Dictionary<string, object>;

            foreach (ISaveable saveable in GetComponents<ISaveable>())
            {
                if (_saveData.TryGetValue(saveable.GetType().ToString(), out object data))
                    saveable.RestoreData(data);
            }
        }
    }
}
