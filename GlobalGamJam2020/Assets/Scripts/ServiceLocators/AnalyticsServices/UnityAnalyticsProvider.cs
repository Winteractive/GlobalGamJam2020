using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
/// <summary>
/// Uses Unitys Analytics Service for handeling Analytics
/// </summary>
public class UnityAnalyticsProvider : IAnalyticsService
{
    public void Initialize()
    {
        Debug.Log("unity analytics initialized");
    }

    public void SendAnalytics(string EventName, AnalyticParameter[] parameters = null)
    {
        Analytics.CustomEvent(EventName, parameters.GetAsKeyValuePairs());
    }
}
