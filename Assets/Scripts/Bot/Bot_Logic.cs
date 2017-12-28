using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit {
    public class Bot_Logic : MonoBehaviour {
        private Unit_Main mainUnit;
        private Transform tm;

        private void Awake() {
            tm = transform;
            mainUnit = GetComponent<Unit_Main>();
            mainUnit.StartMove += StartMove;
            mainUnit.EndMove += EndMove;
            mainUnit.DefatAction += Defat;
        }

        private void Start() {
        }
        public void Defat() {
            var points = mainUnit.GameController.GetAllPointsOfUnit(tm);
            Debug.Log(points.Count);
            if (points.Count > 0) {
                float valueOfAll = 0;
                for (int i = 0; i < points.Count; i++) {
                    valueOfAll += (points[i].Base.iActualCost / 2);
                }
                if (mainUnit.info.iMoney + valueOfAll > -500) {
                    while (mainUnit.info.iMoney < 0 && points.Count > 0) {
                        var rand = Random.Range(0, points.Count - 1);
                        var point = points[rand >= points.Count ? points.Count - 1 : rand];
                        mainUnit.info.iMoney += point.Base.iActualCost / 2;
                        point.RestoreBackup();
                        points.Remove(point);
                    }
                    Debug.Log(mainUnit.info.iMoney + " | " + mainUnit.sName);
                }
            }
        }
        public void StartMove() {
            Invoke("ThrowBones", Random.Range(10, 50) / 60);
        }
        public void EndMove() {
            CheckToBuyPoint();
            Invoke("GiveNextTour", .3f);
        }
        public void GiveNextTour() {
            mainUnit.GameController.NextRound();
        }
        public void CheckToBuyPoint() {
            int rand = mainUnit.GameController.randomGenerator.Next(10, 500);
            if (rand < 100) {
                var point = mainUnit.GameController.PointsController.GetPointAt(tm.position);
                if (point.data.CheckOwn(tm) && point.data.CanBuy(mainUnit.info.iMoney)) {
                    point.data.tmOwner = tm;
                    mainUnit.UpdateMoney(-point.data.iActualCost).GameController.UpdateTexts();
                    point.transform.Find("Box").GetComponent<Renderer>().material = GetComponent<Unit_Outfit>().body.Chest.GetComponent<Renderer>().material;
                    point.draw.SetTexts(
                        point.data.sName,
                        point.data.iActualCost.ToString(),
                        point.data.iActualProfit.ToString(),
                        (point.data.iActualLvL + 1).ToString(),
                        mainUnit.sName
                        );
                } else if (point.data.CheckOwn(tm) && point.data.CanUpgrade(mainUnit.info.iMoney)) {
                    mainUnit.UpdateMoney(-point.data.iActualCost).GameController.UpdateTexts();
                    point.data.Upgrade();
                    point.draw.SetTexts(
                        point.data.sName,
                        point.data.iActualCost.ToString(),
                        point.data.iActualProfit.ToString(),
                        (point.data.iActualLvL + 1).ToString(),
                        mainUnit.sName
                        );
                }
            }
        }
        public void ThrowBones() {
            var first = mainUnit.GameController.BonesController.Throw;
            var second = mainUnit.GameController.BonesController.Throw;
            //            Debug.Log(val + mainUnit.sName);
            mainUnit.GameController.BonesController.SetText("Wyrzuciłeś: " + first + " + " + second);
            mainUnit.GameController.MoveActualUnit(first + second);
        }
    }
}