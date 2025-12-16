using System;
using Unity.LEGO.Minifig;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;



public class KillPlayer : MonoBehaviour {
    public enum OnPlayerDeath {
        restartScene = 1,
        goBackToLastSafeSpot = 2,
        changeScene = 3
    }
    [SerializeField, HideInInspector]
    public OnPlayerDeath onPlayerDeath = OnPlayerDeath.goBackToLastSafeSpot;
    [SerializeField, HideInInspector]
    string sceneToChangeTo;
    [SerializeField, HideInInspector]
    public bool killPlayer;
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
            KillPlayerFunc();
    }
    private void Update() {
        if (killPlayer)
            KillPlayerFunc();
    }
    public void KillPlayerFunc() {
        GameObject.FindWithTag("Player").GetComponent<MinifigController>().Explode();
        if (onPlayerDeath == OnPlayerDeath.goBackToLastSafeSpot)
            throw new NotImplementedException("add something in to the player to make ");
        else if (onPlayerDeath == OnPlayerDeath.changeScene)
            SceneManager.LoadScene(sceneToChangeTo);
        else if (onPlayerDeath == OnPlayerDeath.restartScene)
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else {
            throw new NullReferenceException(onPlayerDeath + "not set to an instance of an object");
        }
    }
}
