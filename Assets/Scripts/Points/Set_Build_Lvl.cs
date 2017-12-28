using UnityEngine;
using System.Collections;

[System.Serializable]
public class Set_Build_Lvl : MonoBehaviour {

    [SerializeField]
    public GameObject[] builds = new GameObject[5];

    public void Spawn(Transform tm, int lvl) {
        //  return;
        if (tm.childCount > 0)
            Destroy(tm.GetChild(0).gameObject);
        tm.localScale = new Vector3(.25f, .5f, .35f);
        GameObject Go = Instantiate(builds[lvl]);
        Go.transform.SetParent(tm);
        Go.transform.localPosition = Vector3.zero;
        Go.transform.localRotation = Quaternion.identity;
        Go.transform.localScale = new Vector3(15, 15, 20);
    }
}
