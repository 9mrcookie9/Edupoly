using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Menu_Controller : MonoBehaviour {
    public Transform TmMenu;
    public Transform TmMenuA , TmMenuB;

    public bool bMenuA = false;
    public bool bMove;
    public float fSpeed;

    void Start() {
        Transform target = TmMenuB;
        if (bMenuA)
            target = TmMenuA;
        TmMenu.position = target.position;
    }

    public void ChangeMenuState(bool Hide = false) {
        if (!bMove) {
            StartCoroutine(ShowMenuAnimation());
            bMove = true;
        }
    }
    public void LoadScene(int scene) {
        SceneManager.LoadScene(scene);
    }

    public void Exit() {
        Application.Quit();
    }

    IEnumerator ShowMenuAnimation() {
        if (bMove)
            yield break;
        bMenuA = !bMenuA;
        Transform target = TmMenuB;
        if (bMenuA)
            target = TmMenuA;
        for (;;) {
            TmMenu.position = Vector3.MoveTowards(TmMenu.position,target.position,fSpeed * Time.deltaTime);
            if (TmMenu.position == target.position || Vector2.Distance(TmMenu.position,target.position) < 1) {
                bMove = false;
                break;
            } else
                yield return new WaitForSeconds(Time.deltaTime / 10);
        }
    }
}
