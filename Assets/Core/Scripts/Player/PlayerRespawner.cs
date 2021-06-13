using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawner : MonoBehaviour
{
    public PlayerMovement2D Player;

    Vector2 spawnPosition;
    void Start()
    {
        Player = GameObject.FindObjectOfType<PlayerMovement2D>();
        spawnPosition = Player.transform.position;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == Player.gameObject)
        {
            Player.transform.position = spawnPosition;
            Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
