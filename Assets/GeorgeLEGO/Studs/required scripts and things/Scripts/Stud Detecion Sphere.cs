using System;
using System.Collections;
using UnityEngine;

public class StudDetecionSphere : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Stud")) {
            try {
                other.transform.SetParent(StudManagerScript.studMovementCanvas.transform);
                other.GetComponent<Stud>().SendToBank();
                Debug.Log(other);
            }
            catch (Exception) {
                Debug.LogWarning("Please do not make any other objects with the Stud tag: remove from " + other.name + " or do not remove the stud component from the objects");
            }
        }
    }
}
