using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventWriter
{
    public static void Initialize()
    {
        GlobalMediator.AddListener(RecieveEvents);
    }

    private static void RecieveEvents(GameEvents events, GeneralData data)
    {
      //  if (events != GameEvents.PLAYER_INPUT)
          //  Debug.Log(events);
    }
}
