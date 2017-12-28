using UnityEngine;
using System.Collections;

public class Background_Rotation : MonoBehaviour {

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
        if (tmTarget)
            if (tmTarget.rotation != transform.rotation)
                transform.rotation = new Quaternion(tmTarget.rotation.x, tmTarget.rotation.y, 0, tmTarget.rotation.w);
    }

}
