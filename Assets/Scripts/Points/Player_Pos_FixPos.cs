using UnityEngine;
using System.Collections;

public class Player_Pos_FixPos : MonoBehaviour {

    [SerializeField]
    Transform TmPlayer;

    [SerializeField]
//    Points_Controller Points;

	public void SetPlayer(Transform player) {
        TmPlayer = player;
    }

    void Update() {
        if (TmPlayer) {
            //    SetParent(TmPlayer.GetComponent<Player_Movement>().iActualPos);
          //  transform.rotation = TmPlayer.rotation;
        }
    }

    public void SetParent(int pos) {
        transform.localPosition = Vector3.zero;
    }
}
