using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





public static class AudioManager
{
    public static void PlaySoundEvent(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    break;
                case GameEvents.PLAYER_GROUND_CHECK:
                    GroundCheckData check = (GroundCheckData)data;
                    UnitAnimator.Character character = check.id.GetCharacterFromID();
                    if (check.isGrounded)
                    {

                    }
                    else
                    {

                    }
                    break;
                case GameEvents.PLAYER_CHARGE_START:
                    break;
                case GameEvents.PLAYER_CHARGE_RELEASED:
                    break;
                case GameEvents.PLAYER_CHARGE_CANCELLED:
                    break;
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    break;
                case GameEvents.PLAYER_BREAK:
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    break;
                case GameEvents.PLAYER_ON_PLAYER_CHECK:
                    break;
                case GameEvents.PLAYER_SLEEP:
                    break;
                default:
                    break;
            }
        }
    }
}
