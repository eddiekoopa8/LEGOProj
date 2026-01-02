using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class NextClickLevel : MonoBehaviour
{
    public MyLEGOBuild build;
    bool touch = false;
    public void Update()
    {
        if (touch) {
            SCENEManager.ChangeScene("Scenes/Level2");
        }
        else {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.name == gameObject.name && Input.GetMouseButtonDown(0))
                {
                    touch = true;
                }
            }
        }
    }
}
