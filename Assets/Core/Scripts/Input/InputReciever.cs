
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Core.Input
{
    [System.Serializable]
    public class InputReciever
    {
        public event System.Action<InputAction.CallbackContext> OnStarted;
        public event System.Action<InputAction.CallbackContext> OnPerformed;
        public event System.Action<InputAction.CallbackContext> OnCancelled;

        [SerializeField] InputActionData inputActionData;
        
        [SerializeField] MethodPicker<InputAction.CallbackContext>[] onStarted;
        [SerializeField] MethodPicker<InputAction.CallbackContext>[] onPerformed;
        [SerializeField] MethodPicker<InputAction.CallbackContext>[] onCancelled;

        protected InputAction m_inputAction = null;
        public InputAction Action => m_inputAction;

        bool m_initalized = false;

        public bool RegisterInput(int playerIndex = 0)
        {
            if (!inputActionData || playerIndex > (InputHandler.PlayerCount - 1))
                return false;

            m_inputAction = inputActionData.InputAction(playerIndex);

            
            AssignListeners();

            return true;
        }

        public void DeregisterInput()
        {
            if (OnStarted != null)
                m_inputAction.started -= OnStarted;

            if (OnPerformed != null)
                m_inputAction.performed -= OnPerformed;

            if (OnCancelled != null)
                m_inputAction.canceled -= OnCancelled;
        }

        void AssignListeners()
        {
            if (!m_initalized) InitializeListeners();

            if (OnStarted != null)
                m_inputAction.started += OnStarted;

            if (OnPerformed != null)
                m_inputAction.performed += OnPerformed;

            if (OnCancelled != null)
                m_inputAction.canceled += OnCancelled;
        }

        void InitializeListeners()
        {
            OnStarted = OnPerformed = OnCancelled = null;

        
            foreach (MethodPicker<InputAction.CallbackContext> method in onStarted)
            {
                if (method.Behaviour == null) continue;
                OnStarted += method.BuildMethod();
            }

            foreach (MethodPicker<InputAction.CallbackContext> method in onPerformed)
            {
                if (method.Behaviour == null) continue;
                OnPerformed += method.BuildMethod();
            }

            foreach (MethodPicker<InputAction.CallbackContext> method in onCancelled)
            {
                if (method.Behaviour == null) continue;
                OnCancelled += method.BuildMethod();
            }

            //InitializeListeners(ref OnStarted, onStarted);
            //InitializeListeners(ref OnPerformed, onPerformed);
            //InitializeListeners(ref OnCancelled, onCancelled);

            m_initalized = true;
        }

        void InitializeListeners(ref System.Action<InputAction.CallbackContext> action, MethodPicker<InputAction.CallbackContext>[] listeners)
        {
            foreach (MethodPicker<InputAction.CallbackContext> method in listeners)
            {
                if (method.Behaviour == null) continue;
                Debug.Log(method);
                action += method.BuildMethod();
            }
        }
    }



}
