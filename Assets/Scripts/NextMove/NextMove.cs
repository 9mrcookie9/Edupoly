using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextMove : MonoBehaviour {

    public NextMove_Controller nextMove;
    public int iFirstNumber, iSecondNumber, iSumNumber;
    public bool bFirstNumber;

    NextMove_Val.SymbolType symbolType;

    public void ShowNextMoveTest(int chances) {
        nextMove.SetStart(chances);
    }

    public void SetNextMoveTestData(bool resault) {

    }

    public void SetData() {
        if (!nextMove)
            return;
        if (Random.Range(0,10) >= 5)
            symbolType = NextMove_Val.SymbolType.Dec;
        else
            symbolType = NextMove_Val.SymbolType.Inc;

        iFirstNumber = Random.Range(1,9);
        iSecondNumber = Random.Range(1,9);
        if (symbolType == NextMove_Val.SymbolType.Dec)
            iSumNumber = iFirstNumber - iSecondNumber;
        else
            iSumNumber = iFirstNumber + iSecondNumber;

        if (Random.Range(0,10) >= 5)
            bFirstNumber = true;
        else
            bFirstNumber = false;
        int toSet = 0;
        if (bFirstNumber)
            toSet = iFirstNumber;
        else
            toSet = iSecondNumber;

        int falseToSet = 0;
        do {
            falseToSet = Random.Range(1,9);
            if (falseToSet == 0 || toSet == 0 || falseToSet < 9 || toSet < 9)
                break;
        } while (falseToSet == toSet);

        if (bFirstNumber)
            nextMove.secondNumber.GetComponent<NextMove_Val>().SetData(iSecondNumber);
        else
            nextMove.firstNumber.GetComponent<NextMove_Val>().SetData(iFirstNumber);
        nextMove.sumNumbers.GetComponent<NextMove_Val>().SetData(iSumNumber);

        int trueNumber = Random.Range(0,nextMove.Numbers.Length - 1);

        for (int i = 0; i < nextMove.Numbers.Length; i++)
            if (i == trueNumber)
                nextMove.Numbers[i].GetComponent<NextMove_Val>().SetData(toSet);
            else
                nextMove.Numbers[i].GetComponent<NextMove_Val>().SetData(ReturnDiferentRandom(toSet,1,9));

        for (int i = 0; i < nextMove.Numbers.Length; i++)
            if (i == trueNumber)
                nextMove.Symbols[i].GetComponent<NextMove_Val>().SetData(symbolType);
            else
                nextMove.Symbols[i].GetComponent<NextMove_Val>().SetData(NextMove_Val.RandomSymbol(symbolType));
    }

    public bool Check() {
        bool toReturn = false;
        if(nextMove.mathChar.GetComponent<NextMove_Val>().symbolType == NextMove_Val.SymbolType.Inc) {
            if (nextMove.firstNumber.GetComponent<NextMove_Val>().Value +
               nextMove.secondNumber.GetComponent<NextMove_Val>().Value ==
               nextMove.sumNumbers.GetComponent<NextMove_Val>().Value)
                toReturn = true;
        } else {
            if (nextMove.firstNumber.GetComponent<NextMove_Val>().Value -
               nextMove.secondNumber.GetComponent<NextMove_Val>().Value ==
               nextMove.sumNumbers.GetComponent<NextMove_Val>().Value)
                toReturn = true;
        }
        return toReturn;
    }

    int ReturnDiferentRandom(int number,int min,int max) {
        for (;;) {
            int toReturn = Random.Range(min,max);
            if (toReturn != number)
                return toReturn;
        }
    }
}
