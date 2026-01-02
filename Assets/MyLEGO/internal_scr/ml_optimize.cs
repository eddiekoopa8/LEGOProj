using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ml_optimize : MonoBehaviour
{
    // Start is called before the first frame update
    public void OnBecameVisible()
    {
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = true;
        }
    }
    public void OnBecameInvisible()
    {
        if (GetComponent<MeshRenderer>())
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        if (GetComponent<SpriteRenderer>())
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        if (GetComponent<Animator>())
        {
            GetComponent<Animator>().enabled = false;
        }
    }
}
