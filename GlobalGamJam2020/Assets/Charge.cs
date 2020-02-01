using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : PlayerPart, IMediatorListener
{
    Rigidbody2D rigi;
    bool isCharging;
    Vector2 aimDirection = Vector2.up;

    [SerializeField] float chargePower;
    [SerializeField] float chargePowerIncrement;
    public float maxChargePower;
    public float minChargePower;

    public override void CustomStart()
    {
        rigi = GetComponent<Rigidbody2D>();
        chargePower = minChargePower;
        GlobalMediator.AddListener(this);
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if(data is PlayerInput.InputMessage inputMessage)
        {
            if(inputMessage.charge)
            {
                Charging(inputMessage.leftStick);
            }
            else
            {
                if(isCharging)
                {
                    ReleaseCharge();
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

        if(aimDirection != inputDirection && inputDirection != Vector2.zero)
            aimDirection = inputDirection;
        // Aiming with inputMessage.leftStick;
        // Locks Aiming to set angles
        GlobalMediator.SendMessage(GameEvents.PLAYER_CHARGING, new ChargeMessage
        {
            playerNumber = playerNumber,
            charging = isCharging
        });
    }
    public void ReleaseCharge()
    {
        isCharging = false;
        if (chargePower < minChargePower)
            chargePower = minChargePower;

        rigi.AddForce(aimDirection * chargePower, ForceMode2D.Impulse);

        chargePower = minChargePower;
        GlobalMediator.SendMessage(GameEvents.PLAYER_RELEASED_CHARGE, new ChargeMessage
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
        if(isCharging)
        {
            Gizmos.DrawRay(transform.position, aimDirection);
        }
    }
}
