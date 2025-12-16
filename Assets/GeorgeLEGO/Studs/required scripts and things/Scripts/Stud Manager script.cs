using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//ATTATCH TO A MANAGER GAMEOBJECT IN THE SCENE

public class StudManagerScript : MonoBehaviour {
    [DoNotSerialize]
    public static StudManagerScript thisGameObject;
    [SerializeField]
    public float studScale = 1/2;
    //AnimatorController stud2D;
    int studTotal;
    public static GameObject studMovementCanvas;
    public static void SendNewStudToBank(GameObject stud) {
        stud.GetComponent<Stud>().LerpTo(studMovementCanvas.transform.GetChild(0).GetChild(0).gameObject);
    }
    public static GameObject SilverStud;
    public static GameObject GoldStud;
    public static GameObject BlueStud;
    public static GameObject PurpleStud;
    private void Start() {
        if (thisGameObject == null) {
            thisGameObject = this;
            studMovementCanvas = new();
            studMovementCanvas.AddComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
            studMovementCanvas.name = "Canvas where studs move";
            studMovementCanvas.AddComponent<CanvasScaler>();
            studMovementCanvas.AddComponent<GraphicRaycaster>();
            studMovementCanvas.transform.SetParent(transform);
            Instantiate(Resources.Load<GameObject>("Stud counter prefab/Stud Counter")).transform.SetParent(studMovementCanvas.transform);
            SilverStud = Resources.Load<GameObject>("10 Silver");
            GoldStud = Resources.Load<GameObject>("100 Gold");
            BlueStud = Resources.Load<GameObject>("1000 Blue");
            PurpleStud = Resources.Load<GameObject>("10000 Purple");
        }
        else {
            Debug.LogWarning("Multiple studManager scripts detected: " + gameObject + " and " + thisGameObject);
        }
    }
    public void AddStuds(int studsToAdd) {
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<StudRotation2D>().Value = studsToAdd;
        transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = Convert.ToString(Convert.ToInt32(transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text) + studsToAdd);
        studTotal += studsToAdd;
    }
    private void Update() {
        try {
            studMovementCanvas.GetComponent<CanvasScaler>().scaleFactor = .5f;
        }
        catch(NullReferenceException e) {
            try {
                throw new NullReferenceException(studMovementCanvas + " " + e.Message);
            }
            catch {
                studMovementCanvas = transform.GetChild(0).gameObject;
                thisGameObject = this;
            }
        }
    }
}

