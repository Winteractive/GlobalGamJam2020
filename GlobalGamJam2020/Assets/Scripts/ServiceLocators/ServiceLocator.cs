/*[Note]:
The service locator should maybe not initialize services by looking at GameManager. GameManager should probably tell the Locator
What to initialize

The service locator should initialize everything as a Null provider

Where's the SetService Function(s)?
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator
{
    
    public enum AudioOptions
    {
        Null,
        Debug,
        Wwise
    }
    public enum AnalyticsOptions
    {
        Null,
        Facebook,
        Debug,
        Unity
    }


  
    private static IAudioService audioService;
    private static IAnalyticsService analyticsService;
    
    public static void Initialize()
    {

        audioService = new NullAudioService();
        analyticsService = new NullAnalyticsProvider();

    }

    /// <summary>
    /// Only works with INetworkServie, IAudioServie & IAnalyticsService
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetService<T>()
    { 
        if (typeof(T) == typeof(IAudioService))
        {
            return (T)audioService;
        }
        if (typeof(T) == typeof(IAnalyticsService))
        {
            return (T)analyticsService;
        }
        return default;
    }
}
