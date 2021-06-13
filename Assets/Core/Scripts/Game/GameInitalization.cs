using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AddressableAssets;

namespace Core
{
    [DefaultExecutionOrder(-9999)]
    class GameInitialization
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitalizeGameLoading()
        {

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("Starting Game Initalization");
#endif
            //Addressables.InstantiateAsync(GameConstants.GAME_LOADER_PATH).Completed += OnLoadDone;

        }

        

    }
}
