using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioManager
{
    public static GameObject targetObject;

    public static void Initialize()
    {
        GlobalMediator.AddListener(PlaySoundEvent);
        targetObject = new GameObject("@Wwise Audio Target");
        

    }

    public static void PlaySoundEvent(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    PlayerInputData inputData = (PlayerInputData)data;
                    string player = inputData.id == 1 ? "blue_" : "pink_";

                    AkSoundEngine.SetRTPCValue(player + "volume", Mathf.Abs(inputData.axis.x));
                    break;
                case GameEvents.PLAYER_GROUND_CHECK:
                    GroundCheckData check = (GroundCheckData)data;
                    if (check.isGrounded)
                    {
                        AkSoundEngine.PostEvent("player_grounded", targetObject);
                    }
                    else
                    {

                    }
                    break;
                case GameEvents.PLAYER_CHARGE_START:
                    PlayerData pData = (PlayerData)data;
                    UnitAnimator.Character character = pData.id.GetCharacterFromID();
                    switch (character)
                    {
                        case UnitAnimator.Character.Blue:
                            AkSoundEngine.PostEvent("blue_start_charge", targetObject);
                            break;
                        case UnitAnimator.Character.Pink:
                            AkSoundEngine.PostEvent("pink_start_charge", targetObject);
                            break;
                        default:
                            break;
                    }
                    break;
                case GameEvents.PLAYER_CHARGE_RELEASED:
                    pData = (PlayerData)data;
                    character = pData.id.GetCharacterFromID();
                    switch (character)
                    {
                        case UnitAnimator.Character.Blue:
                            AkSoundEngine.PostEvent("blue_release_charge", targetObject);
                            break;
                        case UnitAnimator.Character.Pink:
                            AkSoundEngine.PostEvent("pink_release_charge", targetObject);
                            break;
                        default:
                            break;
                    }
                    break;
                case GameEvents.PLAYER_CHARGE_CANCELLED:
                    AkSoundEngine.PostEvent("player_grounded", targetObject);
                    break;
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    AkSoundEngine.PostEvent("player_repair", targetObject);
                    break;
                case GameEvents.PLAYER_IS_MOUNTING:
                    break;
                case GameEvents.PLAYER_SLEEP:
                    AkSoundEngine.PostEvent("player_sleep", targetObject);
                    break;
                case GameEvents.PLAYER_GOT_MOUNTED:
                    AkSoundEngine.PostEvent("player_mounted", targetObject);
                    break;
                case GameEvents.PLAYER_COLLIDE_WITH_PLAYER:
                    AkSoundEngine.PostEvent("players_collide", targetObject);
                    break;
                case GameEvents.GAME_STARTED:
                    AkSoundEngine.PostEvent("bgm_start", targetObject);
                    break;
                default:
                    break;
            }
        }
    }
}
