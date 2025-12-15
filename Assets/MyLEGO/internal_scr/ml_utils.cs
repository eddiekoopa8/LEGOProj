using System.Collections;
using System.Collections.Generic;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class ml_utils
{
    public static void SetVelocityX(Rigidbody body, float val)
    {
        var v = body.GetComponent<Rigidbody>().velocity;
        v.x = val;
        body.GetComponent<Rigidbody>().velocity = v;
    }
    public static void SetVelocityY(Rigidbody body, float val)
    {
        var v = body.GetComponent<Rigidbody>().velocity;
        v.y = val;
        body.GetComponent<Rigidbody>().velocity = v;
    }
    public static void SetVelocityZ(Rigidbody body, float val)
    {
        var v = body.GetComponent<Rigidbody>().velocity;
        v.z = val;
        body.GetComponent<Rigidbody>().velocity = v;
    }
    public static void SetRotateX(Rigidbody body, float val)
    {
        var v = body.GetComponent<Rigidbody>().rotation;
        body.GetComponent<Rigidbody>().rotation = Quaternion.FromToRotation(new Vector3(v.x, v.y, v.z), new Vector3(v.x + val, v.y, v.z));
    }
    public static void SetRotateY(Rigidbody body, float val)
    {
        var v = body.GetComponent<Rigidbody>().rotation;
        body.GetComponent<Rigidbody>().rotation = Quaternion.FromToRotation(new Vector3(v.x, v.y, v.z), new Vector3(v.x, v.y + val, v.z));
    }
    public static void SetRotateZ(Rigidbody body, float val)
    {
        var v = body.GetComponent<Rigidbody>().rotation;
        body.GetComponent<Rigidbody>().rotation = Quaternion.FromToRotation(new Vector3(v.x, v.y, v.z), new Vector3(v.x, v.y, v.z + val));
    }
}
