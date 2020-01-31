using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Parameter for a Analytic Event
/// </summary>
[Serializable]
public struct AnalyticParameter
{
    public string parameterName;
    public object parameter;

    public AnalyticParameter(string _parameterName, object _parameter)
    {
        parameterName = _parameterName;
        parameter = _parameter;
    }

    public bool Legitimize()
    {
        if (parameter == null) return false;

        Type type = parameter.GetType();
        return type == typeof(int) || type == typeof(string) || type == typeof(bool) || type == typeof(double);
    }


}
/// <summary>
/// Extension for AnalyticsParameters
/// </summary>
public static class AnalyticsParameterExtension
{
    /// <summary>
    /// Creates a Dictionary of the AnalyticsParameters array
    /// </summary>
    /// <param name="analyticParameters"></param>
    /// <returns></returns>
    public static Dictionary<string, object> GetAsKeyValuePairs(this AnalyticParameter[] analyticParameters)
    {
        Dictionary<string, object> keyValuePairs = new Dictionary<string, object>();
        foreach (var item in analyticParameters)
        {
            if (keyValuePairs.ContainsKey(item.parameterName))
            {
                Debug.LogError("duplicate key found! " + item.parameterName);
                continue;
            }

            keyValuePairs.Add(item.parameterName, item.parameter);
        }

        return keyValuePairs;
    }
}

public interface IAnalyticsService
{
    /// <summary>
    /// Sends a Event with data, for analytics
    /// </summary>
    /// <param name="EventName"></param>
    /// <param name="parameters"></param>
    void SendAnalytics(string EventName, AnalyticParameter[] parameters = null);
    /// <summary>
    /// Setup needed for Service
    /// </summary>
    void Initialize();

}
