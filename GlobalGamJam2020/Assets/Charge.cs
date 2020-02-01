using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : PlayerPart, IMediatorListener
{
    Rigidbody2D rigi;
    bool alowedToCharge;
    bool isCharging;
    Vector2 aimDirection = Vector2.zero;
    


    [SerializeField] float chargePower;
    [Range(1, 20)] [SerializeField] float chargePowerIncrement = 1;
    public float maxChargePower;
    public float minChargePower;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        rigi = GetComponent<Rigidbody2D>();
        chargePower = minChargePower;
        GlobalMediator.AddListener(this);
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (data is PlayerInput.InputMessage inputMessage)
        {
            if (inputMessage.playerNumber != playerNumber) return;

            if (inputMessage.charge)
            {
                if (alowedToCharge)
                    Charging(inputMessage.leftStick);
            }
            else
            {
                if (isCharging)
                {
                    ReleaseCharge(inputMessage.leftStick);
                }

            }
        }

        if(events.HasFlag(GameEvents.PLAYER_GROUND_CHECK))
        {
            if(data is TagCheck.TagCheckMessage tagMessage)
            {
                if(playerNumber == tagMessage.playerNumber)
                {
                    alowedToCharge = tagMessage.triggerInside;
                }
            }
        }


    }
    public void Charging(Vector2 inputDirection)
    {
        //Charging
        isCharging = true;
        chargePower += chargePowerIncrement * Time.deltaTime;
        if (chargePower > maxChargePower)
        {
            chargePower = maxChargePower;
        }

        if (aimDirection != inputDirection && inputDirection != Vector2.zero && Vector2.Dot(Vector2.up, inputDirection.normalized) > 0)
        {
            aimDirection = inputDirection;
        }

        // Aiming with inputMessage.leftStick;
        // Locks Aiming to set angles
        SendChargeMessage(GameEvents.PLAYER_CHARGING);
    }
    public void ReleaseCharge(Vector2 inputDirection)
    {
        isCharging = false;

        if (chargePower < minChargePower)
            chargePower = minChargePower;

        if (Vector2.Dot(Vector2.up, inputDirection.normalized) < 0)
        {
            Reset();
            SendChargeMessage(GameEvents.PLAYER_CANCELED_CHARGE);
            return;
        }

        rigi.AddForce(aimDirection * chargePower, ForceMode2D.Impulse);

        Reset();
        SendChargeMessage(GameEvents.PLAYER_CHARGING);
    }
    public void Reset()
    {
        aimDirection = Vector2.zero;
        chargePower = minChargePower;
    }
    public void SendChargeMessage(GameEvents gameEvents)
    {
        GlobalMediator.SendMessage(gameEvents, new ChargeMessage
        {
            playerNumber = playerNumber,
            charging = isCharging
        });

    }
    public struct ChargeMessage
    {
        public int playerNumber;
        public bool charging;
    }

    private void OnDrawGizmos()
    {
        if (isCharging)
        {
            Gizmos.DrawRay(transform.position, aimDirection * chargePower/10);
        }
    }
}
