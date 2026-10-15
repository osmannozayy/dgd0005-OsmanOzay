using UnityEngine;
using System;
using System.Collections.Generic;

public class ConnectionHud : MonoBehaviour
{
    public static List<Func<string>> Panels = new List<Func<string>>();
    private static string flashMessage = "";
    private static float flashTime = 0f;

    private void Awake()
    {
        Panels.Clear();
    }

    public static void Flash(string message)
    {
        flashMessage = message;
        flashTime = Time.time + 3f;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(10, 100, 400, 600));

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 24;
        style.normal.textColor = Color.white;

        foreach (Func<string> panel in Panels)
        {
            if (panel != null)
            {
                GUILayout.Label(panel.Invoke(), style);
            }
        }

        if (Time.time < flashTime)
        {
            style.normal.textColor = Color.yellow;
            GUILayout.Label(flashMessage, style);
        }

        GUILayout.EndArea();
    }
}