using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Linq;

namespace Utilities.Console
{
    public class DeveloperConsole : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] bool pauseOnEnable = true;
        [SerializeField] bool enableTimestamp = true;
        [SerializeField] string logPrefix = "";

        [Header("References")]
        [SerializeField] GameObject uiCanvas;
        [SerializeField] TMP_InputField inputField;
        [SerializeField] TMP_Text scrollingText;
        [SerializeField] UnityEngine.UI.Scrollbar scrollbar;

        [Space(10)]
        [SerializeField] ConsoleCommand[] consoleCommands;

        static DeveloperConsole developerConsole;

        ConsoleProcesser Console;
        System.DateTime _dateTime;
        float _timescale;


        void Awake()
        {
            if (developerConsole != null)
                Destroy(gameObject);

            developerConsole = this;

            Console = new ConsoleProcesser("/", consoleCommands);

            Application.logMessageReceived += ProcessError;

            DontDestroyOnLoad(gameObject);
        }

        void OnDestroy()
        {
            if (developerConsole == this)
            {
                Application.logMessageReceived -= ProcessError;
                developerConsole = null;
            }
        }

        public void ToggleConsole(InputAction.CallbackContext inputCallback)
        {
            if (!inputCallback.action.triggered) return;

            if (!uiCanvas.activeSelf) EnableConsole();
            else DisableConsole();
        }

        void EnableConsole()
        {
            if (pauseOnEnable)
            {
                _timescale = Time.timeScale;
                Time.timeScale = 0;
            }

            uiCanvas.SetActive(true);
            inputField.ActivateInputField();

            inputField.onSubmit.AddListener(ProcessCommand);

        }

        void DisableConsole()
        {
            if (pauseOnEnable)
            {
                Time.timeScale = _timescale;
            }

            inputField.text = "";
            scrollbar.value = 0;

            inputField.onSubmit.RemoveListener(ProcessCommand);

            uiCanvas.SetActive(false);
            inputField.DeactivateInputField();
        }

        public void ProcessCommand(string inputCommand)
        {
            scrollbar.value = 0;

            if (string.IsNullOrEmpty(inputCommand))
                return;

            PrintLog(inputCommand);

            inputField.text = string.Empty;

            Console.Process(inputCommand);
            inputField.ActivateInputField();
        }

        string GetTimeStamp()
        {
            _dateTime = System.DateTime.Now;

            return "[<#FFFF80>" + _dateTime.Hour.ToString("d2") + ":" + _dateTime.Minute.ToString("d2") + ":" + _dateTime.Second.ToString("d2") + "</color>] ";
        }

        void ProcessError(string condition, string stackTrace, UnityEngine.LogType logType)
        {
            if (logType == UnityEngine.LogType.Log || logType == UnityEngine.LogType.Warning)
                return;

            PrintLog(condition);
        }

        void PrintLog(string output)
        {
            output = logPrefix + output;

            if (enableTimestamp) output = GetTimeStamp() + output;

            scrollingText.text = scrollingText.text + (scrollingText.text.Length != 0 ? "\n" : "") + output;
        }
    }
}
