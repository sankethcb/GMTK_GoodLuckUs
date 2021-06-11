using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


namespace Core.SaveSystem
{
    [CreateAssetMenu(fileName = "SaveHandler", menuName = "Core/Save System/Save Handler", order = 0)]
    public class SaveHandler : ScriptableObject
    {
        [SerializeField] SaveSettings SaveSettings;

        static HashSet<Saveable> _saveables = new HashSet<Saveable>();

        Dictionary<string, object> _savedData;
        byte[] _cache;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initalize()
        {
            Application.quitting += Release;

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("SaveHandler Set");
#endif
        }

        static void Release()
        {
            _saveables.Clear();

            Application.quitting -= Release;

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("SaveHandler Released");
#endif
        }

        public static bool AddSaveable(Saveable saveable)
        {
            if (_saveables.Contains(saveable))
                return false;

            _saveables.Add(saveable);
            return true;
        }

        public static bool RemoveSaveable(Saveable saveable)
        {
            if (_saveables.Contains(saveable))
            {
                _saveables.Remove(saveable);
                return true;
            }

            return false;
        }

        [ContextMenu("Save Data")]
        public void SaveData()
        {
            //_cache = IOUtility.LoadFile(SaveSettings.SavePath);

            _savedData = IOUtility.LoadFile<Dictionary<string, object>>(SaveSettings.SavePath);

            if (_savedData == null)
                _savedData = new Dictionary<string, object>();
            //else
                //_savedData = SaveSerializer.Deserialzie(_cache) as Dictionary<string, object>;

            foreach (Saveable saveable in _saveables)
            {
                _savedData[saveable.ID] = saveable.CaptureData();
            }

            IOUtility.SaveFile(SaveSerializer.Serialize(_savedData), SaveSettings.SavePath);

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("Data of " + _savedData.Count + " objects saved.");
#endif
        }

        [ContextMenu("Load Data")]
        public void LoadData()
        {
            //_cache = IOUtility.LoadFile(SaveSettings.SavePath);

            _savedData = IOUtility.LoadFile<Dictionary<string, object>>(SaveSettings.SavePath);

            if (_savedData == null)
                _savedData = new Dictionary<string, object>();
            //else
                //_savedData = SaveSerializer.Deserialzie(_cache) as Dictionary<string, object>;

            foreach (Saveable saveable in _saveables)
            {
                if (_savedData.TryGetValue(saveable.ID, out object data))
                {
                    saveable.RestoreData(data);
                }
            }

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("Data of " + _savedData.Count + " objects loaded.");
#endif
        }
        
    }
}
