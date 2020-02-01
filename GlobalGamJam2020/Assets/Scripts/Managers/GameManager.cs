/*[Note]:
The fact that we can have multiple GameManagers in different scenes (not at runtime) means that different iterations of the 
manager can have different servicelocators selected. This could potentially create some really hard to understand bugs

We should probably have a gamestate enum
GameManager should probably do more, e.g switch scenes or pause / unpause? what do you think?
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all the managers
/// </summary>
[DisallowMultipleComponent()]
public class GameManager : MonoBehaviour, IMediatorListener
{

    public static GameManager INSTANCE;

    public GameSettings settings;

    private void Awake()
    {
        if (INSTANCE)
        {
            Destroy(gameObject);
        }
        else
        {
            INSTANCE = this;
            GlobalMediator.AddListener(this);
            RumbleManager.Initialize();
            ServiceLocator.Initialize();
            AudioManager.Initialize();
            InputManager.Initialize();
            DontDestroyOnLoad(gameObject);
        }
        //Time.timeScale = 0.5f;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
    private void OnDestroy()
    {
        GlobalMediator.RemoveListener(this);
    }

    private void Start()
    {

    }
    private void Update()
    {
        UpdateVolumes();
    }
    public void UpdateVolumes()
    {

#if UNITY_EDITOR
        //Makes so the Mute Audio button in unity window mutes sound
        settings.MASTER_MUTE = UnityEditor.EditorUtility.audioMasterMute;
#endif
        if (settings.MASTER_MUTE)
        {
            ServiceLocator.GetService<IAudioService>().SetVolume(MixerName.MASTER, 0);
            return;
        }
        ServiceLocator.GetService<IAudioService>().SetVolume(MixerName.MASTER, settings.MASTER_VOLUME);
        ServiceLocator.GetService<IAudioService>().SetVolume(MixerName.MUSIC, settings.MUSIC_VOLUME);
        ServiceLocator.GetService<IAudioService>().SetVolume(MixerName.SFX, settings.SFX_VOLUME);
        ServiceLocator.GetService<IAudioService>().SetVolume(MixerName.VOICE, settings.VOX_VOLUME);
    }

    private void OnApplicationQuit()
    {
        RumbleManager.PauseAllControllers();
    }

    public void OnMediatorMessageReceived(GameEvents events, object data)
    {

    }
}
