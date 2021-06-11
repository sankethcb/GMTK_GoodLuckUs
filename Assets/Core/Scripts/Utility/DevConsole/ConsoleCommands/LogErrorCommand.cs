using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Console
{
    [CreateAssetMenu(fileName = "ConsoleCommand", menuName = "Utilities/Dev Console/Log Error Command", order = 0)]
    public class LogErrorCommand : ConsoleCommand
    {
        public override bool Process(string[] commands)
        {
            string error = string.Empty;

            foreach (string command in commands)
                error += command + " ";

            Debug.LogError(error);
            return true;
        }
    }
}