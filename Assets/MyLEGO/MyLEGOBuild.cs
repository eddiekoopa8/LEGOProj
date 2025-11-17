using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;

public class MyLEGOBuild : MonoBehaviour
{
    //bool printwarn = false;
    public int GetBrickCount()
    {
        return bricks.Count;
    }

    List<ml_build_collide> bricks;
    // Start is called before the first frame update
    void Start()
    {
        bricks = new List<ml_build_collide>();

        foreach (Transform child in this.transform)
        {
            GameObject objChild = child.gameObject;

            ml_build_collide brick = null;

            if (objChild.GetComponent<ml_build_collide>() == null)
            {
                brick = objChild.AddComponent<ml_build_collide>();
            }
            else
            {
                brick = objChild.GetComponent<ml_build_collide>();
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

    bool bounceAfterComplete = false;

    void Update()
    {
        int buildIndex = 0;
        for (int i = 0; i < GetBrickCount(); i++)
        {
            ml_build_collide brick = bricks.ElementAt(i);

            if (brick.Inactive)
            {
                continue;
            }

            if (brick.Touching && brick.Bouncing)
            {
                brick.DoBounceTick();
                if (brick.CanBounce())
                {
                    utils.SetVelocityY(brick.hRigidbody, 5);
                    brick.ResetBounceTick();
                }
            }

            if (brick.Building)
            {
                brick.BuildGoto();
                if (brick.Built())
                {
                    brick.SetInactive();
                    buildIndex += buildIndex >= GetBrickCount() ? 0 : 1;
                }
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (i <= buildIndex)
                {
                    brick.DoBuild();
                }
            }
            else
            {
                brick.DoBounce();
            }

            brick.Update();
        }
    }
}
