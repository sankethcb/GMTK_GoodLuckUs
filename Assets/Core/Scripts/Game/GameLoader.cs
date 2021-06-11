using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    [DefaultExecutionOrder(-9999)]
    public class GameLoader : MonoBehaviour
    {
        [SerializeField] GameSystems gameSystems;
        [SerializeField] string objectName = "Core Game Systems";

        void Awake()
        {
            LoadSystems();

            DontDestroyOnLoad(gameObject);

            gameObject.name = objectName;
        }

        public void LoadSystems()
        {
            foreach (GameObject system in gameSystems.GetSystems)
            {
                Instantiate(system);
            }

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("Game Systems Loaded");
#endif

            Destroy(gameObject);
        }
    }
}
