using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.SceneManagement;

namespace Utilities.Console
{
    [CreateAssetMenu(fileName = "ConsoleCommand", menuName = "Utilities/Dev Console/Scene Command", order = 0)]
    public class SceneCommand : ConsoleCommand
    {
        public override bool Process(string[] commands)
        {
            if (commands.Length == 0 || commands.Length < 2)
                return true;

            SCENES scene = (SCENES)0;

            int count = 0;
            foreach (string name in System.Enum.GetNames(typeof(SCENES)))
            {
                if (commands[1].Equals(name, System.StringComparison.OrdinalIgnoreCase))
                {
                    scene = (SCENES)count;
                    break;
                }

                count++;
            }
            if (count == System.Enum.GetValues(typeof(SCENES)).Length)
                return true;


            if (commands[0].Equals("Load", System.StringComparison.OrdinalIgnoreCase))
            {
                scene.LoadAsync(UnityEngine.SceneManagement.LoadSceneMode.Single);
            }
        


            return true;
        }
    }
}