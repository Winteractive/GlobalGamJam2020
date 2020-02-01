using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidePlayerLock : PlayerPart, IMediatorListener
{
    public bool isRiding = false;
    private Transform mountedPlayer;
    public float yOffset = 0.8f;

    private void Update()
    {
        if (isRiding)
        {
            if (mountedPlayer)
            {
                transform.position = mountedPlayer.position + new Vector3(0, yOffset, 0);
            }
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
        //Debug.DrawRay(transform.position, bottomPlayer.transform.position + new Vector3(0, yOffset, 0) - transform.position, Color.red, 0.5f);
        //RaycastHit2D hitData = Physics2D.CircleCast(transform.position, 0.05f, bottomPlayer.transform.position + new Vector3(0, yOffset, 0) - transform.position, LayerMask.GetMask("Ground"));
        //Debug.Log("hit " + hitData.collider.name);
        //Debug.Log(hitData);
        //if (false)
        //{
        //    Debug.Log("RAYCAST HIT SOMETHING I THINK");
        //    return;
        //}

        Debug.Log("MOUNTING PLAYER");
        mountedPlayer = bottomPlayer.transform;
        transform.position = mountedPlayer.position + new Vector3(0, yOffset, 0);
        isRiding = true;
    }

    private void DismountPlayer()
    {
        Debug.Log("DISMOUNTING PLAYER");
        mountedPlayer = null;
        isRiding = false;
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.PLAYER_IS_MOUNTING))
        {
            Debug.Log("PlayerOnPLayer");
            if (data is PlayerTriggerBoxData tagData)
            {
                Debug.Log("PlayerOnPLayer: " + tagData.enterExit);
                if (tagData.id == playerNumber && tagData.enterExit)
                {
                    MountPlayer(tagData.collidingObject);
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
    }
}
