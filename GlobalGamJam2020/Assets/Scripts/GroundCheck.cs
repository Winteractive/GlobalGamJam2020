using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : PlayerPart
{
    List<GameObject> objectsInside = new List<GameObject>();
    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            objectsInside.Add(collision.gameObject);
            GlobalMediator.SendMessage(GameEvents.PLAYER_GROUND_CHECK, new GroundCheckData
            {
                id = playerNumber,
                isGrounded = true
            });
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
        {
            objectsInside.Remove(collision.gameObject);


            if(objectsInside.Count == 0)
            {
                Debug.Log("In Air");
                GlobalMediator.SendMessage(GameEvents.PLAYER_GROUND_CHECK, new GroundCheckData
                {
                    id = playerNumber,
                    isGrounded = false
                });
            }
            
        }
    }
}
