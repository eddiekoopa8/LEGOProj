using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class MyLEGOBuild : MonoBehaviour
{
    public bool StartBuildingAutomatically = false;
    public bool DebugMode = false;
    public double BuildAnimationSpeed = 15;
    public bool EnableSound = true;
    public bool DoBounceAnimation = true;
    public bool RestartIfBuildIsUnfinished = true;
    public Transform DestinationPosition = null;
    //bool printwarn = false;
    public int GetBrickCount()
    {
        return bricks.Count;
    }

    Vector3 getOffPos()
    {
        Vector3 chosen;
        if (DestinationPosition == null)
        {
            chosen = transform.position;
        }
        else
        {
            chosen = DestinationPosition.position;
        }
        return chosen;
    }

    List<ml_build_brick> bricks;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(GetComponent<MyLEGOFigure>() == null, "STOP STOP STOP!!!! DO NOT COMBINE FIGURE AND BUILD!!!!");
        bricks = new List<ml_build_brick>();

        foreach (Transform child in this.transform)
        {
            GameObject objChild = child.gameObject;

            ml_build_brick brick = null;

            if (objChild.GetComponent<ml_build_brick>() == null)
            {
                brick = objChild.AddComponent<ml_build_brick>();
            }
            else
            {
                brick = objChild.GetComponent<ml_build_brick>();
            }

            if (brick == null)
            {
                continue;
            }

            brick.Start();
            brick.OffsetDest(getOffPos() - transform.position);

            bricks.Add(brick);
        }

        GetBrickCount();
    }

    bool built = false;

    void Update()
    {
        int buildIndex = 0;
        for (int i = 0; i < GetBrickCount() && !built; i++)
        {
            ml_build_brick brick = bricks.ElementAt(i);

            if (brick.Inactive)
            {
                continue;
            }

            if (brick.Touching && brick.Bouncing)
            {
                brick.DoBounceTick();
                if (brick.CanBounce())
                {
                    if (DoBounceAnimation) ml_utils.SetVelocityY(brick.hRigidbody, 5);
                    brick.ResetBounceTick();
                }
            }

            if (brick.Building)
            {
                if (brick.Built())
                {
                    buildIndex += buildIndex > GetBrickCount() ? 0 : 1;
                    if (EnableSound) brick.BuildSound();
                    brick.PostBuildGoto();
                }
                else
                {
                    brick.BuildGoto(BuildAnimationSpeed);
                }
            }

            if (StartBuildingAutomatically)
            {
                if (i <= buildIndex)
                {
                    brick.DoBuildLoop();
                }
            }
            else
            {
                if (brick.Built() && RestartIfBuildIsUnfinished)
                    brick.DoBounceLoop();
            }

            if (DebugMode)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    StartBuild();
                }
                else
                {
                    StopBuild();
                }
            }
        } // for

        if (buildIndex >= GetBrickCount() && !built)
        {
            for (int i = 0; i < GetBrickCount(); i++)
            {
                if (gameObject.GetComponent<Rigidbody>() == null)
                {
                    gameObject.AddComponent<Rigidbody>();
                    gameObject.GetComponent<Rigidbody>().mass = 1000;
                    gameObject.GetComponent<Rigidbody>().freezeRotation = true;
                }
                bricks.ElementAt(i).SetInactive();
            }
            ml_utils.SetVelocityY(gameObject.GetComponent<Rigidbody>(), 5);
            Debug.Log("done");
            built = true;
        }
    }

    public void StartBuild()
    {
        StartBuildingAutomatically = true;
    }

    public void StopBuild()
    {
        StartBuildingAutomatically = false;
    }
}
