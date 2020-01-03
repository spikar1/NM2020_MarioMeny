using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int sceneToLoad = 1;


    IEnumerator LoadNewScene() {

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad);

        while (!async.isDone) {
            yield return null;
        }
    }
}
