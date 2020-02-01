using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : PlayerPart, IMediatorListener
{
    private Vector2 movementDirection;
    public float speedMultiplier = 10;
    public float brakeValue = 100;
    public float speedLimit = 100;
    [SerializeField]private bool isGrounded = false;
    private bool isCharging = false;
    private bool isBroken = false;
    public float hoverForce = 3000;
    private Rigidbody2D rb;


    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        rb = GetComponent<Rigidbody2D>();
        GlobalMediator.AddListener(this);
    }

    private void UpdateMovement(Vector2 inputDirection)
    {
        if ((inputDirection.x == 0 && isGrounded)) // should only apply on ground check
        {
            if (rb.velocity.x < -1)
            {
                rb.velocity += new Vector2(brakeValue * Time.deltaTime, 0);
                //Debug.Log("breaking");
            }
            else if (rb.velocity.x > 1)
            {
                rb.velocity -= new Vector2(brakeValue * Time.deltaTime, 0);
                //Debug.Log("breaking");
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        if (isCharging)
            return;

        if (!isGrounded)
        {
            inputDirection = Vector2.zero;
        }

        //if (inputDirection.x < 0.5f || inputDirection.x < -0.5f) return;


        movementDirection = new Vector2(inputDirection.x, 0) * Time.deltaTime * speedMultiplier;




        rb.velocity += new Vector2(movementDirection.x, 0);
        if (rb.velocity.x > speedLimit)
        {
            rb.velocity = new Vector2(speedLimit, rb.velocity.y);
            //Debug.Log("limiting speed");
        }
        else if (rb.velocity.x < -speedLimit)
        {
            rb.velocity = new Vector2(-speedLimit, rb.velocity.y);
            //Debug.Log("limiting speed");
        }

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
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_GROUND_CHECK))
        {
            if (data is TagCheck.TagCheckMessage tagData)
            {
                if (tagData.playerNumber == playerNumber)
                {
                    isGrounded = tagData.triggerInside;
                }
            }
        }

        if (events.HasFlag(GameEvents.PLAYER_CHARGE_START ) || events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED) )
        {
            if (data is Charge.ChargeMessage chargeState)
            {
                if (chargeState.playerNumber == playerNumber)
                {
                    isCharging = chargeState.charging;
                }

            }
        }
        if (events.HasFlag(GameEvents.PLAYER_BREAK))
        {
            if (data is int breakPlayerNumber)
            {
                if (breakPlayerNumber == playerNumber)
                {
                    isBroken = true;
                }
            }
        }

        if (events.HasFlag(GameEvents.PLAYER_REPAIRED))
        {
            isBroken = false;
        }
    }

}
