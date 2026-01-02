using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nextlevel : MonoBehaviour
{
    public string SceneName;
    
    public void GotoNext()
    {
        SCENEManager.ChangeScene(SceneName);
    }
}
