//using Facebook.Unity; // add this back after adding Facebook SDK
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Using Facebooks Analytics SDK
/// </summary>
public class FacebookAnalyticsProvider : IAnalyticsService
{
    public void Initialize()
    {
        Debug.Log("facebook analytics initialization");
        Debug.LogError("facebook not integrated");


        //  if (!FB.IsInitialized)
        //  {
        //      Debug.Log("facebook analytics first call");
        //      // Initialize the Facebook SDK
        //      FB.Init(InitCallback);
        //  }
        //  else
        //  {
        //      Debug.Log("facebook analytics already initialized");
        //      // Already initialized, signal an app activation App Event
        //      FB.ActivateApp();
        //  }
    }

    private void InitCallback()
    {
        // if (FB.IsInitialized)
        // {
        //     Debug.Log("facebook analytics initialized successfully");
        //     // Signal an app activation App Event
        //     FB.ActivateApp();
        //     // Continue with Facebook SDK
        //     // ...
        // }
        // else
        // {
        //     Debug.Log("Failed to Initialize the Facebook SDK");
        // }
    }

    public void SendAnalytics(string EventName, AnalyticParameter[] parameters = null)
    {
        Debug.Log("sending facebook analytics");
        Debug.LogError("facebook not integrated");


        //  var parameterDictionary = new Dictionary<string, object>();
        //
        //  if (parameters != null)
        //  {
        //      for (int i = 0; i < parameters.Length; i++)
        //      {
        //          AnalyticParameter item = parameters[i];
        //          if (item.Legitimize() == false)
        //          {
        //              Debug.LogError("parameter was not int, double, string or boolean");
        //              return;
        //          }
        //          parameterDictionary[item.parameterName] = item.parameter;
        //      }
        //  }



        //  if (parameterDictionary != null || parameterDictionary.Count > 0)
        //  {
        //      FB.LogAppEvent(EventName, parameters: parameterDictionary);
        //  }
        //  else
        //  {
        //      FB.LogAppEvent(EventName);
        //  }

    }
}
