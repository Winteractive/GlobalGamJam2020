using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationInfo", menuName = "AnimationInfo", order = 0)]
public class UnitAnimationInfo : ScriptableObject
{
    public int FPS = 12;
    public string TransitionsTo;
}
