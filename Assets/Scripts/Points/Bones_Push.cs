using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Bones_Push: MonoBehaviour {

    [SerializeField]
    Text NumbersText;

    [SerializeField]
    public int[] iNumbersChanceNormalization = new int[6];

    public bool NormalBonesAutoIncrement;

    private Main.Main_Controller main;

    System.Random randomGenerator = new System.Random();

    public List<string> textToChangeString = new List<string>();
    public int Value;
    private void Awake() {
        main = GetComponent<Main.Main_Controller>();
        Random.InitState((int)System.DateTime.Now.Ticks);
    }
    public void SetText(object text) {
        textToChangeString.Add(text.ToString());
    }
    public Bones_Push RemoveAllTexts() {
        textToChangeString = new List<string>();
        return this;
    }
    private void Update() {
        if (NumbersText != null)
            if (textToChangeString.Count > 0) {
                if (NumbersText.text.Length > 0) {
                    for (int i = 0; i < Random.Range(6, 12); i++) {
                        if (NumbersText.text.Length > 0)
                            NumbersText.text = NumbersText.text.Remove(NumbersText.text.Length - 1);
                    }
                } else {
                    NumbersText.text = textToChangeString[0];
                    textToChangeString.RemoveAt(0);
                }
            }
    }
    private bool waitOnNextRoundExec = false;
    public void ButtonPress() {
        if (main.actualUnit.unit.transform.GetComponent<Unit.Unit_Type>().UnitType == Unit.Unit_Type.Type.Player && !waitOnNextRoundExec) {
            if (main.bWaitOnEnd) {
                waitOnNextRoundExec = true;
                Invoke("GiveNextTour", .3f);
            } else if (!main.actualUnit.unit.Move.data.bMove) {
                var val1 = Throw;
                var val2 = Throw;
                if (NumbersText != null)
                    SetText("Wyrzuciłeś: " + val1 + " + " + val2);
                main.MoveActualUnit(val1 + val2);
            }

        }
    }
    void GiveNextTour() {
        waitOnNextRoundExec = false;
        main.NextRound();
    }
    public int Throw {
        get {
            int value = 1;
            int random = randomGenerator.Next(iNumbersChanceNormalization[0], iNumbersChanceNormalization[iNumbersChanceNormalization.Length - 1]);
            random = Mathf.Abs(random);
            for (int i = 0; i < iNumbersChanceNormalization.Length; i++) {
                if (i + 1 < iNumbersChanceNormalization.Length)
                    if (iNumbersChanceNormalization[i] <= random && random < iNumbersChanceNormalization[i + 1]) {
                        if (NormalBonesAutoIncrement)
                            value = i + 1;
                        else
                            value = i;
                        //           Debug.Log(value + " " + random + " | " + iNumbersChanceNormalization[i] + "-" + iNumbersChanceNormalization[i + 1]);

                        break;
                    } else if (random > iNumbersChanceNormalization[iNumbersChanceNormalization.Length - 1]) {
                        //          Debug.Log(6 + " " + random + " | " + iNumbersChanceNormalization[i] + "-" + iNumbersChanceNormalization[i + 1]);
                        value = iNumbersChanceNormalization.Length;
                        break;
                    }

            }
            Value = value;
            return value;
        }
    }

    public bool MaxNumber(int number) {
        if (number == iNumbersChanceNormalization.Length - 1)
            return true;
        else
            return false;
    }
}
