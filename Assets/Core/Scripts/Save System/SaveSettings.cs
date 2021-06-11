using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.SaveSystem
{
    //TODO: Implement non-overwriting saves
    //TODO: Customizable save path
    [CreateAssetMenu(fileName = "InputActionData", menuName = "Core/Save System/Save Settings", order = 1)]
    public class SaveSettings : ScriptableObject
    {
        [SerializeField] string savePath = "save.scb";
        public string SavePath => GetSavePath();

        [SerializeField] int saveSlot = 0;
        public void SetSaveSlot(int saveSlot) => this.saveSlot = saveSlot;

        [SerializeField] bool overwriteSave = true;
        public bool SetOverwriteSave(bool overwriteSave) => overwriteSave;

        public string GetSavePath()
        {
            return $"{GameConstants.DATA_PATH}/" + savePath.Insert(savePath.LastIndexOf("."), saveSlot.ToString());
        }

    }
}
