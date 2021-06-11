using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Core.Input
{
    public class InputEventSystem : MonoBehaviour
    {
        [SerializeField] int playerIndex = 0;

        [CollectionLabel("inputActionData")]
        [SerializeField] InputReciever[] inputActions;

        public int PlayerIndex => playerIndex;
        public void SetPlayerIndex(int playerIndex) => this.playerIndex = playerIndex;

        void OnEnable()
        {
            foreach (InputReciever inputAction in inputActions)
                if (inputAction != null) inputAction.RegisterInput(playerIndex);
        }

        void OnDisable()
        {
            foreach (InputReciever inputAction in inputActions)
                if (inputAction != null) inputAction.DeregisterInput();
        }

    }
}
