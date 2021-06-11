using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity2D : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Rigidbody2D playerBody;

    [Header("Settings")]
    [SerializeField] float _gravity = 9.8f;

    
    void FixedUpdate()
    {
        
    }
}
