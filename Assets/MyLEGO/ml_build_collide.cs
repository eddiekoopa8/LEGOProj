using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// STRUCT CLASS FOR MyLEGOBuild
public class ml_build_collide : MonoBehaviour
{

    static int STT_BOUNCE = 0;
    static int STT_BUILD = 1;
    static int STT_INACTIVE = 2;

    int state = STT_BOUNCE;

    public Rigidbody hRigidbody { get { return body; } }

    // building
    Vector3 oldPos;
    Quaternion oldRot;

    // bounce
    /*public */static float BounceTimer = 50;
    /*public */float BounceTick = 0;
    public bool Bouncing { get { return state == STT_BOUNCE; } }
    public bool Building { get { return state == STT_BUILD; } }
    public bool Inactive { get { return state == STT_INACTIVE; } }

    // collision listening
    public bool Touching = false;
    private new BoxCollider collider;
    private Rigidbody body;
    public GameObject Object = null;

    public void Start()
    {
        state = STT_BOUNCE;

        // building
        oldPos = gameObject.transform.position;
        oldRot = gameObject.transform.rotation;

        // collision listening
        Touching = false;
        Object = null;
        collider = gameObject.AddComponent<BoxCollider>();
        body = gameObject.AddComponent<Rigidbody>();
        Debug.Assert(collider != null, "FUCK!!! collider failed to be born. attempting to get existing");
        Debug.Assert(body != null, "FUCK!!! body failed to be born. attempting to get existing");
        if (collider == null)
        {
            collider = gameObject.GetComponent<BoxCollider>();
        }
        if (body == null)
        {
            body = gameObject.GetComponent<Rigidbody>();
        }
        body.isKinematic = true;
        body.angularDrag = 0.1f;
        body.mass = 500f;

        DoBounce();
    }

    // states
    public void DoBounce()
    {
        body.isKinematic = false;
        state = STT_BOUNCE;
    }
    public void DoBuild()
    {
        state = STT_BUILD;
    }

    // bounce
    public void DoBounceTick()
    {
        if (!Bouncing)
        {
            return;
        }
        BounceTick += Time.deltaTime * 100;
    }
    public void ResetBounceTick()
    {
        if (!Bouncing)
        {
            return;
        }
        BounceTick = 0;
    }
    public bool CanBounce()
    {
        if (!Bouncing)
        {
            return false;
        }
        return BounceTick >= BounceTimer && body.velocity.y <= 1 && body.velocity.y >= -1;
    }

    // building
    public void BuildGoto()
    {
        body.transform.position = Vector3.MoveTowards(body.transform.position, oldPos, 0.5f);
        body.transform.rotation = oldRot;
    }

    public bool Built()
    {
        return Equals(oldPos, body.transform.position);
    }

    // done!
    public void SetInactive()
    {
        Destroy(body);
        state = STT_INACTIVE;
    }

    // main logic
    public void Update()
    {
        collider.enabled = Bouncing;
        body.isKinematic = !Bouncing;
    }

    // collision listening
    public void OnCollisionStay(Collision collision)
    {
        Touching = true;
        Object = collision.gameObject;
    }

    public void OnCollisionExit(Collision collision)
    {
        Touching = false;
        Object = null;
    }
}
