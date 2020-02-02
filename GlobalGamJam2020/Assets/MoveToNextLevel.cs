using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MoveToNextLevel
{
    private const float oneLevelHeight = 13.0f;
    public static GameObject cam;
    public static float firstLevelYPos;

    // Start is called before the first frame update
    public static void Initialize()
    {
        GlobalMediator.AddListener(RecieveEvents);
        cam = UnityEngine.Object.FindObjectOfType<Camera>().gameObject;
        firstLevelYPos = cam.transform.position.y;
    }

    public static void RecieveEvents(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.LEVEL_WON:
                    LeanTween.moveY(cam, cam.transform.position.y + oneLevelHeight, 1.2f).setEaseInOutBack().setDelay(1f);
                    break;
                case GameEvents.RESET_GAME:
                    LeanTween.moveY(cam, firstLevelYPos, 1.2f).setEaseInOutBack().setDelay(0.1f);
                    break;
                default:
                    break;
            }
        }
    }
}
