using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class New_Game_Controler : MonoBehaviour {
    public Text TxtPlayers;
    public Text TxtBots;
    public Text TxtActions;
    public Text TxtChances;
    public Slider SlPlayer;
    public Slider SlBots;
    public Slider SlActions;
    public Slider SlChances;
    public Toggle TgTeachMode;
    public Text TxtMoney;

    public int[] iMoneyChange = new int[2] { 100, 500 };

    public int[] iMoneyLimits = new int[2] { 500, 20000 };

    public int iStartGameMoney = 5000;

    void Start() {
        SlPlayer.value = 1;
        SlBots.value = 3;
        TxtBots.text = SlBots.value.ToString();
        TxtPlayers.text = SlPlayer.value.ToString();
        TxtMoney.text = iStartGameMoney.ToString();
        DeductActions();
        DeductChances();
        CheckTeachMode();
    }

    public void DeductMoneyStart(int value) {
        if (value > 0) {
            if (iMoneyLimits[1] > iStartGameMoney)
                iStartGameMoney += iMoneyChange[SelectMoneyChangeValue()];
            TxtMoney.text = iStartGameMoney.ToString();
        } else {
            if (iMoneyLimits[0] < iStartGameMoney)
                iStartGameMoney -= iMoneyChange[SelectMoneyChangeValue()];
            TxtMoney.text = iStartGameMoney.ToString();
        }
    }

    public void DeductPlayerAndBots(int type) {
        if (type == 1) {
            if (SlBots.value + SlPlayer.value == SlPlayer.maxValue)
                SlPlayer.value = SlPlayer.maxValue - SlBots.value;
            if (SlBots.value > SlPlayer.maxValue - SlPlayer.value)
                SlBots.value = SlPlayer.maxValue - SlPlayer.value;
            TxtBots.text = SlBots.value.ToString();
            TxtPlayers.text = SlPlayer.value.ToString();
        } else {
            if (SlPlayer.value + SlBots.value > SlPlayer.maxValue)
                if (SlPlayer.value == SlPlayer.maxValue)
                    SlBots.value = 0;
                else if (SlBots.value + (SlPlayer.maxValue - SlPlayer.value) < SlBots.maxValue)
                    SlBots.value = SlPlayer.maxValue - SlPlayer.value;
            if (SlPlayer.value > SlBots.maxValue - SlBots.value)
                SlPlayer.value = SlPlayer.maxValue - SlBots.value;
            TxtBots.text = SlBots.value.ToString();
            TxtPlayers.text = SlPlayer.value.ToString();
        }
    }

    public void DeductActions() {
        TxtActions.text = SlActions.value.ToString();
    }

    public void DeductChances() {
        TxtChances.text = SlChances.value.ToString();
    }

    public void StartNewGame() {
        int teachMode = 0;
        if (TgTeachMode.isOn)
            teachMode = 1;
        SetData((int)SlPlayer.value, (int)SlBots.value, iStartGameMoney, (int)SlActions.value, teachMode, (int)SlChances.value);
        //    Debug.Log((int)SlPlayer.value + " " + (int)SlBots.value);
        Scenes.LoadScene(1);
    }

    public void CheckTeachMode() {
        if (TgTeachMode.isOn)
            SlChances.transform.localPosition = Vector2.zero;
        else
            SlChances.transform.localPosition = new Vector2(-99999, -99999);
    }

    public void SetData(int players, int bots, int money, int actions, int teachMode, int chance = 1) {
        PlayerPrefs.SetInt("iPlayersCount", players);
        PlayerPrefs.SetInt("iBotsCount", bots);
        PlayerPrefs.SetInt("iStartMoney", money);
        PlayerPrefs.SetInt("iActionsCount", actions);
        PlayerPrefs.SetInt("iTeachMode", teachMode);
        PlayerPrefs.SetInt("iChances", chance);
        PlayerPrefs.Save();
    }

    int SelectMoneyChangeValue() {
        if (iStartGameMoney >= 1500)
            return 1;
        else
            return 0;
    }
}
