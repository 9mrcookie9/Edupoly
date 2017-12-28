using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NextMove_Val : MonoBehaviour {
    public enum ThisType { Number, Symbol };
    public ThisType Type;

    public enum SymbolType { Inc = 0, Dec, Sum, NumberOfTypes };
    public SymbolType symbolType;

    public enum NumberType { Val, Sum };
    public NumberType numberType;

    public int Value = 0;
    public bool Empty = true;
    public bool Set = false;
    public bool Static = true;

    public void SetBackGroundColor(Color color) {
        Color toSetColor = new Color(color.r, color.g, color.b, 0.39f);
        transform.GetChild(0).GetComponent<Image>().color = toSetColor;
    }

    public void Reset() {
        Static = false;
        SetBackGroundColor(Color.white);
        GetComponent<Text>().text = "";
        Value = 0;
        Empty = true;
    }

    public void SetData(NextMove_Val val) {
        if (val.Type == ThisType.Number)
            SetData(val.Value, false);
        else
            SetData(val.symbolType, false);
    }

    public void SetData(SymbolType symboltype, bool bStatic = true) {
        if (bStatic)
            Static = true;
        symbolType = symboltype;
        Empty = false;
        if (SymbolType.Inc == symbolType)
            GetComponent<Text>().text = "+";
        else
            GetComponent<Text>().text = "-";

    }
    public void SetData(int val, bool bStatic = true) {
        if (bStatic)
            Static = true;
        Value = val;
        Empty = false;
        GetComponent<Text>().text = val.ToString();
    }

    public bool isNumber() {
        if (Type == ThisType.Number)
            return true;
        else
            return false;
    }

    public static SymbolType SymbolById(int id) {
        switch (id) {
            case 1:
                return SymbolType.Dec;
            default:
                return SymbolType.Inc;
        }
    }

    public static SymbolType RandomSymbol(SymbolType toTest) {
        if (SymbolType.Inc != toTest)
            return SymbolType.Inc;
        else
            return SymbolType.Dec;
    }

}
