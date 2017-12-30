using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TIMECONTROLLER : MonoBehaviour {

    [Range(0,100)]
    public float time;
    private void Update() {
        Time.timeScale = time;
    }
}
