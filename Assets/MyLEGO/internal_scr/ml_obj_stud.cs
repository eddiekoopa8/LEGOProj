using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ml_obj_stud : MonoBehaviour
{
    bool collected = false;
    bool sound = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().transform.LookAt(Camera.main.transform);

        if (!collected)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == gameObject.name && Input.GetMouseButtonDown(0))
                {
                    Debug.Log("move !!!");
                    collected = true;
                }
            }
        }
        else
        {
            if (!sound) {
                GetComponent<AudioSource>().Play();
                sound = true;
            }
            GetComponent<Collider>().enabled = false;
            Vector3 stopPos = GameObject.Find("ML_Camera_Stud_Stop").transform.position;
            transform.position = Vector3.MoveTowards(transform.position, stopPos, Time.deltaTime * 10);
            if (transform.position.Equals(stopPos))
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.GetComponent<MyLEGOPlayer>() != null && !collected)
        {
            Debug.Log("COLECT");
            collected = true;
        }
    }
}
