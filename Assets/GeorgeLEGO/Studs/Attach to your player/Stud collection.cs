using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteInEditMode]
#endif
public class StudCollection : MonoBehaviour {
    [HideInInspector]
    [SerializeReference]
    GameObject studCollectionSphereGameObject;
    public static GameObject Player;
    #region Editor things
#if UNITY_EDITOR
    [SerializeReference, HideInInspector]
    GameObject thisGameObject;
    [SerializeField]
    float studCollectionRadius = 1;
    [SerializeField]
    Vector3 studCollectionRadiusOffset;
    [SerializeField]
    Color sphereColour;
    [SerializeField]
    bool showSphere;
    public void CheckOrMakeNewSphereCollider() {
        if (transform.Find("Stud collection sphere") != null) {
            studCollectionSphereGameObject = transform.Find("Stud collection sphere").gameObject;
            Debug.Log("1");
            if (studCollectionSphereGameObject.transform.localPosition == Vector3.zero /*&& studCollectionSphereGameObject.hideFlags == HideFlags.HideInHierarchy/**/) {
                return;
            }
            Debug.Log("Please don't try to remake the object");
        }
        studCollectionSphereGameObject = new GameObject();
        studCollectionSphereGameObject.transform.SetParent(transform);
        studCollectionSphereGameObject.name = "Stud collection sphere";
        studCollectionSphereGameObject.transform.localPosition = Vector3.zero;
        studCollectionSphereGameObject.hideFlags = HideFlags.HideInHierarchy;
        if (studCollectionSphereGameObject.transform.GetComponent<StudDetecionSphere>() == null) {
            studCollectionSphereGameObject.AddComponent<StudDetecionSphere>();
        }
    }
    void Reset() {
        if (Application.isPlaying)
            return;
        thisGameObject = gameObject;
        CheckOrMakeNewSphereCollider();
        InitializeAttatchedCollider();

        if (FindObjectOfType<StudManagerScript>() == null) {
            new GameObject().AddComponent<StudManagerScript>().gameObject.name = "Stud Manager";
        }
    }
    void OnDestroy() {
        if (Application.isPlaying)
            return;
        try {
            try {
                DestroyImmediate(studCollectionSphereGameObject.gameObject);
            }
            catch { }
        }
        catch (NullReferenceException) {
            Debug.Log("You deleted the sphere at some point, please remember to delete whatever one you added if you added one" + "\n couldn't find sphere collider, please contact help");
        }
        //Debug.LogWarning("We recommend also removing the stud manager");
    }
#endif
    #endregion
    void Start() {

        if (Player == null)
            Player = gameObject;
        else
            new NotSupportedException("There can only be one gameobject that picks up studs", new NotImplementedException("If you want, you might be able to cannibalise some of my code to create that if you want"));
    }
    private void OnDrawGizmosSelected() {
        if (showSphere) {
            Gizmos.color = sphereColour;
            Gizmos.DrawWireSphere(transform.position + studCollectionRadiusOffset, studCollectionRadius);
            //Gizmo
        }
    }
    void InitializeAttatchedCollider() {
        try {
            studCollectionSphereGameObject.AddComponent<SphereCollider>();
            studCollectionSphereGameObject.GetComponent<SphereCollider>().isTrigger = true;
            studCollectionSphereGameObject.GetComponent<SphereCollider>().radius = studCollectionRadius;
            studCollectionSphereGameObject.GetComponent<SphereCollider>().center = studCollectionRadiusOffset;
        }
        catch (NullReferenceException) {
            InitializeAttatchedCollider();
        }
    }
    void RemoveStudCollectionObject() {
        try {
            Destroy(studCollectionSphereGameObject.GetComponent<SphereCollider>());
        }
        catch (NullReferenceException) {
            CheckOrMakeNewSphereCollider();
        }
    }

    private void OnApplicationQuit() {
        RemoveStudCollectionObject();
    }
}

[CustomEditor(typeof(StudCollection)), CanEditMultipleObjects]
public class StudCollectionEditor : Editor {
    bool showSphereFoldout = false;
    SphereCollider studCollectionsSphereGameObject;
    public override void OnInspectorGUI() {
        using (new EditorGUI.DisabledScope(true))
            EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MonoBehaviour)target), GetType(), false);

        EditorGUILayout.Space();
        RefindSphereCollider();
        SphereColliderSettingsFoldout();
    }
    void RefindSphereCollider() {
        try {
            if (serializedObject.FindProperty("studCollectionSphereGameObject") == null) {
                if (GUILayout.Button("refind Sphere collider")) {
                    ((StudCollection)target).CheckOrMakeNewSphereCollider();
                }
            }
        }
        catch (NullReferenceException) {
            if (GUILayout.Button("refind Sphere collider")) {
                ((StudCollection)target).CheckOrMakeNewSphereCollider();
            }
        }
    }
    void SphereColliderSettingsFoldout() {
        showSphereFoldout = EditorGUILayout.Foldout(showSphereFoldout, "Stud Collection Sphere settings");
        if (showSphereFoldout) {
            serializedObject.FindProperty("sphereColour").colorValue = EditorGUILayout.ColorField("Colour", serializedObject.FindProperty("sphereColour").colorValue);
            serializedObject.FindProperty("showSphere").boolValue = EditorGUILayout.Toggle("Show Stud Collection sphere", serializedObject.FindProperty("showSphere").boolValue);
            serializedObject.FindProperty("studCollectionRadius").floatValue = EditorGUILayout.FloatField("Radius", serializedObject.FindProperty("studCollectionRadius").floatValue);
            ((StudCollection)target).transform.Find("Stud collection sphere").GetComponent<SphereCollider>().radius = serializedObject.FindProperty("studCollectionRadius").floatValue;
            serializedObject.FindProperty("studCollectionRadiusOffset").vector3Value = EditorGUILayout.Vector3Field("studCollectionRadiusOffset", serializedObject.FindProperty("studCollectionRadiusOffset").vector3Value);
            ((StudCollection)target).transform.Find("Stud collection sphere").GetComponent<SphereCollider>().center = serializedObject.FindProperty("studCollectionRadiusOffset").vector3Value;
        }
        serializedObject.ApplyModifiedProperties();
    }
}
