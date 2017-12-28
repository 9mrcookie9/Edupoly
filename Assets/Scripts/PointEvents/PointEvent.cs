using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Points {
    [System.Serializable]
    public class PointEvent: MonoBehaviour {
        private bool bEndRoundAtPoint;
        public virtual void OnPlayerEndPointSelect(Main.Main_Controller GameController, Unit.UnitData Unit) { }
        public virtual void OnPlayerEnter(Main.Main_Controller GameController, Unit.UnitData Unit) { }
        public virtual void OnPlayerExit(Main.Main_Controller GameController, Unit.UnitData Unit) { }
        public virtual void OnPlayerEndRoundAtPoint(Main.Main_Controller GameController, Unit.UnitData Unit) { }
        public virtual void OnPlayerEndRoundAtPointButEndPointChange(Main.Main_Controller GameController, Unit.UnitData Unit) {}

		public virtual void OnPlayerEndPointSelectBackward(Main.Main_Controller GameController ,Unit.UnitData Unit) { }
		public virtual void OnPlayerEnterBackward(Main.Main_Controller GameController ,Unit.UnitData Unit) { }
		public virtual void OnPlayerExitBackward(Main.Main_Controller GameController ,Unit.UnitData Unit) { }
		public virtual void OnPlayerEndRoundAtPointBackward(Main.Main_Controller GameController ,Unit.UnitData Unit) { }
		public virtual void OnPlayerEndRoundAtPointButEndPointChangeBackward(Main.Main_Controller GameController ,Unit.UnitData Unit) { }
        public bool EndRoundAtPoint {
            get { return bEndRoundAtPoint; }
            private set { bEndRoundAtPoint = value; }
        }
    }
}