using System;
using System.Collections;
using System.Collections.Generic;
using Main;
using Unit;
using UnityEngine;

namespace Points {
    public class PointMoneyDecrease : PointEvent {
        public long[] iMoneyChangeValueRange = { 400, 800 };

        public override void OnPlayerEndRoundAtPoint(Main_Controller GameController, UnitData Unit) {
            int dec = ((int)UnityEngine.Random.Range(iMoneyChangeValueRange[0] / 10, iMoneyChangeValueRange[1] / 10)) * 10;
            GameController.EventDravController.AddEvent("Utracono: " + dec + "$", 1.5f);
            Unit.Main.UpdateMoney(-dec);
        }
    }
}