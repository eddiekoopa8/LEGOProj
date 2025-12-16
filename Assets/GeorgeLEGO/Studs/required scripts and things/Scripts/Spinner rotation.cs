using System;
using UnityEngine;
using UnityEngine.UI;

public class StudRotation2D : MonoBehaviour {
    float startTime;
    [SerializeField]
    public int value = 10;
    public int Value {
        get {
            return value;
        }
        set {
            this.value = value;
            GetComponent<Animator>().SetInteger("lastValueAdded", value);
            transform.localScale = Vector3.one * 1f;
            startTime = Time.time;
        }
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
    void SizePop() {
        if (transform.localScale == Vector3.one * StudScale || (transform.localScale - Vector3.one* StudScale).sqrMagnitude < (0.002f * StudScale)) {
            transform.localScale = Vector3.one * StudScale;
            return;
        }
        transform.localScale = (Vector3.one * StudScale * 2) * (float)Math.Pow(0.01f, (Time.time - startTime)/2) + Vector3.one * StudScale;
    }
    void Update() {
        SizePop();
        GetComponent<Image>().sprite = GetComponent<SpriteRenderer>().sprite;
    }
    //remove image component
    //set the sprite to one of the ones from the stud sprite sheet
    //set the value in the animator
    //set the image.sprite property? (should work from what the internet says
}
