using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Restarter
{
    static float timer;
    static int AwakePlayers = 2;

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
                case GameEvents.PLAYER_SLEEP:
                    AwakePlayers--;
                    if (AwakePlayers == 0)
                    {
                        //Using leantween as a timer
                        LeanTween.value(1, 2, 2f).setOnComplete(()=>
                        {
                            if (AwakePlayers == 0)
                            {
                                Debug.Log("SLEPT FOR 2 SEC");
                                GlobalMediator.SendMessage(GameEvents.RESTART_LEVEL);
                            }
                        });
                    }
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    AwakePlayers = 2;
                    break;
            }
        }
    }
}
