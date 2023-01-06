using UnityEngine;
using UnityEditor;
using System.Linq;

// Note : Try switch debugger buttons into property drawer if time

[CustomEditor(typeof(PlayerInputController), true)]
public class PlayerInputControllerEditor : Editor
{
    // SECTION - Field =========================================================
    private PlayerInputController myTarget;

    private const int minColumnQty = 1;
    private const int maxColumnQty = 3;

    private const float minBtnWidth = 90.0f;
    private const float minBtnHeight = 30.0f;

    private float btnWidthAll;
    private float btnHeightAll;

    private float btnWidthSingle;
    private float btnHeightSingle;


    // SECTION - Method - Editor Specific =========================================================
    public void OnEnable()
    {
        myTarget = ((PlayerInputController)target);
        btnHeightAll = Screen.height * 0.025f; // No currentViewHeight available
        btnHeightSingle = Screen.height * 0.03f; // No currentViewHeight available

        myTarget.ColumnQty = 2; // Set base column quantity so that it is easier to read
    }

    public override void OnInspectorGUI()
    {
        // Get current graphView (inspector window)'s width
        // Allows for rect to scale with inspector resizing
        btnWidthAll = EditorGUIUtility.currentViewWidth * 0.3f;
        btnWidthSingle = EditorGUIUtility.currentViewWidth * 0.3f;


        // Column Qty -----------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Fill with empty space <=
        myTarget.ColumnQty = EditorGUILayout.IntField("Debuggers per Row", myTarget.ColumnQty);
        SetMinMaxColumnQty(minColumnQty, maxColumnQty);
        GUILayout.FlexibleSpace(); // Fill with empty space =>
        EditorGUILayout.EndHorizontal();

        // Button for ALL -----------------------------------------------------
        GUILayout.Space(10.0f);
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Fill with empty space <=

        // Set Debugger for ALL
        // Button w. ugly parameter layout because godline
        if
        (
            GUILayout.Button($"Debug ALL\n {myTarget.OnDebugAll}",
            GUILayout.Width(btnWidthAll),
            GUILayout.Height(btnHeightAll),
            GUILayout.MinWidth(minBtnWidth),
            GUILayout.MinHeight(minBtnHeight))
        )
            myTarget.SetAllDebuggers(myTarget.OnDebugAll);
        GUILayout.FlexibleSpace(); // Fill with empty space =>

        EditorGUILayout.EndHorizontal();

        // Individual Buttons -----------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace(); // Fill with empty space <=
        GUILayout.Space(10.0f);
        EditorGUILayout.BeginVertical();
        PrintButtonDebuggerAsRows(myTarget.ColumnQty); // BUTTONS
        EditorGUILayout.EndVertical();
        GUILayout.FlexibleSpace(); // Fill with empty space =>
        EditorGUILayout.EndHorizontal();


        GUILayout.Space(10.0f);

        base.OnInspectorGUI();
    }


    // SECTION - Method - Utility Specific =========================================================
    private void SetMinMaxColumnQty(int min, int max)
    {
        if (myTarget.ColumnQty < min)
            myTarget.ColumnQty = min;
        else if (myTarget.ColumnQty > max)
            myTarget.ColumnQty = max;
    }

    private void PrintButtonDebuggerAsRows(int maxColumn)
    {
        if (maxColumn >= 1) // To prevent bugs
        {
            int index = 0;

            int dicoCount = myTarget.debugDico.Count;

            int rowQty = (dicoCount % maxColumn == 0) ? dicoCount / maxColumn : (dicoCount / maxColumn) + 1;

            for (int row = 0; row < rowQty; row++)
            {
                EditorGUILayout.BeginHorizontal();
                    for (int column = 0; column < maxColumn; column++)
                    {
                        if (index < dicoCount)
                        {
                            string key = myTarget.debugDico.ElementAt(index).Key;
                            bool value = myTarget.debugDico.ElementAt(index).Value;

                            if
                            (
                            GUILayout.Button($"{key}\n {value}",
                            GUILayout.Width(btnWidthSingle),
                            GUILayout.Height(btnHeightSingle),
                            GUILayout.MinWidth(minBtnWidth),
                            GUILayout.MinHeight(minBtnHeight))
                            )
                                myTarget.debugDico[key] = !myTarget.debugDico[key];
                            index++;
                        }
                    }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
