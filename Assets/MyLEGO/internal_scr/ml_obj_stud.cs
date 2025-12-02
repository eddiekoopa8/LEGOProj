using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ml_obj_stud : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().transform.LookAt(Camera.main.transform);

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == gameObject.name && Input.GetMouseButtonDown(0))
            {
                Debug.Log("STUD CLICK !!!");
            }
        }
    }
}
