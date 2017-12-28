using UnityEngine;
using System.Collections;

public class Menu_Animation : MonoBehaviour {

    public Transform TmMain;
    public Transform TmNewGame;
    public Transform TmMid;
    public Transform TmLeft;
    public Transform TmRight;

    public int iSpeed;
    private int iDir = -1;

    private bool bMove;
    public bool bMoving;
    public void MoveNewGame(int dir) {
        if (bMoving)
            return;
        iDir = dir;
        bMove = true;
    }

    private void Update() {
        if (bMove) {
            bMove = false;
            bMoving = true;
        }
        if (bMoving) {
            if (iDir == 0) {
                if (Vector3.Distance(TmNewGame.position, TmRight.position) < 0.01f && Vector3.Distance(TmMain.position, TmMid.position) < 0.01f) {
                    TmMain.position = TmMid.position;
                    TmNewGame.position = TmRight.position;
                    bMoving = false;
                } else {
                    TmNewGame.position = Vector3.MoveTowards(TmNewGame.position, TmRight.position, iSpeed * Time.deltaTime);
                    TmMain.position = Vector3.MoveTowards(TmMain.position, TmMid.position, iSpeed * Time.deltaTime);
                }
            } else {
                if (Vector3.Distance(TmNewGame.position, TmMid.position) < 0.01f && Vector3.Distance(TmMain.position, TmLeft.position) < 0.01f) {
                    TmMain.position = TmLeft.position;
                    TmNewGame.position = TmMid.position;
                    bMoving = false;
                } else {
                    TmNewGame.position = Vector3.MoveTowards(TmNewGame.position, TmMid.position, iSpeed * Time.deltaTime);
                    TmMain.position = Vector3.MoveTowards(TmMain.position, TmLeft.position, iSpeed * Time.deltaTime);
                }
            }
        }
    }

}
