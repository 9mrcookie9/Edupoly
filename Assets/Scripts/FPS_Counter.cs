using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPS_Counter : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(counter());
	}

    IEnumerator counter() {
        for (;;) {
            GetComponent<Text>().text = "FPS: " + (1/Time.deltaTime).ToString("00.00");
            yield return new WaitForSeconds(.1f);
        }
    }

}
