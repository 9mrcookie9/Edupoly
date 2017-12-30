using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Points;

namespace Unit {
    [System.Serializable]
    public class Informations {
        public long iMoney;
        public int iActualPoint;
        public List<Point> pOwnedPoints;
        public Point pLastPoint;
        public int iActionPoints;
        public int iInJail;

        public Informations(long money, int actualPoint, List<Point> owned, Point lastPoint,int jail) {
            iMoney = money;
            iActualPoint = actualPoint;
            pOwnedPoints = owned;
            pLastPoint = lastPoint;
            iInJail = jail;
        }
        public class Builder {
            private long iMoney;
            private int iActualPoint;
            private List<Point> pOwnedPoints;
            private Point pLastPoint;
            private int iInJail;

            public Builder(Informations info) {
                iMoney = info.iMoney;
                iActualPoint = info.iActualPoint;
                pOwnedPoints = info.pOwnedPoints;
                pLastPoint = info.pLastPoint;
                iInJail = info.iInJail;
            }

            public Builder() {
                iMoney = 0;
                iActualPoint = 0;
                pOwnedPoints = new List<Point>();
                pLastPoint = null;
            }

            public Builder SetMoney(long value) {
                iMoney = value;
                return this;
            }
            public Builder InJail(int value){
                iInJail = value;
                return this;
            }
            public Builder SetActualPoint(int value) {
                iActualPoint = value;
                return this;
            }
            public Builder SetOwnedPoints(List<Point> value) {
                pOwnedPoints = value;
                return this;
            }
            public Builder SetLastPoint(Point value) {
                pLastPoint = value;
                return this;
            }

            public Informations Build() {
                return new Informations(iMoney, iActualPoint, pOwnedPoints, pLastPoint,iInJail);
            }
        }
    }
    public class Unit_Main : MonoBehaviour {
        public Informations info;
        public int ID;
        public string sName {
            get { return transform.name; }
            set { transform.name = value; }
        }
        public bool bActive = true;
        private Unit_Move move;
        public Unit_Move moveController { get { return move; } }
        private Main.Main_Controller gameController;
        public Main.Main_Controller GameController { get { return gameController; } }

        public Functions.Handle.NoArgs StartMove, EndMove, DefatAction;

        public Unit_Main SetActive(bool val) {
            bActive = val;
            return this;
        }
        void Awake() {
            move = GetComponent<Unit_Move>();
        }
        public Unit_Main Init(Main.Main_Controller main,int ID) {
            gameController = main;
            this.ID = ID;
            return this;
        }
        public Unit_Main SetData(Informations Info) {
            info = Info;
            return this;
        }
        public Unit_Main NextPoint() {
            gameController.NextPoint();
            return this;
        }
        public Unit_Main CallStart() {
            StartMove?.Invoke();
            return this;
        }
        public Unit_Main CallMoveStart() {
            move.StartMove();
            return this;
        }
        public Unit_Main CallDefateAction() {
            DefatAction?.Invoke();
            return this;
        }
        public Unit_Main UpdateMoney(long value) {
            info.iMoney += value;
            gameController.dravController.SetBalanceText(info.iMoney.ToString());
            return this;
        }
        public Unit_Main SetMoney(long value) {
            info.iMoney = value;
            gameController.dravController.SetBalanceText(info.iMoney.ToString());
            return this;
        }
        public void CallEnd() {
            var point = gameController.PointsController.GetPointAt(transform.position);
            if (point.data.tmOwner != transform && point.data.tmOwner != null) {
                UpdateMoney(-point.data.iActualProfit);
                point.data.tmOwner.GetComponent<Unit_Main>().UpdateMoney(point.data.iActualProfit);
                if (info.iMoney < 0)
                    Debug.Log(sName);
            }
            Invoke("End", Random.Range(100, 200) / 100);
        }
        void End() {
            EndMove?.Invoke();
            move.StopMove();
        }
        public float RotationSide() {
            return gameController.RootationSide();
        }
        public float GetMaxDistanceBetweenPointsTm() {
            return gameController.PointsController.fMaxDistanceBetweenPointsTm;
        }
    }
}