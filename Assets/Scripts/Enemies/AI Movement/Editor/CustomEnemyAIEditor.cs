using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CustomEnemyAI))]
public class CustomEnemyAIEditor : Editor
{
    protected Texture2D headerTexture;
    protected Texture2D logo;

    protected Rect headerSection;

    protected CustomEnemyAI helper;

    protected const float TOP_PADDING = 6;
    protected const float MIDDLE_PADDING = 3;
    protected const float BOTTOM_PADDING = 10;

    void OnEnable()
    {
        InitTextures();
    }

    void InitTextures()
    {
        if (EditorGUIUtility.isProSkin == true)
            logo = Resources.Load<Texture2D>("Editor\\EnemyAILogo");
        else
            logo = Resources.Load<Texture2D>("Editor\\Pixelate Logo Dark");

        headerTexture = new Texture2D(1, 1);

        if (EditorGUIUtility.isProSkin == true)
            headerTexture.SetPixel(0, 0, new Color32(0, 0, 0, 255));
        else
            headerTexture.SetPixel(0, 0, new Color32(187, 187, 187, 255));

        headerTexture.Apply();
    }

    public override void OnInspectorGUI()
    {
        InitiateBanner();

        helper = (CustomEnemyAI)target;

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Space(TOP_PADDING);

            EditorGUILayout.LabelField("AI Settings", EditorStyles.boldLabel);

            GUILayout.Space(MIDDLE_PADDING);

            var drawDebugRays = serializedObject.FindProperty("drawDebugRays");
            var stopDistance = serializedObject.FindProperty("stopDistance");
            var collisionRadius = serializedObject.FindProperty("collisionRadius");

            EditorGUILayout.PropertyField(drawDebugRays);
            EditorGUILayout.PropertyField(stopDistance);
            EditorGUILayout.PropertyField(collisionRadius);

            GUILayout.Space(BOTTOM_PADDING);
        }

        InjectMiddle();

        DiscordButton();
        serializedObject.ApplyModifiedProperties();
    }

    public override bool UseDefaultMargins() => false;

    protected virtual void InjectMiddle() { }

    protected void DiscordButton()
    {
        if (GUILayout.Button("Join The Community Discord Server", GUILayout.Height(40)))
        {
            Application.OpenURL("https://discord.gg/ASkVNuet8K");
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void InitiateBanner()
    {
        headerSection.x = 0;
        headerSection.y = 0;
        headerSection.width = Screen.width;
        headerSection.height = 100;

        GUI.DrawTexture(headerSection, headerTexture);

        GUILayout.Space(12);
        GUILayout.FlexibleSpace();
        GUILayout.Label(logo, GUILayout.Width(250), GUILayout.Height(85));
        GUILayout.FlexibleSpace();
        GUILayout.Space(0);
    }
}
