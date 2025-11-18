using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ml_audio : MonoBehaviour
{
    AudioSource data;
    // Start is called before the first frame update
    void Start()
    {
        data = gameObject.AddComponent<AudioSource>();
        data.clip = Resources.Load(name) as AudioClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
