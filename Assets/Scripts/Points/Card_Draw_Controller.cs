using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Card_Draw_Controller : MonoBehaviour {

    [SerializeField]
    Transform[] TmPos = new Transform[2];

    [SerializeField]
    Transform[] TmPosButton = new Transform[2];

    [SerializeField]
    Transform TmButton;


    [SerializeField]
    Transform TmCard;

    [SerializeField]
    Text[] txtCard = new Text[6]; // Name , cost , profit, lvl , owner , description

    public bool bShow;
    public bool bCardMoving;

    int id;

    private Main.Main_Controller main;
    private Points.Points_Controller points;

    private void Awake() {
        main = GetComponent<Main.Main_Controller>();
        points = GetComponent<Points.Points_Controller>();
    }

    void Start() {
        SetShowCard(bShow);
    }

    public void SetTextCard(int target = -1) {
        var unitTransform = main.actualUnit.unit.transform;
        var point = target >= 0 ? points.points[target] : points.GetPointAt(unitTransform.position);
        txtCard[0].text = point.data.sName;
        if (point.data.bCanBuy) {
            if (point.data.iActualLvL < point.data.iLvlMax || point.data.tmOwner == null)
                txtCard[1].text = "Koszt: " + point.data.iCost[point.data.iActualLvL];
            else
                txtCard[1].text = "Koszt: <color=#00ff00ff>Max</color>";
            txtCard[2].text = "Zysk: " + point.data.iProfit[point.data.iActualLvL];
            if (point.data.iActualLvL == point.data.iLvlMax)
                txtCard[3].text = "Poziom:  <color=#00ff00ff>Max</color>";
            else
                txtCard[3].text = "Poziom: " + (1 + point.data.iActualLvL) + "/" + (1 + point.data.iLvlMax);
            txtCard[4].text = "<color=green>" + point.data.sOwner + "</color>";
            txtCard[5].text = "Opis: <size=35>" + point.data.sDescription + "</size>";
            TmButton.position = TmPosButton[1].position;
            TmButton.GetComponentInChildren<Text>().text = "Zakup";
            if (point.data.tmOwner != unitTransform && point.data.tmOwner) {
                TmButton.GetComponent<Button>().interactable = false;
                if (point.data.tmOwner)
                    if (point.data.bMaxed) {
                        if (point.data.tmOwner == unitTransform)
                            TmButton.GetComponentInChildren<Text>().text = "Ulepsz";
                    }
            } else {
                TmButton.GetComponent<Button>().interactable = true;
                if (point.data.tmOwner)
                    if (point.data.tmOwner == unitTransform) {
                        TmButton.GetComponentInChildren<Text>().text = "Ulepsz";
                        if (unitTransform.GetComponent<Unit.Unit_Main>().info.iMoney <= point.data.iCost[point.data.iActualLvL] || unitTransform.GetComponent<Unit.Unit_Main>().info.iActionPoints <= 0) {
                            TmButton.GetComponent<Button>().interactable = false;
                            if (point.data.bMaxed)
                                TmButton.position = TmPosButton[0].position;
                        } else if (point.data.bMaxed)
                            TmButton.position = TmPosButton[0].position;

                    }
            }
        } else {
            TmButton.position = TmPosButton[0].position;
            txtCard[1].text = "<color=red>Nie można zakupić!</color>";
            for (int i = 2; i < 6; i++)
                txtCard[i].text = null;
            txtCard[2].text = "Opis: <size=35>" + point.data.sDescription + "</size>";
        }
        Canvas.ForceUpdateCanvases();
        var lastHeight = txtCard[5].rectTransform.sizeDelta.y;
        var lastY = txtCard[5].rectTransform.localPosition.y;
        var newSizeDelta = new Vector2(txtCard[5].rectTransform.sizeDelta.x, LayoutUtility.GetPreferredHeight(txtCard[5].rectTransform));
        if (lastHeight != newSizeDelta.y) {
            txtCard[5].rectTransform.sizeDelta = newSizeDelta;
            txtCard[5].rectTransform.localPosition = new Vector2(
                txtCard[5].rectTransform.localPosition.x,
                -(Mathf.Clamp(lastY, Mathf.Min(lastHeight, txtCard[5].rectTransform.sizeDelta.y), Mathf.Max(lastHeight, txtCard[5].rectTransform.sizeDelta.y))));
        }
        //Debug.Log(+ " vs " + txtCard[5].rectTransform.rect.height);
    }
    public void HideCard() {
        bShow = true;
        bCardMoving = false;
        TmCard.position = TmPos[1].position;
    }
    public void SetShowCard(bool status = false) {
        if (bCardMoving)
            return;

        if (status) {
            TmCard.position = TmPos[0].position;
            return;
        }
        if (main?.actualUnit?.unit.transform?.GetComponent<Unit.Unit_Type>().UnitType != Unit.Unit_Type.Type.Player) {
            TmCard.position = TmPos[1].position;
            return;
        }
        //   if (!playersController.TmActualPlayer.GetComponent<Player_Type>().Player)
        //    return;

        bShow = !bShow;
        if (bShow) {
            if (GetComponent<Menu_Controller>().bMenuA)
                if (!bCardMoving) {
                    bCardMoving = true;
                    id = 1;
                } else {
                    if (!bCardMoving) {
                        GetComponent<Menu_Controller>().ChangeMenuState(true);
                        bCardMoving = true;
                        id = 1;
                    }
                }
        } else {
            if (!bCardMoving) {
                bCardMoving = true;
                id = 0;
            }
        }

    }

    void Update() {
        if (bCardMoving) {
            TmCard.position = Vector3.MoveTowards(TmCard.position, TmPos[id].position, 2500 * Time.deltaTime);
            if (TmCard.position == TmPos[id].position || Vector3.Distance(TmCard.position, TmPos[id].position) < 0.01f) {
                TmCard.position = TmPos[id].position;
                bCardMoving = false;
            }
        }
    }
}
