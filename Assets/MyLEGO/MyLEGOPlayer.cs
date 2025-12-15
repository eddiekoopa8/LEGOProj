using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLEGOPlayer : MyLEGOFigure
{
    float speed = 10;
    Rigidbody body;
    void Start()
    {
        body = GetComponent<Rigidbody>();
        if (body == null)
        {
            Debug.Log("where is my soul?");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            body.rotation = Quaternion.Euler(0, 0, 0);
            ml_utils.SetVelocityX(body, transform.forward.x * speed);
            ml_utils.SetVelocityZ(body, transform.forward.z * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            body.rotation = Quaternion.Euler(0, 180, 0);
            ml_utils.SetVelocityX(body, transform.forward.x * speed);
            ml_utils.SetVelocityZ(body, transform.forward.z * speed);
        }

    }
}
