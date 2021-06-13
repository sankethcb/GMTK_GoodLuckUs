using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLedgeCheck2D : MonoBehaviour
{

    [Header("References")]
    [SerializeField] Transform wallCheck;
    [SerializeField] Transform ledgeCheck;

    [Header("Settings")]
    [SerializeField] float ledgeRaycastDist = 1.0f;
    [SerializeField] LayerMask levelMask;


    public Vector2 CornerPosition => m_cornerPosition;


    RaycastHit2D m_raycastLedge;
    RaycastHit2D m_raycastWall;

    Vector2 m_cornerPosition = Vector2.zero;

    //UNOPTIMIZED
    public bool CheckLedge()
    {
        m_raycastWall = Physics2D.Raycast(wallCheck.position, Vector2.right, ledgeRaycastDist, levelMask);

        if (m_raycastWall.collider)
        {
            m_raycastLedge = Physics2D.Raycast(ledgeCheck.position, Vector2.right, ledgeRaycastDist, levelMask);

            if (!m_raycastLedge.collider)
            {
                CalculateLedgeCorner(Vector3.right);
                return true;
            }
        }

        m_raycastWall = Physics2D.Raycast(wallCheck.position, Vector2.left, ledgeRaycastDist, levelMask);

        if (m_raycastWall.collider)
        {
            m_raycastLedge = Physics2D.Raycast(ledgeCheck.position, Vector2.left, ledgeRaycastDist, levelMask);

            if (!m_raycastLedge.collider)
            {
                CalculateLedgeCorner(Vector3.left);
                return true;
            }
        }

        return false;
    }

    //UNOPTIMIZED
    void CalculateLedgeCorner(Vector3 direction)
    {
        float xDist = m_raycastWall.distance;

        m_raycastWall = Physics2D.Raycast(ledgeCheck.position + xDist * direction, Vector2.down, ledgeCheck.position.y - wallCheck.position.y, levelMask);
        float yDist = m_raycastWall.distance;

        m_cornerPosition.x = wallCheck.position.x + xDist * direction.x;
        m_cornerPosition.y = ledgeCheck.position.y - yDist;
    }
}
