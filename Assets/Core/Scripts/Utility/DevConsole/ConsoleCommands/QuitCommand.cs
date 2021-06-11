using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Console
{
    [CreateAssetMenu(fileName = "ConsoleCommand", menuName = "Utilities/Dev Console/Quit Command", order = 0)]
    public class QuitCommand : ConsoleCommand
    {
        public override bool Process(string[] commands)
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
            return true;
        }
    }
}