using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidePlayerLock : PlayerPart, IMediatorListener
{
    public bool isRiding = false;
    public bool isMounted = false;
    private Transform mountedPlayer;
    public float yOffset = 0.8f;

    bool WallOver;
    public float circleRaduis;
    public float rayDistance;
    public LayerMask mask;

    private void Update()
    {
        if (isRiding && !isMounted)
        {
            if (mountedPlayer)
            {
                transform.position = mountedPlayer.position + new Vector3(0, yOffset, 0);
            }
        }
    }

    private void FixedUpdate()
    {
        if (isMounted)
        {
            var hit = Physics2D.CircleCast(transform.position, circleRaduis, Vector2.up, rayDistance, mask);

            if (hit)
            {
                if (!WallOver)
                {
                    WallOver = true;
                    GlobalMediator.SendMessage(GameEvents.PLAYER_FORCE_DISMOUNT, new PlayerData
                    {
                        id = mountedPlayer.GetComponent<Player>().playerNumber,
                    });

                }
            }
            else
            {
                if (WallOver)
                {
                    WallOver = false;
                    //Send so we can't go to the direction
                }
            }
        }
        else
        {
            WallOver = false;
        }

    }
    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        GlobalMediator.AddListener(this);
    }

    private void MountPlayer(GameObject bottomPlayer)
    {
        if ((GetComponent<Rigidbody2D>().velocity.y > 0) || !bottomPlayer.GetComponent<Movement>().isGrounded)
        {
            return;
        }
        mountedPlayer = bottomPlayer.transform;
        transform.position = mountedPlayer.position + new Vector3(0, yOffset, 0);
        isRiding = true;
        GlobalMediator.SendMessage(GameEvents.PLAYER_GOT_MOUNTED, new PlayerGotMountedData
        {
            id = bottomPlayer.GetComponent<Player>().playerNumber,
            playerMounted = transform
        });

    }

    private void DismountPlayer()
    {
        if (mountedPlayer)
            GlobalMediator.SendMessage(GameEvents.PLAYER_GOT_DISMOUNTED, new PlayerData
            {
                id = mountedPlayer.GetComponent<Player>().playerNumber
            });
        mountedPlayer = null;
        isRiding = false;
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.PLAYER_IS_MOUNTING))
        {
            if (data is PlayerTriggerBoxData tagData)
            {

                if (tagData.id == playerNumber && tagData.enterExit)
                {
                    MountPlayer(tagData.collidingObject);
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_FORCE_DISMOUNT))
        {
            if (data is PlayerData tagData)
            {
                if (tagData.id == playerNumber)
                {
                    DismountPlayer();
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_GOT_MOUNTED))
        {
            if (data is PlayerGotMountedData tagData)
            {
                if (tagData.id == playerNumber)
                {
                    isMounted = true;
                    mountedPlayer = tagData.playerMounted;
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_GOT_DISMOUNTED))
        {
            if (data is PlayerData tagData)
            {
                if (tagData.id == playerNumber)
                {
                    isMounted = false;
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED))
        {
            if (data is PlayerChargeReleaseData chargedata)
            {
                if (chargedata.id == playerNumber)
                {
                    DismountPlayer();
                }
            }
        }

        if(events.HasFlag(GameEvents.PLAYER_RESPAWN))
        {
            if (data is PlayerData tagData)
            {
                if (tagData.id == playerNumber)
                {
                    mountedPlayer = null;
                    isMounted = false;
                    isRiding = false;
                }
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        if(isMounted)
        {
            Gizmos.color = WallOver ? Color.green : Color.red;

            Gizmos.DrawRay(transform.position, Vector3.up * rayDistance);
            Gizmos.DrawWireSphere(transform.position + (Vector3.up * rayDistance), circleRaduis);

            Gizmos.color = Color.white;
        }
    }
}
