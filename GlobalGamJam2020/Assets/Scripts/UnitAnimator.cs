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

    private string myCharacterFolder = string.Empty;

    private string animationInfo = "AnimationInfo";

    public int currentFrame;
    private Sprite[] currentAnimation;
    private UnitAnimationInfo currentInfo;


    public enum Character { Blue, Pink };
    private Character myCharacter;

    private Dictionary<string, Sprite[]> foundAnimations;
    private Dictionary<string, UnitAnimationInfo> foundInfo;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        spriteRenderer = GetComponent<SpriteRenderer>();
        foundAnimations = new Dictionary<string, Sprite[]>();
        foundInfo = new Dictionary<string, UnitAnimationInfo>();


        myCharacter = playerNumber == 1 ? Character.Blue : Character.Pink;
        myCharacterFolder = (myCharacter is Character.Blue) ? BlueFolder : PinkFolder;


        TryStartAnimation("Idle");
        SetFrameRate("Idle");
        NextFrame();
    }

    private void SetFrameRate(string animation)
    {
        string path = characterBase + myCharacterFolder + animation + "/" + animationInfo;
        string fallbackPath = characterBase + myCharacterFolder + FallbackFolder + animationInfo;

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
                Debug.LogWarning("fallback info used for: " + myCharacter.ToString() + " " + animation);
            }
        }

        currentInfo = info;

    }

    public void NextFrame()
    {
        currentFrame++;
        if (currentFrame > currentAnimation.Length - 1)
        {
            if (!string.IsNullOrEmpty(currentInfo.TransitionsTo))
            {
                TryStartAnimation(currentInfo.TransitionsTo);
            }
            currentFrame = 0;
        }

        spriteRenderer.sprite = currentAnimation[currentFrame] ?? null;

        Invoke("NextFrame", 1f / (float)currentInfo.FPS);
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {
        if (data is int == false) return; // to weak

        int id = (int)data;
        if (id != playerNumber) return;


        if (events.HasFlag(GameEvents.PLAYER_CHARGE_START)) // Example
        {
            TryStartAnimation("ChargeUp");
            SetFrameRate("ChargeUp");
        }

    }

    public void TryStartAnimation(string animation)
    {
        string path = characterBase + myCharacterFolder + animation + "/";

        if (foundAnimations.ContainsKey(path))
        {
            currentAnimation = foundAnimations[path];
            currentFrame = -1;
            return;
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(path);

        if (sprites == null || sprites.Length == 0)
        {
            Debug.LogWarning("fallback animation used for: " + myCharacter.ToString() + " " + animation);
            sprites = Resources.LoadAll<Sprite>(characterBase + myCharacterFolder + FallbackFolder);
            if (sprites == null)
            {
                Debug.LogWarning("fallback folder not found");
            }
        }
        else
        {
            foundAnimations.Add(path, sprites);
        }

        currentFrame = -1;
        currentAnimation = sprites;

    }
}