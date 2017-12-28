using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMECONTROLLER : MonoBehaviour {

    public float time;
    private void Update() {
        Time.timeScale = time;
    }
}
