using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    [CreateAssetMenu(fileName = "InputActionDataSet", menuName = "Core/Input/Input Action Set", order = 0)]
    public class InputActionDataSet : ScriptableObject
    {
        [SerializeField] List<InputActionData> m_inputActionDataList;

        public void RegisterPlayerInput(InputActionAsset playerAsset, int playerIndex = 0)
        {
            foreach (InputActionData inputActionData in m_inputActionDataList)
            {
                inputActionData.AddInputAction(playerAsset.FindAction(inputActionData.InputAction().name), playerIndex);
            }

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("Player " + playerIndex + " input registered!");
#endif
        }
    }
}
