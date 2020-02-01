using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : PlayerPart, IMediatorListener
{
    private SpriteTrail.SpriteTrail spriteTrail;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        GlobalMediator.AddListener(this);
        spriteTrail = GetComponent<SpriteTrail.SpriteTrail>();
        toggleTrail(false);
    }

    private void toggleTrail(bool value)
    {
        if (value)
        {
            spriteTrail.EnableTrail();
            spriteTrail.m_CurrentTrailPreset.m_TrailDuration = 0.2f;
        }
        else
        {
            spriteTrail.m_CurrentTrailPreset.m_TrailDuration = 0.05f;
        }
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.PLAYER_GROUND_CHECK))
        {
            if (data is GroundCheckData tagData)
            {
                if (tagData.id == playerNumber && tagData.isGrounded)
                {
                    toggleTrail(false);
                }
            }
        }
        if (events.HasFlag(GameEvents.PLAYER_CHARGE_RELEASED))
        {
            if (data is PlayerChargeReleaseData chargeReleaseData)
            {
                if (chargeReleaseData.id == playerNumber)
                {
                    toggleTrail(true);
                }
            }
        }
    }

}
