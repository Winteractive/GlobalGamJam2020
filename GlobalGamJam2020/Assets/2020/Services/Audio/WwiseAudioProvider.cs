using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseAudioProvider : IAudioService
{
    private GameObject gameObjectID;
    Dictionary<string, List<uint>> eventNameToIDs;

    public void Initialize()
    {
        gameObjectID = new GameObject("@Wwise gameObject ID");
        Object.DontDestroyOnLoad(gameObjectID);
        eventNameToIDs = new Dictionary<string, List<uint>>();
    }

    public void PauseAll()
    {
        AkSoundEngine.PostEvent("PauseAll", gameObjectID);
    }

    private void WwiseFormatting(ref string audioName)
    {
        audioName = audioName.ToLower();
    }

    public void PauseSound(string audioName)
    {
        WwiseFormatting(ref audioName);

        if (eventNameToIDs.ContainsKey(audioName))
        {
            foreach (uint ID in eventNameToIDs[audioName])
            {
                AkSoundEngine.ExecuteActionOnPlayingID(AkActionOnEventType.AkActionOnEventType_Pause, ID);
            }
        }
        else
        {
            Debug.LogWarning("Wwise Warning: " + audioName + " can not be paused.");
        }
    }


    void CallBack(object in_cookie, AkCallbackType in_type, object in_info)
    {
        Debug.Log(in_type);
        string finishedEventName = in_cookie as string;
        AkEventCallbackInfo info = in_info as AkEventCallbackInfo;

        Debug.Log(info.eventID);
        Debug.Log(finishedEventName);

        if (eventNameToIDs.ContainsKey(finishedEventName))
        {
            try
            {
                eventNameToIDs[finishedEventName].Remove(info.eventID);
            }
            catch
            {
                Debug.LogError("Wwise Error: ID: " + info.eventID + " not found in dictionary with key: " + finishedEventName);
            }

        }
        else
        {
            Debug.LogError("Wwise Error: " + finishedEventName + " is not a used key");

        }
    }

    /// <param name="loop">this parameter is not in use when utilizing wwise</param>
    public void PlaySound(string audioName, bool loop = false)
    {
        WwiseFormatting(ref audioName);

        try
        {
            if (eventNameToIDs.ContainsKey(audioName))
            {
                eventNameToIDs[audioName].Add(AkSoundEngine.PostEvent(in_pszEventName: audioName, gameObjectID, in_uFlags: (uint)AkCallbackType.AK_EndOfEvent, CallBack, audioName));
            }
            else
            {
                eventNameToIDs.Add(audioName, new List<uint>() { AkSoundEngine.PostEvent(audioName, gameObjectID) });
            }
        }
        catch
        {
            Debug.LogError("Wwise Error: " + audioName + " not present in bank.");
        }
    }

    public void SetRTPC(string valueName, float value)
    {
        WwiseFormatting(ref valueName);

        AkSoundEngine.SetRTPCValue(valueName, value);
    }

    public void StopAll()
    {
        AkSoundEngine.PostEvent("StopAll", gameObjectID);
    }

    public void StopSound(string audioName)
    {
        WwiseFormatting(ref audioName);

        if (eventNameToIDs.ContainsKey(audioName))
        {
            foreach (uint ID in eventNameToIDs[audioName])
            {
                AkSoundEngine.StopPlayingID(ID);
            }

            eventNameToIDs.Remove(audioName);
        }
        else
        {
            Debug.LogWarning("Wwise Warning: " + audioName + " is not playing and can therefor not be stopped");
        }
    }

    public void ResumeSound(string audioName)
    {
        WwiseFormatting(ref audioName);

        if (eventNameToIDs.ContainsKey(audioName))
        {
            foreach (uint ID in eventNameToIDs[audioName])
            {
                AkSoundEngine.ExecuteActionOnPlayingID(AkActionOnEventType.AkActionOnEventType_Resume, ID);
            }
        }
        else
        {
            Debug.LogWarning("Wwise Warning: " + audioName + " cant be resumed");
        }
    }

    public void ResumeAll()
    {
        AkSoundEngine.PostEvent("ResumeAll", gameObjectID);
    }

    public void PlayMusic(string musicName)
    {
        PlaySound(musicName);
    }

    public void PlaySFX(string clipName)
    {
        PlaySound(clipName);
    }

    public void PlaySFX(string clipName, GameObject gameObject)
    {
        PlaySound(clipName);
    }

    public void SetVolume(MixerName mixer, float volume)
    {
        Debug.Log("volume change not implemented");
    }
}
