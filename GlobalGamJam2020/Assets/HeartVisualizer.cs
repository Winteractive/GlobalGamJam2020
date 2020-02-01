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

    public void Awake()
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

        StartCoroutine(ShowHearts(false, 1, 0f));
        StartCoroutine(ShowHearts(false, 2, 0f));
    }

    public static SpriteRenderer GetFirstFullHeart(int playerID)
    {
        for (int i = 2; i >= 0; i--)
        {
            SpriteRenderer item = playerID == 1 ? p1Hearts[i] : p2Hearts[i];
            if (item.sprite == filledHeart) return item;
        }


        return null;
    }

    public static List<SpriteRenderer> GetAllHeartsFromCharacter(int playerID)
    {
        return playerID == 1 ? p1Hearts : p2Hearts;
    }



    public IEnumerator TakeDamage(int playerID)
    {

        foreach (var item in playerID == 1 ? p1Hearts : p2Hearts)
        {
            LeanTween.scale(item.gameObject, Vector3.one, 0.1f).setEaseInOutBack();
        }

        yield return new WaitForSeconds(0.1f);

        SpriteRenderer heart = GetFirstFullHeart(playerID);


        if (heart != null)
        {
            LeanTween.cancel(heart.gameObject);
            heart.transform.localScale = Vector3.one;
            LeanTween.scale(heart.gameObject, Vector2.one * 2.5f, 0.3f).setEasePunch();
            heart.sprite = emptyHeart;
        }

        yield return new WaitForSeconds(0.3f);

        foreach (var item in playerID == 1 ? p1Hearts : p2Hearts)
        {
            LeanTween.cancel(heart.gameObject);
            heart.transform.localScale = Vector3.one;
            LeanTween.scale(item.gameObject, Vector3.zero, 0.1f).setEaseInOutBack();
        }
    }

    public IEnumerator GetRepaired(int playerID)
    {
        foreach (var item in playerID == 1 ? p1Hearts : p2Hearts)
        {
            LeanTween.scale(item.gameObject, Vector3.one, 0.1f).setEaseInOutBack();
        }

        yield return new WaitForSeconds(0.1f);

        SpriteRenderer heart = GetFirstFullHeart(playerID);




        foreach (var item in playerID == 1 ? p1Hearts : p2Hearts)
        {
            if (item.sprite == emptyHeart)
            {
                LeanTween.cancel(item.gameObject);
                item.transform.localScale = Vector3.one;
                LeanTween.scale(item.gameObject, Vector2.one * 2, 0.2f).setEasePunch();
                item.sprite = filledHeart;
            }
        }

        yield return new WaitForSeconds(0.3f);

        foreach (var item in playerID == 1 ? p1Hearts : p2Hearts)
        {
            LeanTween.cancel(heart.gameObject);
            heart.transform.localScale = Vector3.one;
            LeanTween.scale(item.gameObject, Vector3.zero, 0.1f).setEaseInOutBack();
        }
    }

    private void RecieveEvents(GameEvents events, GeneralData data)
    {
        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    PlayerData pData = (PlayerData)data;
                    StartCoroutine(TakeDamage(pData.id));

                    break;
                case GameEvents.PLAYER_REPAIRED:
                    pData = (PlayerData)data;
                    StartCoroutine(GetRepaired(pData.id));
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
            p1Hearts[i].transform.position += Vector3.up * 0.4f;
            p1Hearts[i].transform.position += Vector3.right * (i - 1) * 0.25f;
        }

        for (int i = 0; i < p2Hearts.Count; i++)
        {
            p2Hearts[i].transform.position = p2.transform.position;
            p2Hearts[i].transform.position += Vector3.up * 0.4f;
            p2Hearts[i].transform.position += Vector3.right * (i - 1) * 0.25f;
        }
    }
}
