using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SaveSystem;

namespace Utilities.Console
{
    [CreateAssetMenu(fileName = "ConsoleCommand", menuName = "Utilities/Dev Console/Save Command", order = 0)]
    public class SaveCommand : ConsoleCommand
    {
        [SerializeField] SaveHandler saveHandler;
        public override bool Process(string[] commands)
        {
            if (commands.Length == 0)
                return true;

            if (commands[0].Equals("save", System.StringComparison.OrdinalIgnoreCase))
                saveHandler.SaveData();
            if (commands[0].Equals("load", System.StringComparison.OrdinalIgnoreCase))
                saveHandler.LoadData();

            return true;
        }
    }
}
