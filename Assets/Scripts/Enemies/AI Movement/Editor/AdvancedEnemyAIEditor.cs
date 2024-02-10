using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AdvancedEnemyAI))]
public class AdvancedEnemyAIEditor : BasicEnemyAIEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }

    protected override void InjectMiddle()
    {
        base.InjectMiddle();

        CombatSection();
    }

    protected override void ExtraAnimationSection()
    {
        var attackAnimationName = serializedObject.FindProperty("attackAnimationName");
        EditorGUILayout.PropertyField(attackAnimationName);
    }

    protected void CombatSection()
    {
        using (new EditorGUILayout.VerticalScope(EditorStyles.helpBox))
        {
            var attackDelay = serializedObject.FindProperty("attackDelay");
            var attackRadius = serializedObject.FindProperty("attackRadius");
            var attackStopDistance = serializedObject.FindProperty("attackStopDistance");
            var attackMoveSpeed = serializedObject.FindProperty("attackMoveSpeed");
            var useAttackVFX = serializedObject.FindProperty("useAttackVFX");
            var attackVFX = serializedObject.FindProperty("attackVFX");

            GUILayout.Space(TOP_PADDING);

            EditorGUILayout.LabelField("Combat", EditorStyles.boldLabel);

            GUILayout.Space(MIDDLE_PADDING);

            EditorGUILayout.PropertyField(attackDelay);
            EditorGUILayout.PropertyField(attackRadius);
            EditorGUILayout.PropertyField(attackStopDistance);
            EditorGUILayout.PropertyField(attackMoveSpeed);
            EditorGUILayout.PropertyField(useAttackVFX);

            if (useAttackVFX.boolValue)
                EditorGUILayout.PropertyField(attackVFX);

            GUILayout.Space(BOTTOM_PADDING);

            var attackEvents = serializedObject.FindProperty("attackEvents");
            EditorGUILayout.PropertyField(attackEvents);

            GUILayout.Space(TOP_PADDING);
        }
    }
}
