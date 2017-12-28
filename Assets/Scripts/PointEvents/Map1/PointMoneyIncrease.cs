using System;
using System.Collections;
using System.Collections.Generic;
using Main;
using Unit;
using UnityEngine;

namespace Points {
    public class PointMoneyIncrease: PointEvent {
        public long[] iMoneyChangeValueRange = { 400, 800 };
        public override void OnPlayerEndRoundAtPoint(Main_Controller GameController, UnitData Unit) {
            if (Unit.Move.data.LastTarget != transform) {
                var point = Unit.Move.data.LastTarget.GetComponent<Point>();
                Debug.Log("ERROR: Rediction! " + name + " => " + point.name);
                Functions.Handle.OneArg go = (object a) => {
                    var p = a as PointEvent;
                    p?.OnPlayerEndRoundAtPoint(GameController, Unit);
                };
                if (point.bRandomEvent)
                    point.RunOnRandomPoints(go);
                else
                    point.RunOnEventPoints(go);
                return;
            } //Prevent from using bad point (Memory leak?)
            int dec = ((int)UnityEngine.Random.Range(iMoneyChangeValueRange[0] / 10, iMoneyChangeValueRange[1] / 10)) * 10;
            GameController.EventDravController.AddEvent("Otrzymano: " + dec + "$", 1.5f);
            Unit.Main.UpdateMoney(dec);
        }
    }
}
