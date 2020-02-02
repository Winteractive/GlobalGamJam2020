using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : PlayerPart
{
    List<GameObject> objectsInside = new List<GameObject>();
    public LayerMask mask;
    public float rayDistance = 2;
    public float circleRaduis = 1;
    public bool isGrounded;
    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);

    }

    private void FixedUpdate()
    {
        var hit = Physics2D.CircleCast(transform.position + Vector3.up, circleRaduis, Vector2.down, rayDistance, mask);

        if (hit)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                GlobalMediator.SendMessage(GameEvents.PLAYER_GROUND_CHECK, new GroundCheckData
                {
                    id = playerNumber,
                    isGrounded = true
                });
            }
        }
        else
        {
            if(isGrounded)
            {
                isGrounded = false;
                GlobalMediator.SendMessage(GameEvents.PLAYER_GROUND_CHECK, new GroundCheckData
                {
                    id = playerNumber,
                    isGrounded = false
                });
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;


        Gizmos.DrawRay(transform.position + Vector3.up, Vector3.down * rayDistance);
        Gizmos.DrawWireSphere(transform.position + Vector3.up + (Vector3.down * rayDistance), circleRaduis);

        Gizmos.color = Color.white;
    }
   
}
