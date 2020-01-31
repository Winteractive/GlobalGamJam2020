using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InspectorAnalyticsParameter
{

    public string parameterName;

    public enum ObjectType { Int, Bool, Double, String };
    public ObjectType _type;

    public string _string;
    public int _int;
    public bool _bool;
    public double _double;


    public object GetParameter()
    {
        switch (_type)
        {
            case ObjectType.Int:
                return _int;
               // break;
            case ObjectType.Bool:
                return _bool;
                //break;
            case ObjectType.Double:
                return _double;
                //break;
            case ObjectType.String:
                return _string;
                //break;
        }

        Debug.LogError("ERROR!");
        return _int;
    }

    public static explicit operator AnalyticParameter(InspectorAnalyticsParameter inspectorParameter)
    {
        AnalyticParameter a = new AnalyticParameter();

        a.parameterName = inspectorParameter.parameterName;

        switch (inspectorParameter._type)
        {
            case ObjectType.Int:
                a.parameter = inspectorParameter._int;
                break;
            case ObjectType.Bool:
                a.parameter = inspectorParameter._bool;
                break;
            case ObjectType.Double:
                a.parameter = inspectorParameter._double;
                break;
            case ObjectType.String:
                a.parameter = inspectorParameter._string;
                break;
            default:
                break;
        }

        return a;
    }


}
