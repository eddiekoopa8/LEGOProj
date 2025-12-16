using System;
using System.Collections;
using Unity.LEGO.Minifig;
using UnityEngine;
using UnityEngine.UI;

public class Stud : MonoBehaviour {
    public Vector2 randomoffsets;
    [SerializeField]
    int studValue;
    public int StudValue {
        get { return studValue; }
        set { }
    }
    float StudScale {
        get {
            try {
                return StudManagerScript.thisGameObject.GetComponent<StudManagerScript>().studScale;
            }
            catch {
                throw new NullReferenceException("Please remove and re-add the stud script to the player (as neccessary)");
            }
        }
        set { }
    }
    private void Reset() {
        transform.localScale = Vector3.one * StudScale;
    }
    private void Start() {
        if (Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hitInfo, 10))
            transform.position = hitInfo.point;
        StartCoroutine(EnableCollider());
    }
    IEnumerator EnableCollider() {
        yield return new WaitForSeconds(1);
        GetComponent<SphereCollider>().enabled = true;
    }
    bool collidedWith;
    public void SendToBank() {
        collidedWith = true;
        Vector3 WorldToScreenPoint = Camera.main.WorldToScreenPoint(transform.position + transform.GetChild(0).transform.localPosition / 2);
        transform.GetChild(0).gameObject.AddComponent<Image>();
        transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
        transform.position = WorldToScreenPoint;
        StudManagerScript.SendNewStudToBank(gameObject);
        transform.GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        transform.rotation = Quaternion.identity;
    }
    public void LerpTo(GameObject finalDestination) {
        StartCoroutine(LerpToCoroutine(finalDestination.transform.localPosition / 2 + finalDestination.transform.parent.localPosition));
    }
    IEnumerator LerpToCoroutine(Vector3 finalDestination) {
        float startingTime = Time.time;
        Vector3 startingPosition = transform.localPosition;
        float interpolationValue = 0;
        while ((finalDestination - (transform.localPosition + transform.GetChild(0).localPosition)).sqrMagnitude > 1) {
            interpolationValue = 1 / (1 + Mathf.Pow(3, -10 * (0.4f * (Time.time - startingTime) - 0.5f)));
            transform.localPosition = Vector3.Lerp(startingPosition, finalDestination - transform.GetChild(0).localPosition, interpolationValue);
            yield return null;
        }
        bool wait1Frame = false;
        try {
            StudManagerScript.thisGameObject.GetComponent<StudManagerScript>().AddStuds(studValue);
        }
        catch {
            wait1Frame = true;
        }
        if (wait1Frame) {
            yield return null;
            StudManagerScript.thisGameObject.GetComponent<StudManagerScript>().AddStuds(studValue);
        }
        Destroy(gameObject);
        yield return null;
    }

    void Update() {
        if (collidedWith) {
            transform.localScale = Vector3.one * StudScale;
            transform.rotation = Quaternion.identity;
            transform.GetChild(0).GetComponent<Image>().sprite = transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
            transform.GetChild(0).GetComponent<Image>().SetNativeSize();
        }
        else {
            transform.LookAt(Camera.main.transform);
            transform.localScale = Vector3.one * StudScale;
        }
    }//add image componenet, and set the sprite each frame once it's been collided with, change to a rect transform, use worldtoscreenpoint and voila?
    //lerp at speed =        1
    //                1+3^(-10(time-.5))
}
