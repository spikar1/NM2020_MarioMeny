using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    GameObject[] players;
    
    public void LoadScene(int sceneToLoad) {
        StartCoroutine(LoadNewScene(sceneToLoad));
        players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            players[i].GetComponent<Player>().ResetKnockback();
        }
    }

    IEnumerator LoadNewScene(int sceneIndex) {

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);

        while (!async.isDone) {
            yield return null;
        }    
    }
}
