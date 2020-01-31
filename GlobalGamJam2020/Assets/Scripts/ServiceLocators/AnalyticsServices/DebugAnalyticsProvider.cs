using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Sends out Debug Logs instead of recording all events 
/// </summary>
public class DebugAnalyticsProvider : IAnalyticsService
{
    public void Initialize()
    {
        Debug.Log("analytics - debug analytics service initialized");
    }

    public void SendAnalytics(string EventName, AnalyticParameter[] parameters = null)
    {
        if (parameters != null)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].Legitimize() == false)
                {
                    Debug.LogError("parameter was not int, double, string or boolean");
                    Debug.LogError(parameters[i].parameter.GetType());
                    return;
                }
                Debug.Log("analytics - adding parameter: " + parameters[i].parameterName + " with value: " + parameters[i].parameter.ToString());
            }
        }

        Debug.Log("analytics - pretending to send: " + EventName);
    }
}
