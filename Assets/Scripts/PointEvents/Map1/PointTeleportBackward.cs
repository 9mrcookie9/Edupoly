using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Points {
	[System.Serializable]
	public class PointTeleportBackward : PointEvent {
		public Point target;
		private bool bShowTextOnEnter;
		public override void OnPlayerEndPointSelect(Main.Main_Controller GameController ,Unit.UnitData Unit) {
			if (!target)
				return;
			var point = GameController.PointsController.GetPointAt(Unit.Move.data.FirstTarget?.position ?? Vector3.zero);
			var actualPoint = GameController.PointsController.GetPointAt(Unit.transform.position);
			var toMove = Mathf.Abs(
				GameController.PointsController.GetPointId(target) -
				GameController.PointsController.GetPointId(point)
			);
			bShowTextOnEnter = true;
			Unit.Move.data.bMoveBackward = true;
			GameController.MoveActualUnit(toMove);
		}
		public override void OnPlayerEnter(Main.Main_Controller GameController ,Unit.UnitData Unit) {
			if (bShowTextOnEnter) {
				bShowTextOnEnter = false;
				GameController.EventDravController.AddEvent("Przechodzisz na pole: " +
					GameController.PointsController.GetPointId(target) ,3f);
				GameController.PointsController.GetPointAt(transform.position).RunOnEventPoints((object data) => {
					PointEvent Event = data as PointEvent;
					if (Event != this)
						Event?.OnPlayerEndRoundAtPointBackward(GameController ,Unit);
				});
			}
		}
	}
}