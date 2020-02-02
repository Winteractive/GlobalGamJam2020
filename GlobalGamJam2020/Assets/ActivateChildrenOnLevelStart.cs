using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateChildrenOnLevelStart : MonoBehaviour
{
    private List<Transform> allChilren;
    public int levelID;
    // Start is called before the first frame update
    void Awake()
    {
        allChilren = new List<Transform>();
        foreach (var item in GetComponentsInChildren<Transform>())
        {
            if (item.gameObject == this.gameObject) continue;
            allChilren.Add(item);
        }
        GlobalMediator.AddListener(RecieveEvents);
        foreach (var item in allChilren)
        {
            if (item.gameObject == this.gameObject) continue;
            item.gameObject.SetActive(false);
        }
    }

    private void RecieveEvents(GameEvents events, GeneralData data)
    {
        if (events.HasFlag(GameEvents.LEVEL_START))
        {
            LevelStartData lvlData = (LevelStartData)data;
            foreach (var item in allChilren)
            {
                if (item.gameObject == this.gameObject) continue;

                item.gameObject.SetActive(lvlData.levelIndex == levelID);
            }
        }
        // if (events.HasFlag(GameEvents.GAME_STARTED))
        // {
        //     LevelStartData lvlData = (LevelStartData)data;
        //     foreach (var item in GetComponentsInChildren<Transform>())
        //     {
        //         if (item.gameObject == this.gameObject) continue;
        //
        //         item.gameObject.SetActive(lvlData.levelIndex == levelID);
        //     }
        // }
    }


}
