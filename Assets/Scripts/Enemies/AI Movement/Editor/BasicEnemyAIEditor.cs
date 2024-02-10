using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BasicEnemyAI))]
public class BasicEnemyAIEditor : CustomEnemyAIEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected override void InjectMiddle()
    {
        var autoTargeting = serializedObject.FindProperty("autoTargeting");
        var target = serializedObject.FindProperty("target");
        var targetLayerMask = serializedObject.FindProperty("targetLayerMask");
        var targetRadius = serializedObject.FindProperty("targetRadius");

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Space(TOP_PADDING);

            EditorGUILayout.LabelField("Targeting", EditorStyles.boldLabel);

            GUILayout.Space(MIDDLE_PADDING);

            EditorGUILayout.PropertyField(autoTargeting);

            if (autoTargeting.boolValue)
            {
                EditorGUILayout.PropertyField(targetLayerMask);
                EditorGUILayout.PropertyField(targetRadius);
            }
            else
            {
                EditorGUILayout.PropertyField(target);
            }

            GUILayout.Space(BOTTOM_PADDING);
        }

        var moveSpeed = serializedObject.FindProperty("moveSpeed");
        var retreatMoveSpeed = serializedObject.FindProperty("retreatMoveSpeed");
        var movementSmoothing = serializedObject.FindProperty("movementSmoothing");

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            GUILayout.Space(TOP_PADDING);

            EditorGUILayout.LabelField("Movement", EditorStyles.boldLabel);

            GUILayout.Space(MIDDLE_PADDING);

            EditorGUILayout.PropertyField(moveSpeed);
            EditorGUILayout.PropertyField(retreatMoveSpeed);
            EditorGUILayout.PropertyField(movementSmoothing);

            GUILayout.Space(BOTTOM_PADDING);
        }

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            var flipSprite = serializedObject.FindProperty("flipSprite");
            var sprite = serializedObject.FindProperty("sprite");

            GUILayout.Space(TOP_PADDING);

            EditorGUILayout.LabelField("Sprite", EditorStyles.boldLabel);

            GUILayout.Space(MIDDLE_PADDING);

            EditorGUILayout.PropertyField(flipSprite);

            if (flipSprite.boolValue)
                EditorGUILayout.PropertyField(sprite);

            GUILayout.Space(BOTTOM_PADDING);
        }

        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            var useAnimation = serializedObject.FindProperty("useAnimation");
            var anim = serializedObject.FindProperty("anim");
            var movingBoolName = serializedObject.FindProperty("movingBoolName");

            GUILayout.Space(TOP_PADDING);

            EditorGUILayout.LabelField("Animation", EditorStyles.boldLabel);

            GUILayout.Space(MIDDLE_PADDING);

            EditorGUILayout.PropertyField(useAnimation);
            
            if (useAnimation.boolValue)
            {
                EditorGUILayout.PropertyField(anim, new GUIContent("Animator"));

                if (anim.objectReferenceValue != null)
                {
                    EditorGUILayout.PropertyField(movingBoolName);
                    ExtraAnimationSection();
                }
            }

            GUILayout.Space(BOTTOM_PADDING);
        }
    }

    protected virtual void ExtraAnimationSection() { }
}
