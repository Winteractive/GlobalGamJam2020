using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : PlayerPart, IMediatorListener
{

    public GameObject chargeDisplay;
    Rigidbody2D rigi;

    bool sleeping;
    bool alowedToCharge;
    bool isCharging;
    Vector2 aimDirection = Vector2.zero;

    [SerializeField] float chargePower;
    [Range(1, 20)] [SerializeField] float chargePowerIncrement = 1;
    public float maxChargePower;
    public float minChargePower;

    public float powerDamageThresholdWhenHittingFloor = 15;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        rigi = GetComponent<Rigidbody2D>();
        chargePower = minChargePower;
        GlobalMediator.AddListener(this);
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if (data is PlayerInputData inputMessage)
        {
            if (inputMessage.id != playerNumber || sleeping) return;

            if (inputMessage.key_charge)
            {
                if (alowedToCharge)
                    Charging(inputMessage.axis);
            }
            else
            {
                if (isCharging)
                {
                    ReleaseCharge(inputMessage.axis);
                }

            }
        }
        if (events.HasFlag(GameEvents.PLAYER_GROUND_CHECK))
        {
            if (data is GroundCheckData groundCheckData)
            {
                if (playerNumber == groundCheckData.id)
                {
                    alowedToCharge = groundCheckData.isGrounded;

                    if (groundCheckData.isGrounded)
                    {
                        if (chargePower > powerDamageThresholdWhenHittingFloor)
                        {
                            GlobalMediator.SendMessage(GameEvents.PLAYER_TAKE_DAMAGE, new PlayerData
                            {
                                id = playerNumber,
                            });

                        }
                        Reset();
                    }
                        
                }
            }
        }

        if (events.HasFlag(GameEvents.PLAYER_IS_MOUNTING))
        {
            if (data is PlayerTriggerBoxData isMounting)
            {
                if (playerNumber == isMounting.id)
                {
                    alowedToCharge = isMounting.enterExit;
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_GOT_MOUNTED))
        {
            if (data is PlayerData isMounted)
            {
                if (playerNumber == isMounted.id)
                {
                    alowedToCharge = true;
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_SLEEP))
        {
            if (data is PlayerData isSleeping)
            {
                if (playerNumber == isSleeping.id)
                {
                    sleeping = true;
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_REPAIRED))
        {
            if (data is PlayerData isSleeping)
            {
                if (playerNumber == isSleeping.id)
                {
                    sleeping = false;
                }
            }
        }

    }
    public void Charging(Vector2 inputDirection)
    {
        if (!isCharging)
        {
            isCharging = true;
            chargeDisplay.SetActive(true);
            GlobalMediator.SendMessage(GameEvents.PLAYER_CHARGE_START, new PlayerData
            {
                id = playerNumber,
            });
        }

        chargePower += chargePowerIncrement * Time.deltaTime;
        if (chargePower > maxChargePower)
        {
            chargePower = maxChargePower;
        }
        chargeDisplay.transform.localScale = new Vector3(0.1f * chargePower, 1, 1);

        if (aimDirection != inputDirection && inputDirection != Vector2.zero && Vector2.Dot(Vector2.up, inputDirection.normalized) > 0)
        {

            aimDirection = inputDirection;
            Vector3 direction = inputDirection;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            chargeDisplay.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        }
        // Aiming with inputMessage.leftStick;
        // Locks Aiming to set angles

    }
    public void ReleaseCharge(Vector2 inputDirection)
    {
        isCharging = false;

        if (chargePower < minChargePower)
            chargePower = minChargePower;

        if (Vector2.Dot(Vector2.up, inputDirection.normalized) < 0)
        {
            Reset();
            GlobalMediator.SendMessage(GameEvents.PLAYER_CHARGE_CANCELLED, new PlayerData
            {
                id = playerNumber,
            });
            return;
        }

        rigi.AddForce(aimDirection * chargePower, ForceMode2D.Impulse);

        chargeDisplay.SetActive(false);

        GlobalMediator.SendMessage(GameEvents.PLAYER_CHARGE_RELEASED, new PlayerChargeReleaseData
        {
            id = playerNumber,
            releasedPower = chargePower
        });
    }
    public void Reset()
    {
        aimDirection = Vector2.zero;
        chargePower = minChargePower;
    }

    public struct ChargeMessage
    {
        public int playerNumber;
        public bool charging;
        public float power;
    }

    private void OnDrawGizmos()
    {
        if (isCharging)
        {
            Gizmos.DrawRay(transform.position, aimDirection * chargePower / 10);
        }
    }
}
