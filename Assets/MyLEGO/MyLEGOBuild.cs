using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

/*
 * THIS CODE IS FOR EDDIE HILL TO HANDLE
 * IF YOU ARE NOT EDDIE, LEAVE THIS ALONE !
 */

public class MyLEGOBuild : MonoBehaviour
{
    public bool StartBuildingAutomatically = false;
    public bool DebugMode = true;

    /// <summary>
    /// Change the value under 0 to play no animation
    /// </summary>
    public double BuildAnimationSpeed = 5;
    public bool EnableSound = true;
    public bool DoBounceAnimation = true;
    public Vector3 DestinationPosition = Vector3.zero;
    //bool printwarn = false;
    public int GetBrickCount()
    {
        return bricks.Count;
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

            if ((Input.GetKey(KeyCode.Space) && DebugMode) || StartBuildingAutomatically)
            {
                if (i <= buildIndex)
                {
                    brick.DoBuildLoop();
                }
            }
            else
            {
                brick.DoBounceLoop();
            }
        }

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

    public void EndBuild()
    {
        StartBuildingAutomatically = false;
    }
}
