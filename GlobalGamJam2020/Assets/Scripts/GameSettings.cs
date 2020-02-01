using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings")]
public class GameSettings : ScriptableObject
{
    [Header("Service Locators")]
    public ServiceLocator.AnalyticsOptions ANALYTICS_SERVICE;


    public bool MASTER_MUTE;
    [Range(0,100)]
    public float MASTER_VOLUME = 100;
    [Range(0, 100)]
    public float MUSIC_VOLUME = 100;
    [Range(0, 100)]
    public float SFX_VOLUME = 100;
    [Range(0, 100)]
    public float VOX_VOLUME = 100;


}
