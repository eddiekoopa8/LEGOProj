using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyLEGOPlayer : MyLEGOFigure
{
    float speed = 10;
    Rigidbody body;
    bool turning = false;
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
            //body.rotation = Quaternion.Euler(0, 0, 0);
            ml_utils.SetVelocityX(body, transform.forward.x * (speed * (turning ? 0.5f : 1)));
            ml_utils.SetVelocityZ(body, transform.forward.z * (speed * (turning ? 0.5f : 1)));
        }
        else  if (Input.GetKey(KeyCode.DownArrow))
        {
            //body.rotation = Quaternion.Euler(0, 0, 0);
            ml_utils.SetVelocityX(body, transform.forward.x * -(speed * (turning ? 0.5f : 1)));
            ml_utils.SetVelocityZ(body, transform.forward.z * -(speed * (turning ? 0.5f : 1)));
        }
        /*else if (Input.GetKey(KeyCode.DownArrow))
        {
            body.rotation = Quaternion.Euler(0, 180, 0);
            ml_utils.SetVelocityX(body, transform.forward.x * speed);
            ml_utils.SetVelocityZ(body, transform.forward.z * speed);
        }*/
        else {
            ml_utils.SetVelocityX(body, 0);
            ml_utils.SetVelocityZ(body, 0);
            ml_utils.SetVelocityZ(body, 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.transform.eulerAngles += new Vector3(0,-4,0);
            turning = true;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            body.transform.eulerAngles += new Vector3(0,4,0);
            turning = true;
        }
        else
        {
            turning = false;
        }
    }
}
