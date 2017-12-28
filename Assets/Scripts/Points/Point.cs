using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Main;

namespace Points {
    [System.Serializable]
    public class PointData {
        public string sActualCost { get { return (iCost.Count > iActualLvL) ? iCost[iActualLvL].ToString() : ""; } }
        public string sActualProfit { get { return (iProfit.Count > iActualLvL) ? iProfit[iActualLvL].ToString() : ""; } }
        public string sOwner { get { return (tmOwner) ? tmOwner.name : "Wolne"; } }
        public string sName = "Name";
        public List<int> iCost = new List<int>{ 500, 1000, 1500, 2000, 2500 };
        public List<int> iProfit = new List<int>{ 150, 400, 900, 1500, 2250 };
        public int iLvlMax = 1;
        public int iActualLvL = 0;
        public int ID;
        public bool bCanUpgrade = true;
        public bool bCanBuy = true;
        public string sDescription = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus non pulvinar ex. Nunc feugiat arcu sed condimentum dapibus. Nunc venenatis quis lorem consectetur condimentum. Vestibulum sed diam commodo, consequat arcu eget, condimentum nulla. In tristique neque vitae magna tempor tempor. Maecenas vitae lorem ut sem iaculis eleifend. In tristique tincidunt magna at sollicitudin. Nullam bibendum ex vel dui dapibus, non rutrum eros tempus. Suspendisse nisi quam, rhoncus luctus suscipit ut, pulvinar et orci. Nunc venenatis, eros sed interdum tempor, lectus lectus viverra velit, porttitor tincidunt urna lacus at libero. In suscipit neque eros, sed consequat arcu pharetra id. Mauris massa urna, luctus a commodo sed, efficitur sit amet tortor. Duis molestie, mauris ac venenatis elementum, elit purus mattis justo, eget rhoncus urna purus nec elit. Nunc a dui tellus. Pellentesque malesuada imperdiet mauris, sit amet eleifend velit convallis in. Pellentesque eu erat eu arcu lobortis ultricies vel vitae mauris.";
        public Transform tmOwner;

        public PointData(PointData data) {
            sName = data.sName;
            iCost = data.iCost;
            iProfit = data.iProfit;
            iLvlMax = data.iLvlMax;
            iActualLvL = data.iActualLvL;
            ID = data.ID;
            bCanUpgrade = data.bCanUpgrade;
            bCanBuy = data.bCanBuy;
            sDescription = data.sDescription;
            tmOwner = data.tmOwner;
        }

        public int iActualCost { get { return (iCost.Count > iActualLvL) ? iCost[iActualLvL] : -1; } }
        public int iActualProfit { get { return (iProfit.Count > iActualLvL) ? iProfit[iActualLvL] : -1; } }
        public bool bMaxed { get { return iLvlMax <= iActualLvL; } }

        public bool CheckOwn(Transform player) {
            return tmOwner == null || player == tmOwner;
        }

        public bool CanUpgrade(long val) {
            return (!bMaxed && bCanUpgrade) ? iActualCost <= val : false;
        }
        public bool CanBuy(long val) {
            return (bCanBuy) ? iActualCost <= val : false;
        }

        public void Upgrade() {
            if (!bCanUpgrade || bMaxed)
                return;
            this.iActualLvL++;
        }
    }
    [System.Serializable]
    public class PointDraw {
        public TextMesh txtCost;
        public TextMesh txtName;
        public TextMesh txtOwner;
        public TextMesh txtLvl;
        public TextMesh txtProfit;
        public bool bWithoutTextcard;
        private Point main;

        public void Init(Point Main) {
            main = Main;
        }

        public void SetTexts(string name, string cost, string profit, string lvl, string owner) {
            if (bWithoutTextcard)
                return;
            FindTexts();
            txtCost?.SetText(Strings.Points.sCost + ": " + cost);
            txtName?.SetText(name);
            txtOwner?.SetText(owner == "Właściciel" ? "Wolne" : owner);
            txtLvl?.SetText(Strings.Points.sLvl + ": " + lvl);
            txtProfit?.SetText(Strings.Points.sProfit + ": " + profit);
        }

        void FindTexts() {
            if (!txtCost || !txtCost || !txtLvl || !txtOwner || !txtProfit) {
                Transform textpoint = main.transform.Find("Texts").transform;
                if (textpoint) {
                    txtName = textpoint.Find("Name")?.GetComponent<TextMesh>();
                    txtCost = textpoint.Find("Cost")?.GetComponent<TextMesh>();
                    txtLvl = textpoint.Find("Lvls")?.GetComponent<TextMesh>();
                    txtOwner = textpoint.Find("Owner")?.GetComponent<TextMesh>();
                    txtProfit = textpoint.Find("Profit")?.GetComponent<TextMesh>();
                }
            }
        }
    }

    public class Point: MonoBehaviour {
        public PointData data;
        private PointData StartBackup;
        private Material materialBackup;
        public PointDraw draw;
        public List<PointEvent> pointEvent = new List<PointEvent>();
        public bool bRandomEvent;
        void Awake() {
            draw.Init(this);
            pointEvent = new List<PointEvent>();
            if (!GetComponent<PointEvent>())
                gameObject.AddComponent<PointEvent>();
            var components = GetComponents<PointEvent>();
            for (int i = 0; i < components.Length; i++) {
                pointEvent.Add(components[i]);
            }
        }

        void Start() {
            draw.SetTexts(data.sName, data.sActualCost, data.sActualProfit, (data.iActualLvL + 1).ToString(), data.sOwner);
        }
        public void RunOnEventPoints(Functions.Handle.OneArg f) {
            if (!bRandomEvent)
                RunOnEventPointsOnAll(f);
            else
                RunOnRandomPoints(f);
        }
        public void RunOnEventPointsOnAll(Functions.Handle.OneArg f) {
            for (int i = 0; i < pointEvent.Count; i++) {
                f?.Invoke(pointEvent[i]);
            }
        }

        public void RunOnRandomPoints(Functions.Handle.OneArg f) {
            f?.Invoke(pointEvent[Random.Range(0, pointEvent.Count - 1)]);
        }
        public void RestoreBackup() {
            data = StartBackup;
            draw.SetTexts(StartBackup.sName, StartBackup.sActualCost, StartBackup.sActualProfit, (StartBackup.iActualLvL + 1).ToString(), StartBackup.sOwner);
            if (materialBackup != null)
                transform.Find("Box")?.GetComponent<Renderer>().SetMaterial(materialBackup);
        }
        public PointData Base {
            get { return new PointData(StartBackup); }
        }
        public Point Init(int id) {
            data.ID = id;
            materialBackup = transform.Find("Box")?.GetComponent<Renderer>().material;
            StartBackup = new PointData(data);
            return this;
        }

        public Vector3 UnitPosition(int id) {
            Transform StayPos = transform.Find("StayPos").GetChild(id);
            return new Vector3(StayPos?.position.x ?? 0, StayPos?.position.y ?? 0, StayPos?.position.z ?? 0);
        }
        public Transform UnitTransform(int id) {
            return transform?.Find("StayPos")?.GetChild(id) ?? null;
        }
    }
}