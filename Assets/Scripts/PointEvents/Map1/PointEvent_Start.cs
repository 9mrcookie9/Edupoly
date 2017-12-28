using System;
using System.Collections;
using System.Collections.Generic;
using Main;
using Unit;
using UnityEngine;

namespace Points {
    public class PointEvent_Start: PointEvent {
        public long iMoneyOnExit = 400;
        public override void OnPlayerEnter(Main_Controller GameController, UnitData unit) {
            GameController.EventDravController.AddEvent("Wchodzisz na start, otrzymujesz: " + iMoneyOnExit, 1.5f);
            unit.Main.UpdateMoney(iMoneyOnExit);
        }

        public override void OnPlayerExit(Main_Controller GameController, UnitData unit) {

        }

    }
}