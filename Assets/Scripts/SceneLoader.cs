using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void LoadScene(int sceneToLoad) {
        StartCoroutine(LoadNewScene(sceneToLoad));
    }

    IEnumerator LoadNewScene(int sceneIndex) {

        AsyncOperation async = SceneManager.LoadSceneAsync(sceneIndex);

        while (!async.isDone) {
            yield return null;
        }

        
    }
}
