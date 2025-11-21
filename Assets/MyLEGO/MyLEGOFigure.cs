using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using UnityEditor.SceneManagement;
using UnityEngine;

public class MyLEGOFigure : MonoBehaviour
{
    bool Broken = false;

    public bool FigureBroken { get { return Broken; } }

    int randInt(int min, int max)
    {
        int myval = 0;
        System.Random rand = new System.Random();
        myval = (rand.Next()) % (max-min+1)  + min;
        return myval;
    }

    AudioSource snd;

    public void BreakIntoPieces(int breakIntensity = 2)
    {
        if (Broken)
        {
            return;
        }
        snd.Play();
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
            if (GetComponent<Collider>() != null) GetComponent<Collider>().enabled = true;

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

    public void DestroyMe()
    {
        DestroyBrokenPieces();
        Destroy(gameObject);
    }

    void Start()
    {
        snd = gameObject.AddComponent<AudioSource>();
        snd.clip = Resources.Load(ml_globals.AUDIO + "break") as AudioClip;
        snd.playOnAwake = false;
        foreach (Transform child in this.transform)
        {
            GameObject objChild = child.gameObject;
            if (objChild == null)
            {
                continue;
            }

            Debug.Log("[OBJ:\"" + this.name + "\"] will be using " + objChild.name);
        }
    }
    float deathtimer = 0;
    float flashtimer;
    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == gameObject.name && Input.GetMouseButtonDown(0))
            {
                BreakIntoPieces();
            }
        }

        if (Broken)
        {
            deathtimer += Time.deltaTime * 100;
            if (deathtimer >= 330)
            {
                flashtimer += Time.deltaTime * 100;
                foreach (Transform child in transform)
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
                DestroyMe();
            }
        }
    }
}
