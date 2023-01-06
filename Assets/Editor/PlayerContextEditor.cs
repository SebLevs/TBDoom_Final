using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerContext), true)]
public class PlayerContextEditor : Editor
{
    // SECTION - Field =========================================================
    private float btnWidth;
    private float btnHeight;

    private const float minBtnWidth = 120.0f;
    private const float minBtnHeight = 30.0f;


    // SECTION - Method =========================================================
    public void OnEnable()
    {
        btnHeight = Screen.height * 0.025f; // No currentViewHeight available
    }

    public override void OnInspectorGUI()
    {
        // Get current graphView (inspector window)'s width
        // Allows for rect to scale with inspector resizing
        btnWidth = EditorGUIUtility.currentViewWidth * 0.5f;

        GUILayout.Space(10.0f);

        EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace(); // Fill with empty space <=

            // Button w. ugly parameter layout because godline
            if 
            (
                GUILayout.Button($"Debugger : {((PlayerContext)target).IsDebugOn}", 
                GUILayout.Width(btnWidth), 
                GUILayout.Height(btnHeight),
                GUILayout.MinWidth(minBtnWidth),
                GUILayout.MinHeight(minBtnHeight))
            ) 
            ((PlayerContext)target).IsDebugOn = !((PlayerContext)target).IsDebugOn;

            GUILayout.FlexibleSpace(); // Fill with empty space =>
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10.0f);

        DrawDefaultInspector();
    }
}
