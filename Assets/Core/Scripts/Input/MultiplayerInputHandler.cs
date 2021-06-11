using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Core.Input
{
    [RequireComponent(typeof(PlayerInputManager))]
    public class MultiplayerInputHandler : MonoBehaviour
    {
        public static event System.Action<PlayerInput> OnPlayerJoin;
        public static event System.Action<PlayerInput> OnPlayerLeft;

        [SerializeField] PlayerInputManager playerInputManager;


        // Start is called before the first frame update
        void Awake()
        {

            if (!playerInputManager) playerInputManager = GetComponent<PlayerInputManager>();
            playerInputManager.onPlayerJoined += PlayerJoined;
            playerInputManager.onPlayerLeft += PlayerLeft;

        }

        // Update is called once per frame
        void OnDestroy()
        {
            playerInputManager.onPlayerJoined -= PlayerJoined;
            playerInputManager.onPlayerLeft -= PlayerLeft;

            OnPlayerJoin = null;
            OnPlayerLeft = null;

        }

        void PlayerJoined(PlayerInput playerInput) => OnPlayerJoin?.Invoke(playerInput);
        void PlayerLeft(PlayerInput playerInput) => OnPlayerLeft?.Invoke(playerInput);

        public void JoinSplitKeyBoardPlayer(string controlScheme = "KBSplit")
        {
            playerInputManager.JoinPlayer(playerInputManager.playerCount, 0, controlScheme, Keyboard.current);
        }
    }
}
