using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFoldOut : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerData playerData;
    [SerializeField] PlayerMovement2D playerMovement;

    [Header("Settings")]
    [SerializeField] float foldoutSpeed = 1;

    void Start()
    {
        
    }


    public void FoldoutLeftStart(InputAction.CallbackContext inputCallback)
    {

    }
    public void FoldoutRightStart(InputAction.CallbackContext inputCallback)
    {

    }

    public void FoldoutUpStart(InputAction.CallbackContext inputCallback)
    {

    }
    public void FoldOutEnd(InputAction.CallbackContext inputCallback)
    {

    }
}
