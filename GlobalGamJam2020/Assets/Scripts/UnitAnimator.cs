using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnitAnimator : PlayerPart, IMediatorListener
{
    private SpriteRenderer spriteRenderer;
    private string characterBase = "Sprites/Characters/";
    private string PinkFolder = "Pink/";
    private string BlueFolder = "Blue/";
    private string FallbackFolder = "Fallback/";

    private string animationInfo = "AnimationInfo";

    public int FPS;
    public int currentFrame;
    private Sprite[] currentAnimation;

    public enum Character { Blue, Pink };

    private Dictionary<string, Sprite[]> foundAnimations;
    private Dictionary<string, UnitAnimationInfo> foundInfo;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        spriteRenderer = GetComponent<SpriteRenderer>();
        foundAnimations = new Dictionary<string, Sprite[]>();
        foundInfo = new Dictionary<string, UnitAnimationInfo>();
        TryStartAnimation(playerNumber == 1 ? Character.Blue : Character.Pink, "Idle");
        SetFrameRate(playerNumber == 1 ? Character.Blue : Character.Pink, "Idle");
        NextFrame();
    }

    private void SetFrameRate(Character character, string animation)
    {
        string path = characterBase + ((character is Character.Blue) ? BlueFolder : PinkFolder) + animation + "/" + animationInfo;
        string fallbackPath = characterBase + ((character is Character.Blue) ? BlueFolder : PinkFolder) + FallbackFolder + animationInfo;
     
        UnitAnimationInfo info = null;

        if (foundInfo.ContainsKey(path))
        {
            info = foundInfo[path];
        }
        else
        {
            info = Resources.Load<UnitAnimationInfo>(path);

            if (info != null)
            {
                foundInfo.Add(path, info);
            }
            else
            {
                info = Resources.Load<UnitAnimationInfo>(fallbackPath);
                Debug.LogWarning("fallback info used for: " + character.ToString() + " " + animation);
            }
        }
        Debug.Log(info);
        FPS = info.FPS;
    }

    public void NextFrame()
    {
        currentFrame++;
        if (currentFrame > currentAnimation.Length - 1)
        {
            currentFrame = 0;
        }

        spriteRenderer.sprite = currentAnimation[currentFrame] ?? null;

        Invoke("NextFrame", 1f / (float)FPS);
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (data is int == false) return;

        int id = (int)data;
        if (id != playerNumber) return;



    }

    public void TryStartAnimation(Character character, string animation)
    {
        string path = characterBase + ((character is Character.Blue) ? BlueFolder : PinkFolder) + animation + "/";

        if (foundAnimations.ContainsKey(path))
        {
            currentAnimation = foundAnimations[path];
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        if (sprites == null)
        {
            Debug.LogWarning("fallback animation used for: " + character.ToString() + " " + animation);
            sprites = Resources.LoadAll<Sprite>(characterBase + ((character is Character.Blue) ? BlueFolder : PinkFolder) + FallbackFolder);
            if (sprites == null)
            {
                Debug.LogWarning("fallback folder not found");
            }
            else
            {
                foundAnimations.Add(path, sprites);
            }
        }

        currentFrame = -1;
        currentAnimation = sprites;

    }
}