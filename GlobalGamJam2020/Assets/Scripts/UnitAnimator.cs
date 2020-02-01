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
    [SerializeField] private UnitAnimationInfo currentInfo;

    public string currentAnimationName;
    public enum Character { Blue, Pink };
    private Character myCharacter;

    private Dictionary<string, Sprite[]> foundAnimations;
    private Dictionary<string, UnitAnimationInfo> foundInfo;

    public override void Initialize(int playerNumber)
    {
        base.Initialize(playerNumber);
        GlobalMediator.AddListener(this);
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
                SetFrameRate(currentInfo.TransitionsTo);
            }
            if (currentInfo.loop)
            {
                currentFrame = 0;
            }
            else 
            {
                currentFrame--;
            }
        }

        spriteRenderer.sprite = currentAnimation[currentFrame] ?? null;

        Invoke("NextFrame", 1f / (float)currentInfo.FPS);
    }

    public void OnMediatorMessageReceived(GameEvents events, GeneralData data)
    {

        foreach (GameEvents item in Enum.GetValues(typeof(GameEvents)))
        {
            if (events.HasFlag(item) == false) continue;

            switch (item)
            {
                case GameEvents.PLAYER_INPUT:
                    break;
                case GameEvents.PLAYER_GROUND_CHECK:
                    if (data is GroundCheckData tagMessage)
                    {
                        if (tagMessage.id == playerNumber)
                        {
                            if (tagMessage.isGrounded)
                            {
                                if (currentAnimationName != "Sleep")
                                {
                                    TryStartAnimation("Idle");
                                    SetFrameRate("Idle");
                                }

                            }
                        }
                    }
                    break;
                case GameEvents.PLAYER_CHARGE_START:
                    if (data is PlayerData chargeStart)
                    {
                        if (chargeStart.id == playerNumber)
                        {

                            TryStartAnimation("ChargeUp");
                            SetFrameRate("ChargeUp");

                        }
                    }

                    break;
                case GameEvents.PLAYER_CHARGE_RELEASED:
                    if (data is PlayerData chargeRelesed)
                    {
                        if (chargeRelesed.id == playerNumber)
                        {
                            TryStartAnimation("InAir");
                            SetFrameRate("InAir");

                        }
                    }
                    break;
                case GameEvents.PLAYER_CHARGE_CANCELLED:
                    
                    break;
                case GameEvents.PLAYER_TAKE_DAMAGE:
                    break;
                case GameEvents.PLAYER_REPAIRED:
                    if (data is PlayerData player)
                    {
                        if (player.id == playerNumber)
                        {
                            if(currentAnimationName != "Squat" && currentAnimationName != "OnTop" && currentAnimationName != "ChargeUp" && currentAnimationName != "InAir")
                            {
                                TryStartAnimation("Idle");
                                SetFrameRate("Idle");
                            }
                                
                        }
                    }
                    break;
                case GameEvents.PLAYER_IS_MOUNTING:
                    if (data is PlayerData playerIsMountingdata)
                    {
                        if (playerIsMountingdata.id == playerNumber)
                        {
                            TryStartAnimation("OnTop");
                            SetFrameRate("OnTop");
                        }
                    }
                    break;
                case GameEvents.PLAYER_SLEEP:
                    if (data is PlayerData sleepData)
                    {
                        if (sleepData.id == playerNumber)
                        {
                            TryStartAnimation("Sleep");
                            SetFrameRate("Sleep");
                        }
                    }
                    break;
                case GameEvents.PLAYER_GOT_MOUNTED:
                    if (data is PlayerData playerGotMounted)
                    {
                        if (playerGotMounted.id == playerNumber)
                        {
                            TryStartAnimation("Squat");
                            SetFrameRate("Squat");
                        }
                    }
                    break;
                case GameEvents.PLAYER_COLLIDE_WITH_PLAYER:
                    
                    break;
                case GameEvents.GAME_STARTED:
                    
                    break;
                default:
                    break;
            }
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
        currentAnimationName = animation;
    }
}