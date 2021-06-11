using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Console
{
    public interface IConsoleCommand
    {
        string Command { get; }

        bool Process(string[] commands);
    }

    //[CreateAssetMenu(fileName = "ConsoleCommand", menuName = "Utilities/Dev Console/Console Command", order = 0)]
    public class ConsoleCommand : ScriptableObject, IConsoleCommand
    {
        [SerializeField] string command;
        public string Command => command;

        public virtual bool Process(string[] commands)
        {
            return true;
        }
    }
}