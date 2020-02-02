using System;
using UnityEngine;

public static class MoveToNextLevel
{
    private const float oneLevelHeight = 0.0f;
    public static GameObject cam;
    public static float firstLevelYPos;
    public static int currentLevel;

    public static void Initialize()
    {
        currentLevel = 1;
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
                case GameEvents.GAME_STARTED:
                    GlobalMediator.SendMessage(GameEvents.LEVEL_START, new LevelStartData { levelIndex = currentLevel });
                    break;
                case GameEvents.LEVEL_WON:
                    currentLevel++;
                    LeanTween.moveY(cam, cam.transform.position.y + oneLevelHeight, 1.2f).setEaseInOutBack().setDelay(2.25f).setOnComplete(() => GlobalMediator.SendMessage(GameEvents.LEVEL_START, new LevelStartData { levelIndex = currentLevel }));
                    break;
                case GameEvents.RESET_GAME:
                    //  LeanTween.moveY(cam, firstLevelYPos, 1.2f).setEaseInOutBack().setDelay(0.1f);
                    break;
                default:
                    break;
            }
        }
    }
}
