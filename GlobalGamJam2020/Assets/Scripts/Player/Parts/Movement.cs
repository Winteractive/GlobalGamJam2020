using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : PlayerPart, IMediatorListener
{
    private Vector2 movementDirection;
    public float speedMultiplier = 10;
    public float breakValue = 100;
    public float speedLimit = 100;

    public float hoverForce = 3000;
    private Rigidbody2D rb;


    public override void CustomStart()
    {
        rb = GetComponent<Rigidbody2D>();
        GlobalMediator.AddListener(this);
    }

    private void UpdateMovement(Vector2 inputDirection)
    {
        if (inputDirection.y > 0)
        {
            Hover();
        }
        movementDirection = new Vector2(inputDirection.x, 0) * Time.deltaTime * speedMultiplier;

        if (inputDirection.x == 0) // should only apply on ground check
        {
            if (rb.velocity.x < -1)
            {
                rb.velocity += new Vector2(breakValue * Time.deltaTime, 0);
                Debug.Log("breaking");
            }
            else if (rb.velocity.x > 1)
            {
                rb.velocity -= new Vector2(breakValue * Time.deltaTime, 0);
                Debug.Log("breaking");
            }

        }

        //rb.velocity = new Vector2(movementDirection.x, rb.velocity.y);
        rb.velocity += new Vector2(movementDirection.x, 0);
        if (rb.velocity.x > speedLimit)
        {
            rb.velocity = new Vector2(speedLimit, rb.velocity.y);
            Debug.Log("limiting speed");
        }
        else if (rb.velocity.x < -speedLimit)
        {
            rb.velocity = new Vector2(-speedLimit, rb.velocity.y);
            Debug.Log("limiting speed");
        }

        //rb.AddForce(movementDirection);
        Debug.Log("Moving!");

    }

    //Vertical movement to allow airborne movement testing
    private void Hover()
    {
        rb.AddForce(new Vector2(0, hoverForce * Time.deltaTime));
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (events.HasFlag(GameEvents.PLAYER_INPUT))
        {
            if (data is PlayerInput.InputMessage inputData)
            {
                if (inputData.playerNumber == playerNumber)
                {
                    UpdateMovement(inputData.leftStick);
                    if (inputData.leftStick != Vector2.zero)
                    {

                    }
                }
            }
        }
    }

}
