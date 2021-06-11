using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(PlayerInput))]
    public class InputHandler : MonoBehaviour
    {
        [SerializeField] PlayerInput playerInput;
        [SerializeField] InputActionDataSet inputActionDataSet;

        public static int PlayerCount { get; private set; }

        void Awake()
        {
            if (!playerInput) playerInput = GetComponent<PlayerInput>();

            playerInput.onActionTriggered += ProcessInputEvent;
            playerInput.onControlsChanged += CurrentDeviceSwitched;

            InputSystem.onDeviceChange += DeviceChanged;

            PlayerCount++;
            inputActionDataSet.RegisterPlayerInput(playerInput.actions, playerInput.playerIndex);


            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            PlayerCount--;
            playerInput.onActionTriggered -= ProcessInputEvent;
        }

        void OnEnable()
        {
            playerInput.ActivateInput();
        }

        void OnDisable()
        {
            playerInput.DeactivateInput();
        }


        void ProcessInputEvent(InputAction.CallbackContext inputCallback)
        {

        }

        void DeviceChanged(InputDevice device, InputDeviceChange deviceChange)
        {
#if UNITY_EDITOR || DEV_BUILD
            (device.name + " " + deviceChange.ToString()).Log();
#endif

            switch (deviceChange)
            {
                case InputDeviceChange.Added:
                    break;
                case InputDeviceChange.Removed:
                    break;
                case InputDeviceChange.Reconnected:
                    break;
                case InputDeviceChange.Disconnected:
                    break;
            }
        }

        void CurrentDeviceSwitched(PlayerInput playerInput)
        {

#if UNITY_EDITOR || DEV_BUILD
            Debug.Log("Player " + (playerInput.playerIndex) + " controls switched to" + playerInput.currentControlScheme.ToString());
#endif
        }


        public static void SaveBindingsToPrefs(InputActionAsset actionsAsset, string key)
        {
            PlayerPrefs.SetString(key, actionsAsset.ToJson());
        }

        public static void LoadBindingsFromPrefs(InputActionAsset actionsAsset, string key)
        {
            actionsAsset.LoadFromJson(PlayerPrefs.GetString(key));
        }
    }
}