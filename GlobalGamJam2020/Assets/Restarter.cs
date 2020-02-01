using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Restarter
{
    static float timer;

    public static void Initialize()
    {
        timer = 0;
        GlobalMediator.AddListener(RecieveEvent);
    }

    public static void RecieveEvent(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    PlayerInputData pInData = (PlayerInputData)data;
                    if (pInData.key_respawn)
                    {
                        timer += Time.deltaTime;
                        if (timer > 1.5f)
                        {
                            Debug.Log("restart level");
                            timer = 0;
                            GlobalMediator.SendMessage(GameEvents.RESTART_LEVEL);
                            return;
                        }
                    }
                    else
                    {
                        timer = 0;
                    }
                    break;
            }
        }
    }
}
