using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseAudioService : IAudioService
{
    public static GameObject objRef;
    public WwiseAudioService()
    {
        objRef = new GameObject("Wwise gameObject Reference");
    }

    public void PlayMusic(string musicName)
    {
        AkSoundEngine.PostEvent(musicName, objRef);
    }

    public void PlaySFX(string clipName)
    {
        AkSoundEngine.PostEvent(clipName, objRef);
    }

    public void StopAll()
    {
        AkSoundEngine.StopAll();
    }

    public void PauseAll()
    {

    }

    public void ResumeAll()
    {

    }

    public void PlaySFX(string clipName, GameObject gameObject)
    {
        AkSoundEngine.PostEvent(clipName, gameObject);
    }

    public void SetVolume(MixerName mixer, float volume)
    {
        switch (mixer)
        {
            case MixerName.MASTER:
                AkSoundEngine.SetRTPCValue("MASTER_VOLUME", volume);
                break;
            case MixerName.SFX:
                AkSoundEngine.SetRTPCValue("SFX_VOLUME", volume);
                break;
            case MixerName.MUSIC:
                AkSoundEngine.SetRTPCValue("MUSIC_VOLUME", volume);
                break;
            case MixerName.VOICE:
                AkSoundEngine.SetRTPCValue("VOX_VOLUME", volume);
                break;
            default:
                break;
        }
    }
}
