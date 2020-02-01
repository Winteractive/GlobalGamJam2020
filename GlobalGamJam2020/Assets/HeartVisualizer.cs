using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartVisualizer : MonoBehaviour
{

    public static List<SpriteRenderer> p1Hearts;
    public static List<SpriteRenderer> p2Hearts;
    public static Sprite filledHeart;
    public static Sprite emptyHeart;

    public GameObject p1;
    public GameObject p2;

    public static bool initalized;

    public static void Initialize()
    {
        initalized = true;
        filledHeart = Resources.Load<Sprite>("Sprites/HeartFilled");
        emptyHeart = Resources.Load<Sprite>("Sprites/HeartEmpty");

        p1Hearts = new List<SpriteRenderer>();
        p2Hearts = new List<SpriteRenderer>();

        GlobalMediator.AddListener(RecieveEvents);

        for (int i = 0; i < 3; i++)
        {
            SpriteRenderer heart = Instantiate(new GameObject("p1 heart " + i)).AddComponent<SpriteRenderer>();
            p1Hearts.Add(heart);
        }

        for (int i = 0; i < 3; i++)
        {
            SpriteRenderer heart = Instantiate(new GameObject("p2 heart " + i)).AddComponent<SpriteRenderer>();
            p2Hearts.Add(heart);
        }

        foreach (var item in p1Hearts)
        {
            item.sprite = filledHeart;
        }

        foreach (var item in p2Hearts)
        {
            item.sprite = filledHeart;
        }
    }

    public static SpriteRenderer GetFirstFullHeart(int playerID)
    {
        foreach (var item in playerID == 1 ? p1Hearts : p2Hearts)
        {
            if (item.sprite == filledHeart) return item;
        }

        return null;
    }

    public static List<SpriteRenderer> GetAllHeartsFromCharacter(int playerID)
    {
        return playerID == 1 ? p1Hearts : p2Hearts;
    }

    private static void RecieveEvents(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    PlayerData pData = (PlayerData)data;
                    SpriteRenderer heart = GetFirstFullHeart(pData.id);
                    if (heart != null)
                    {
                        LeanTween.scale(heart.gameObject, Vector2.one * 2, 0.2f).setEasePunch();
                        heart.sprite = emptyHeart;
                    }

                    break;
                case GameEvents.PLAYER_REPAIRED:
                    pData = (PlayerData)data;
                    foreach (var aHeart in GetAllHeartsFromCharacter(pData.id))
                    {
                        if (aHeart.sprite == emptyHeart)
                        {
                            LeanTween.scale(aHeart.gameObject, Vector2.one * 2, 0.2f).setEasePunch();
                            aHeart.sprite = filledHeart;
                        }
                    }
                    break;

            }
        }
    }

    private void LateUpdate()
    {
        if (initalized == false) return;

        for (int i = 0; i < p1Hearts.Count; i++)
        {
            p1Hearts[i].transform.position = p1.transform.position;
            p1Hearts[i].transform.position += Vector3.up * 0.5f;
            p1Hearts[i].transform.position += Vector3.right * (i - 1) * 0.3f;
        }

        for (int i = 0; i < p2Hearts.Count; i++)
        {
            p2Hearts[i].transform.position = p2.transform.position;
            p2Hearts[i].transform.position += Vector3.up * 0.5f;
            p2Hearts[i].transform.position += Vector3.right * (i - 1) * 0.3f;
        }
    }
}
