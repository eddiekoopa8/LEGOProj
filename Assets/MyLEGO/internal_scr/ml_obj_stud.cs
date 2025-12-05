using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ml_obj_stud : MonoBehaviour
{
    bool collected = false;
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
            Vector3 stopPos = GameObject.Find("ML_Camera_Stud_Stop").transform.position;
            transform.position = Vector3.MoveTowards(transform.position, stopPos, 0.4f);
            if (transform.position.Equals(stopPos))
            {
                Destroy(gameObject);
            }
        }
    }
}
