using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unit {
    [System.Serializable]
    public struct UnitData {
        public Transform transform;
        public Unit_Main Main { get { return transform?.GetComponent<Unit_Main>() ?? null; } }
        public Unit_Move Move { get { return transform?.GetComponent<Unit_Move>() ?? null; } }
        public Unit_Outfit Outfit { get { return transform?.GetComponent<Unit_Outfit>() ?? null; } }
        public Unit_Type Type { get { return transform?.GetComponent<Unit_Type>() ?? null; } }

        public UnitData(Transform tm) {
            transform = tm;
        }
        public void SetPoint(int id) {
            Main.info.iActualPoint = id;
        }
        public void NextPoint() {
            SetPoint((Main.GameController.PointsController.points.Count <= Main.info.iActualPoint + 1) ? 0 : Main.info.iActualPoint + 1);
        }
    }
    public class Units_Controller : MonoBehaviour {
        public List<UnitData> units = new List<UnitData>();

        public void AddUnit(Transform toAdd) {
            if (!InList(toAdd))
                units.Add(new UnitData(toAdd));
        }
        public void DelUnit(Transform toDell) {
            if (InList(toDell))
                units.RemoveAt(InListid(toDell));
        }
        public UnitData GetUnit(Transform toGet) {
            if (InList(toGet))
                return units[InListid(toGet)];
            return new UnitData();
        }
        public UnitData GetUnit(int id) {
            if (id < units.Count)
                return units[id];
            return new UnitData();
        }
        bool InList(Transform toTest) {
            for (int i = 0; i < units.Count; i++) {
                if (units[i].transform == toTest)
                    return true;
            }
            return false;
        }
        int InListid(Transform toTest) {
            for (int i = 0; i < units.Count; i++) {
                if (units[i].transform == toTest)
                    return i;
            }
            return -1;
        }
    }
}