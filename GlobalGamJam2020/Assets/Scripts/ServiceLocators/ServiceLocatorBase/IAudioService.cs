using UnityEngine;
public interface IAudioService
{
    void PlayMusic(string musicName);
    void PlaySFX(string clipName);
    void PlaySFX(string clipName, GameObject gameObject);
    void StopAll();
    void PauseAll();
    void ResumeAll();
    void SetVolume(MixerName mixer, float volume);
}

public enum MixerName
{
    MASTER,
    SFX,
    MUSIC,
    VOICE
}