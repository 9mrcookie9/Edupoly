using UnityEngine;
using System.Collections;

public class Set_Camera_Pos : MonoBehaviour {
    private Transform tmTarget;

    public void GetPlayer(Unit.UnitData unit) {
        tmTarget = unit.transform;
    }

    private void Start() {
        SubscribeForPlayer();
    }

    public void SubscribeForPlayer() {
        var obj = FindObjectOfType<Main.Main_Controller>();
        if (!obj)
            Invoke("SubscribeForPlayer", .5f);
        else
            obj.SubscribeForUnit(GetPlayer);
    }

    void Update() {
        if (!tmTarget)
            return;
        transform.SetParent(tmTarget);
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }


}
