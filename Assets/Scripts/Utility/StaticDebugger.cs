using System.Collections.Generic;
using UnityEngine;
using System;

public enum cType { ENUM, OBJECT, SCRIPT, SCRIPTABLE_OBJECT, VARIABLE, PROPERY, METHOD }
public class StaticDebugger : MonoBehaviour
{
    // SECTION - variable =========================================================
    static public readonly string cEnum = "<color=#91ce22>";
    static public readonly string cObject = "<color=#f4c09a>";
    static public readonly string cComponent = "<color=#90f99e>";
    static public readonly string cScriptableObject = "<color=#60f0a7>";
    static public readonly string cVar = "<color=#4deae9>";
    static public readonly string cProperty = "<color=#1fabe0>";
    static public readonly string cMethod = "<color=#fff261>";


    // SECTION - Method =========================================================
    static public void SimpleDebugger(bool isDebugOn, string debugMessage)
    {
        if (isDebugOn)
            Debug.Log(debugMessage);
    }

    static public string Color(string text, cType colorType)
    {
        string color = "";

        switch (colorType)
        {
            case cType.ENUM:
                color = cEnum;
                break;
            case cType.OBJECT:
                color = cObject;
                break;
            case cType.SCRIPT:
                color = cComponent;
                break;
            case cType.SCRIPTABLE_OBJECT:
                color = cScriptableObject;
                break;
            case cType.VARIABLE:
                color = cVar;
                break;
            case cType.PROPERY:
                color = cProperty;
                break;
            case cType.METHOD:
                color = cMethod;
                break;
            default:
                break;
        }

        return color + text + "</color>";
    }

    static public string ColorByType(object obj) // TODO: Finish or delete
    {
        string color = "";
        System.Type type = obj.GetType();
        var controlTypes = UnityEditor.TypeCache.GetTypesDerivedFrom<MonoBehaviour>(); // Get array of types child of MonoBehaviour

        if (type.IsEnum)
            color = cEnum;
        else if (obj.Equals(typeof(GameObject)))
            color = cObject;
        else if (obj.Equals(typeof(MonoBehaviour)))
            color = cComponent;
        else if (obj.Equals(typeof(ScriptableObject)))
            color = cScriptableObject;
        else if (type.IsPrimitive)
            color = cVar;
        else
            color = cMethod;

        return color + obj.ToString() + "</color>";
    }

    public static void Log(string text)
    {
        List<string> list = new List<string>();

        string decoratedText = "";
        string copy = text;

        while (copy.Length > 0)
        {
            int colon = copy.IndexOf(':');
            int semiColon = copy.IndexOf(";");

            if (colon > 0)
            {
                decoratedText = copy.Substring(0, colon - 1);
            }

            string dEnd = "";

            switch (copy.Substring(colon, colon + 2).ToLower())
            {
                case "ce": decoratedText += cEnum; dEnd = "</color>"; break;
                case "co": decoratedText += cObject; dEnd = "</color>"; break;
                case "cc": decoratedText += cComponent; dEnd = "</color>"; break;
                case "cs": decoratedText += cScriptableObject; dEnd = "</color>"; break;
                case "cv": decoratedText += cVar; dEnd = "</color>"; break;
                case "cp": decoratedText += cProperty; dEnd = "</color>"; break;
                case "cm": decoratedText += cMethod; dEnd = "</color>"; break;
                default: break;
            }

            decoratedText += copy.Substring(colon+3, semiColon - 1);
            decoratedText += dEnd;

            copy.Remove(0, semiColon);
        }

        Debug.Log(decoratedText);
    }

    static public void ShowValues()
    {
        Debug.Log(
            $"{StaticDebugger.cObject} OBJECT </color> | " +
            $"{StaticDebugger.cEnum} ENUM </color> | " +
            $"{StaticDebugger.cComponent} SCRIPT </color> | " +
            $"{StaticDebugger.cScriptableObject} SCRIPTABLE </color> | " +
            $"{StaticDebugger.cVar} VAR </color> | " +
            $"{StaticDebugger.cProperty} PROPERTY </color> | " +
            $"{StaticDebugger.cMethod} METHOD </color> | "
            );
    }
}
