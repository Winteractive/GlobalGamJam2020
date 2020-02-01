using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Screenshaker
{
    static GameObject shakeObj;

    public static void Initialize()
    {
        shakeObj = new GameObject("@Screenshake Object");
        shakeObj.transform.position = Vector3.zero;
        UnityEngine.Object.FindObjectOfType<Camera>().transform.SetParent(shakeObj.transform, true);
        GlobalMediator.AddListener(RecieveEvents);
    }

    private static void RecieveEvents(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    break;
                case GameEvents.PLAYER_GROUND_CHECK:
                    GroundCheckData gData = (GroundCheckData)data;
                    if (gData.isGrounded)
                    {
                        Shake();
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
                case GameEvents.PLAYER_COLLIDE_WITH_PLAYER:
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    break;
                case GameEvents.PLAYER_GOT_MOUNTED:
                    break;
                case GameEvents.PLAYER_IS_MOUNTING:
                    break;
                case GameEvents.PLAYER_SLEEP:
                    break;
                case GameEvents.RESTART_LEVEL:
                    break;
                case GameEvents.GAME_STARTED:
                    break;
                case GameEvents.PLAYER_REPAIR_TRIGGER_BOX:
                    break;
            }
        }
    }

    private static void Shake()
    {
        LeanTween.cancel(shakeObj);
        shakeObj.transform.position = Vector3.zero;
        LeanTween.moveLocal(shakeObj, UnityEngine.Random.insideUnitCircle.normalized * 0.3f, 0.3f).setEasePunch().setOnComplete(() => shakeObj.transform.position = Vector3.zero);
    }
}
