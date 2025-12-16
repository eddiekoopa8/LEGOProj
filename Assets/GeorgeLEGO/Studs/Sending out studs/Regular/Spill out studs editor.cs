using UnityEditor;
using Unity.LEGO.Behaviours.Actions;
using UnityEngine;

namespace Unity.LEGO.EditorExt {
    [CustomEditor(typeof(SpillOutStudsRegular), true)]
    public class SpillOutStudsRegularEditor : Editor {
        SerializedProperty m_CooldownTime;
        SerializedProperty m_TotalNumberOfTimesStudsSpill;
        SerializedProperty m_SilverStudTotal;
        SerializedProperty m_GoldStudTotal;
        SerializedProperty m_BlueStudTotal;
        SerializedProperty m_PurpleStudTotal;
        //SerializedProperty m_TotalNumberOfTimesStudsSpill;




        public void OnEnable() {

            m_CooldownTime = serializedObject.FindProperty("m_CooldownTime");
            m_TotalNumberOfTimesStudsSpill = serializedObject.FindProperty("m_totalNumberOfTimesStudsSpilled");
            m_SilverStudTotal = serializedObject.FindProperty("silverStudTotal");
            m_GoldStudTotal = serializedObject.FindProperty("goldStudTotal");
            m_BlueStudTotal = serializedObject.FindProperty("blueStudTotal");
            m_PurpleStudTotal = serializedObject.FindProperty("purpleStudTotal");
        }
        
        public override void OnInspectorGUI() {
            EditorGUILayout.PropertyField(m_CooldownTime);
            EditorGUILayout.PropertyField(m_TotalNumberOfTimesStudsSpill);
            EditorGUILayout.PropertyField(m_SilverStudTotal);
            EditorGUILayout.PropertyField(m_GoldStudTotal);
            EditorGUILayout.PropertyField(m_BlueStudTotal);
            EditorGUILayout.PropertyField(m_PurpleStudTotal);
            serializedObject.FindProperty("trigger").boolValue = GUILayout.Button("spill out all of the studs?");

            // Enable or disable CloseTime field based on AutoClose toggle
        }
    }
}
