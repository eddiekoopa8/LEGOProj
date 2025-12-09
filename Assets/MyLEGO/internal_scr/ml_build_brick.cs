using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/*
 * THIS CODE IS FOR EDDIE HILL TO HANDLE
 * IF YOU ARE NOT EDDIE, LEAVE THIS ALONE !
 */

public class ml_build_brick : MonoBehaviour
{

    static int STT_BOUNCE = 0;
    static int STT_BUILD = 1;
    static int STT_INACTIVE = 2;

    static float MOVE_SPEED = 3;

    int state = STT_BOUNCE;

    bool playBuildSound = false;

    public Rigidbody hRigidbody { get { return body; } }
    public BoxCollider hBoxCollider { get { return collider; } }

    // building
    Vector3 oldPosOff;
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
    public void Awake()
    {
        state = STT_BOUNCE;

        // building
        oldPos = gameObject.transform.position;
        oldRot = gameObject.transform.rotation;
    }

    AudioSource snd;
    public void Start()
    {
        snd = gameObject.AddComponent<AudioSource>();
        snd.clip = Resources.Load(ml_globals.AUDIO + "build") as AudioClip;
        snd.playOnAwake = false;

        Touching = false;
        Object = null;
        collider = gameObject.AddComponent<BoxCollider>();
        body = gameObject.AddComponent<Rigidbody>();
        Debug.Assert(collider != null, "collider failed to be born. attempting to get existing");
        Debug.Assert(body != null, "body failed to be born. attempting to get existing");
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

        DoBounceLoop();
    }

    public void OffsetDest(Vector3 off)
    {
        oldPos += off;
    }
    public void DoLoop()
    {
        collider.enabled = Bouncing;
        body.isKinematic = !Bouncing;
        body.useGravity = Bouncing;
    }
    // states
    public void DoBounceLoop()
    {
        state = STT_BOUNCE;

        DoLoop();
    }
    public void DoBuildLoop()
    {
        state = STT_BUILD;

        DoLoop();

        if (!body.isKinematic) body.velocity = Vector3.zero;
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
    public void BuildGoto(double ass)
    {
        playBuildSound = false;
        double newSpeed = ass / 10.0;
        //Debug.Log("newSpeed:" + newSpeed);
        if (ass < 0)
        {
            // Instant
            body.transform.position = oldPos;
        }
        else
        {
            // Movement
            body.transform.position = Vector3.MoveTowards(body.transform.position, oldPos, (float)newSpeed);
        }
        //body.transform.position = oldPos;
        body.transform.rotation = oldRot;
    }

    public void BuildSound()
    {
        if (!playBuildSound)
        {
            //Debug.Log("playBuildSound");
            snd.Play();
            playBuildSound = true;
        }
    }

    public void PostBuildGoto()
    {
        body.transform.position = oldPos;
        body.transform.rotation = oldRot;
    }

    public bool Built()
    {
        return (Mathf.Abs(oldPos.sqrMagnitude - body.transform.position.sqrMagnitude) < 0.01);
    }

    // done!
    public void SetInactive()
    {
        if (body != null)
        {
            Destroy(body);
        }
        state = STT_INACTIVE;
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
