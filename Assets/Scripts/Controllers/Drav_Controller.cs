using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drav_Controller : MonoBehaviour {

    public Transform tmPlayerText;
    public Transform tmMoneyBalance;
    public Transform tmMoneyBankBalance;

    public Drav_Controller SetPlayerText(object text) {
        tmPlayerText?.GetComponent<Text>().SetText(text);
        return this;
    }
    public Drav_Controller SetBalanceText(object text) {
        tmMoneyBalance?.GetComponent<Text>().SetText(text);
        return this;
    }
    public Drav_Controller SetBankBalanceText(object text) {
        tmMoneyBankBalance?.GetComponent<Text>().SetText(text);
        return this;
    }
}
