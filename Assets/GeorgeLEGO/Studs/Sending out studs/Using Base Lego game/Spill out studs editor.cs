using UnityEditor;
using Unity.LEGO.Behaviours.Actions;
using UnityEngine;

namespace Unity.LEGO.EditorExt {
    [CustomEditor(typeof(SpillOutStuds), true)]
    public class SpillOutStudsEditor : ActionEditor {
        SerializedProperty m_CooldownTime;
        SerializedProperty m_TotalNumberOfTimesStudsSpill;
        SerializedProperty m_SilverStudTotal;
        SerializedProperty m_GoldStudTotal;
        SerializedProperty m_BlueStudTotal;
        SerializedProperty m_PurpleStudTotal;
        SerializedProperty m_spillOutStudsPoint;
        //SerializedProperty m_TotalNumberOfTimesStudsSpill;




        protected override void OnEnable() {
            base.OnEnable();

            m_spillOutStudsPoint = serializedObject.FindProperty("StudSpillOutPoint");
            m_CooldownTime = serializedObject.FindProperty("m_CooldownTime");
            m_TotalNumberOfTimesStudsSpill = serializedObject.FindProperty("m_totalNumberOfTimesStudsSpilled");
            m_SilverStudTotal = serializedObject.FindProperty("silverStudTotal");
            m_GoldStudTotal = serializedObject.FindProperty("goldStudTotal");
            m_BlueStudTotal = serializedObject.FindProperty("blueStudTotal");
            m_PurpleStudTotal = serializedObject.FindProperty("purpleStudTotal");
        }

        protected override void CreateGUI() {
            EditorGUILayout.PropertyField(m_spillOutStudsPoint);
            EditorGUILayout.PropertyField(m_CooldownTime);
            EditorGUILayout.PropertyField(m_TotalNumberOfTimesStudsSpill);
            EditorGUILayout.PropertyField(m_SilverStudTotal);
            EditorGUILayout.PropertyField(m_GoldStudTotal);
            EditorGUILayout.PropertyField(m_BlueStudTotal);
            EditorGUILayout.PropertyField(m_PurpleStudTotal);
            if (Application.isPlaying)
                serializedObject.FindProperty("trigger").boolValue = GUILayout.Button("spill out all of the studs?");

            // Enable or disable CloseTime field based on AutoClose toggle
        }
    }
}
