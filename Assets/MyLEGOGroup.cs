using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MyLEGOGroup : MonoBehaviour
{
    public bool Broken = false;

    // int breakIntensity = 2;

    int randInt(int min, int max)
    {
        int myval = 0;
        System.Random rand = new System.Random();
        myval = (rand.Next()) % (max-min+1)  + min;
        return myval;
    }

    public void BreakIntoPieces(int breakIntensity = 2)
    {
        if (Broken)
        {
            return;
        }
        foreach (Transform child in this.transform)
        {
            GameObject objChild = child.gameObject;

            if (objChild.GetComponent<Rigidbody>() == null) objChild.AddComponent<Rigidbody>();
            if (objChild.GetComponent<BoxCollider>() == null) objChild.AddComponent<BoxCollider>();
            objChild.GetComponent<Rigidbody>().angularDrag = 0.1f;
            objChild.GetComponent<Rigidbody>().mass = 100f;

            utils.SetVelocityX(objChild.GetComponent<Rigidbody>(), randInt(-breakIntensity, breakIntensity));
            utils.SetVelocityY(objChild.GetComponent<Rigidbody>(), (float)(breakIntensity / 1.5));
            utils.SetVelocityZ(objChild.GetComponent<Rigidbody>(), randInt(-breakIntensity, breakIntensity));

            if (GetComponent<Rigidbody>() != null) GetComponent<Rigidbody>().isKinematic = true;
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = false;

            Broken = true;
            deathtimer = 0;
        }
    }

    public void DestroyBrokenPieces()
    {
        foreach (Transform child in this.transform)
        {
            GameObject objChild = child.gameObject;
            objChild.SetActive(false);
            Destroy(objChild);
        }
    }

    void Start()
    {
        foreach (Transform child in this.transform)
        {
            GameObject objChild = child.gameObject;
            if (objChild == null)
            {
                continue;
            }

            Debug.Log("[OBJ:\"" + this.name + "\"] will be used " + objChild.name);
        }
    }
    float deathtimer = 0;
    float flashtimer;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BreakIntoPieces();
        }

        if (Broken)
        {
            deathtimer += Time.deltaTime * 100;
            if (deathtimer >= 330)
            {
                flashtimer += Time.deltaTime * 100;
                foreach (Transform child in this.transform)
                {
                    GameObject objChild = child.gameObject;
                    if (flashtimer >= 10)
                    {
                        objChild.SetActive(!objChild.activeSelf);
                    }
                }
                if (flashtimer >= 10)
                {
                    flashtimer = 0;
                }
            }
            if (deathtimer >= 500)
            {
                DestroyBrokenPieces();
            }
        }
    }
}
