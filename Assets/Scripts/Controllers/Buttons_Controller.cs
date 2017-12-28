using UnityEngine;
using System.Collections;

namespace Main {
    public class Buttons_Controller: MonoBehaviour {
        Main_Controller main;
        private void Awake() {
            main = GetComponent<Main_Controller>();
        }


        #region BuyOrUpgrade
        public void Buy() {
            var mainUnit = main.actualUnit.unit;

            var tmPoint = mainUnit.Move?.data?.LastTarget ?? null;
            Points.Point point = null;
            if (tmPoint)
                if (tmPoint.GetComponent<Points.Point>())
                    point = tmPoint.GetComponent<Points.Point>();
            if (!point)
                return;
            if (mainUnit.transform.GetComponent<Unit.Unit_Type>().UnitType == Unit.Unit_Type.Type.Player && mainUnit.Main.info.iActionPoints > 0 && point.GetType() == typeof(Points.Point)) {
                if (point.data.bCanBuy && point.data.tmOwner == null && point.data.iActualCost <= mainUnit.Main.info.iMoney) {
                    mainUnit.Main.info.iActionPoints -= 1;
                    point.data.tmOwner = mainUnit.transform;
                    mainUnit.Main.UpdateMoney(-point.data.iActualCost).GameController.UpdateTexts();
                    point.transform.Find("Box").GetComponent<Renderer>().material = mainUnit.transform.GetComponent<Unit.Unit_Outfit>().body.Chest.GetComponent<Renderer>().material;
                    main.drawCardController.SetTextCard();
                    point.draw.SetTexts(
                        point.data.sName,
                        point.data.iActualCost.ToString(),
                        point.data.iActualProfit.ToString(),
                        (point.data.iActualLvL + 1).ToString(),
                        mainUnit.Main.sName
                        );
                    mainUnit.Main.GameController.drawCardController.SetTextCard();
                } else if (point.data.bCanUpgrade && !point.data.bMaxed && point.data.tmOwner == mainUnit.transform && point.data.iActualCost <= mainUnit.Main.info.iMoney) {
                    mainUnit.Main.info.iActionPoints -= 1;
                    mainUnit.Main.UpdateMoney(-point.data.iActualCost).GameController.UpdateTexts();
                    point.data.Upgrade();
                    main.drawCardController.SetTextCard();
                    point.draw.SetTexts(
                       point.data.sName,
                       point.data.iActualCost.ToString(),
                       point.data.iActualProfit.ToString(),
                       (point.data.iActualLvL + 1).ToString(),
                       mainUnit.Main.sName
                       );
                }
            }
        }
        #endregion
    }
}