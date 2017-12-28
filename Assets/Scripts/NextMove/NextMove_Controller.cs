using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextMove_Controller : MonoBehaviour {
    public NextMove main;
    public Image Background;

    public RectTransform firstNumber, secondNumber, mathChar, sumNumbers;
    public RectTransform[] Numbers = new RectTransform[2], Symbols = new RectTransform[2];

    public NextMove_Val valToSet;
    public bool bNewData;
    public bool bCheckData;
    public int iChance;

    public Functions.CallBack.NoArgs OnPass, OnFail;
    public bool Show;

    void Start() {
        ChangeState();
        main.nextMove = this;
        ResetAll();
        main.SetData();
    }

    void Update() {
        if (bNewData) {
            bNewData = false;
            ResetAll();
            main.SetData();
        }
        if (bCheckData) {
            bCheckData = false;
            Check();
        }
    }

    public void SetStart(int chances) {
        iChance = chances;
        GetComponent<Canvas>().enabled = true;
        ResetAll();
        main.SetData();
    }

    public void ChangeState(bool status = false, bool force = false) {
        if (!force)
            Show = !Show;
        else
            Show = status;
        GetComponent<Canvas>().enabled = Show;
        ResetAll();
    }

    public void SetCallbacks(Functions.CallBack.NoArgs pass, Functions.CallBack.NoArgs fail) {
        OnPass += pass;
        OnFail += fail;
    }

    public void RemoveCallbacks(Functions.CallBack.NoArgs pass, Functions.CallBack.NoArgs fail) {
        OnPass -= pass;
        OnFail -= fail;
    }

    public void Check() {
        if (!firstNumber.GetComponent<NextMove_Val>().Empty && !secondNumber.GetComponent<NextMove_Val>().Empty && !mathChar.GetComponent<NextMove_Val>().Empty) {
            if (main.Check()) {
                Background.color = Color.green;
                ChangeState(false, true);
                main.SetNextMoveTestData(true);
                OnPass?.Invoke();
            } else {
                iChance--;
                Background.color = Color.red;
            }
        }
        if (iChance <= 0) {
            ChangeState(false, true);
            main.SetNextMoveTestData(false);
            OnFail?.Invoke();
        }
    }

    public void TryToChangeData(NextMove_Val val) {
        if (!val.Set) {
            if (valToSet) {
                valToSet.SetBackGroundColor(Color.white);
                valToSet = null;
            }
            valToSet = val;
            valToSet.SetBackGroundColor(Color.cyan);
        } else {
            if (!val.Static)
                if (valToSet)
                    if (val.Type == valToSet.Type) {
                        valToSet.SetBackGroundColor(Color.white);
                        val.SetData(valToSet);
                    }
        }
    }

    void ResetAll() {
        Background.color = new Color32(23, 23, 23, 246);
        firstNumber.GetComponent<NextMove_Val>().Reset();
        secondNumber.GetComponent<NextMove_Val>().Reset();
        mathChar.GetComponent<NextMove_Val>().Reset();
        sumNumbers.GetComponent<NextMove_Val>().Reset();
        for (int i = 0; i < Numbers.Length; i++)
            Numbers[i].GetComponent<NextMove_Val>().Reset();
        for (int i = 0; i < Symbols.Length; i++)
            Symbols[i].GetComponent<NextMove_Val>().Reset();
    }
}
