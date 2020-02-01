using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : PlayerPart
{

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground"))
        {
            GlobalMediator.SendMessage(GameEvents.PLAYER_GROUND_CHECK, new GroundCheckData
            {
                id = playerNumber,
                isGrounded = true
            });
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            GlobalMediator.SendMessage(GameEvents.PLAYER_GROUND_CHECK, new GroundCheckData
            {
                id = playerNumber,
                isGrounded = false
            });
        }
    }
}
