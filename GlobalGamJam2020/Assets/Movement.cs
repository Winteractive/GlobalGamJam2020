using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : PlayerPart, IMediatorListener
{
    private Vector2 movementDirection;
    public float speedMultiplier = 10;
    private Rigidbody2D rb;


    public override void CustomStart()
    {
        rb = GetComponent<Rigidbody2D>();
        GlobalMediator.AddListener(this);
    }

    private void Move(Vector2 inputDirection)
    {
        if (inputDirection.y > 0)
        {
            Hover();
        }
        movementDirection = new Vector2(inputDirection.x, 0) * Time.deltaTime * speedMultiplier;
        //rb.velocity = new Vector2(movementDirection.x, rb.velocity.y);
        rb.AddForce(movementDirection);
        Debug.Log("Moving!");

    }

    //Vertical movement to allow airborne movement testing
    private void Hover()
    {
        rb.AddForce(new Vector2(0, 3000f * Time.deltaTime));
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (events.HasFlag(GameEvents.PLAYER_INPUT))
        {
            if (data is PlayerInput.InputMessage inputData)
            {
                if (inputData.playerNumber == playerNumber)
                {
                    if (inputData.leftStick != Vector2.zero)
                    {
                        Move(inputData.leftStick);
                    }
                    if (true)
                    {

                    }
                }
            }
        }
    }

}
