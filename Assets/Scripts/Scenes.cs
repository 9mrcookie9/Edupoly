using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Scenes : MonoBehaviour {

	public static void Exit() {
        Application.Quit();
    }

    public static void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    public void Debugtest(string text) {
        Debug.Log(text);
    }
}
