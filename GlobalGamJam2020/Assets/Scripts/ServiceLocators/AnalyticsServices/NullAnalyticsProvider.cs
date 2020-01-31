using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Does nothing
/// </summary>
public class NullAnalyticsProvider : IAnalyticsService
{
    public void Initialize()
    {

    }

    public void SendAnalytics(string EventName, AnalyticParameter[] parameters = null)
    {

    }
}
