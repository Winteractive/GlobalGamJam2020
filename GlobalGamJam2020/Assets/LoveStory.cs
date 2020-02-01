using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LoveStory
{

    public static void Initalize()
    {
        GlobalMediator.AddListener(RecieveEvents);
    }

    private static List<string> mountMessages = new List<string>()
    {
        "hello.",
        "hi.",
        "I'm glad you're here.",
        "I missed you.",
        "welcome back.",
        "you're never heavy.",
        "please carry me."
    };

    private static List<string> ChargeReleaseMessages = new List<string>()
    {
        "You told me to keep moving, I hope you do the same.",
        "Follow me.",
        "I hope I can follow you."
    };

    private static List<string> ChargeStartMessages = new List<string>()
    {
        "I feel tense",
        "Is this going to hurt?"
    };


    private static List<string> SleepMessages = new List<string>()
    {
        "I used to fear sleeping.",
        "I long to be with you again.",
        "I miss you already"
    };

    public static void RecieveEvents(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {

            }
        }
    }

    private static void DisplayMessage(string message)
    {

    }

    private static void TrySleepMessage()
    {
        if (SleepMessages.Count > 0)
        {
            string message = SleepMessages.OrderBy(x => Guid.NewGuid()).ToList()[0];
            DisplayMessage(message);
            SleepMessages.Remove(message);
        }
    }
    private static void TryMountMessage()
    {

    }
    private static void TryChargeReleaseMessage()
    {

    }
    private static void TryChargeStartMessage()
    {

    }

}
