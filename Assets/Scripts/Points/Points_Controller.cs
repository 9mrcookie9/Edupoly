using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace Points {
    public class Points_Controller: MonoBehaviour {
        public List<Point> points;

        public float fMaxDistanceBetweenPointsTm = 6.14f;
        public int[] iRChangeRotArray = { 0, 9, 13, 21, 27 };
        public int[] iLChangeRotArray = { 16 };

        public Transform TmPointList;

        public void Init() {
            for (int i = 0; i < points.Count; i++) {
                points[i].Init(i);
            }
        }

        public void SetPointsList() {
            points = new List<Point>();
            for (int i = 0; i < TmPointList.childCount; i++) {
                points.Add(TmPointList.GetChild(i).SetName("Point #" + i).GetComponent<Point>());
            }
        }

        /*     public Point GetPoint(int id) {
                 if (id < points.Count)
                     return points[id];
                 else
                     return points[0 + (points.Count - id)];
             }*/

        public Point GetPointAt(Vector3 worldPos) {
            if (worldPos == Vector3.zero)
                Debug.Log("Pos = (0,0,0)");
            int pointId = 0;
            float smallesDistance = float.MaxValue;
            for (int i = 0; i < points.Count; i++) {
                for (int x = 0; x < points[i]?.transform?.Find("StayPos").childCount; x++) {
                    var pos = points[i]?.transform?.Find("StayPos")?.GetChild(x)?.position;
                    if (pos != null) {
                        if (pos.GetType() == typeof(Vector3))
                            if (Vector3.Distance(worldPos, (Vector3)pos) < smallesDistance) {
                                smallesDistance = Vector3.Distance(worldPos, (Vector3)pos);
                                pointId = i;
                            }

                    }
                }
            }
            return points[pointId];
        }

        public int GetPointId(Point point) {
            for (int i = 0; i < points.Count; i++) {
                if (points[i].gameObject == point.gameObject)
                    return i;
            }
            return 0;
        }
        public List<Transform> MovePath(int actual, int movePoints) {
            int pointsToMove = movePoints;
            List<Transform> targets = new List<Transform>();
            bool firstRoadEnded = false;
            for (int i = 0; i < pointsToMove; i++) {
                if ((actual + 1) + i >= points.Count) {
                    firstRoadEnded = true;
                    pointsToMove -= points.Count - 1;
                    i = 0;
                }
                if (i >= points.Count) {
                    pointsToMove -= points.Count - 1;
                    i = 0;
                }
                targets.Add(points[(!firstRoadEnded) ? (actual + 1) + i : i].UnitTransform(0).parent);
            }
            return targets;
        }
        #region Gizmos
#if UNITY_EDITOR
        void OnDrawGizmos() {
            if (TmPointList) {
                if (points.Count != TmPointList.childCount)
                    SetPointsList();
                DrawPointToPointLine();
                DrawPointToPoint();
            }
        }
        int[] NewTable(int[] table1, int[] table2) {
            int[] table = new int[table1.Length + table2.Length];
            for (int i = 0; i < table1.Length; i++) {
                table[i] = table1[i];
            }
            for (int i = 0; i < table2.Length; i++) {
                table[i + (table1.Length)] = table2[i];
            }
            System.Array.Sort(table);
            return table;
        }
        void DrawPointToPointLine() {
            Gizmos.color = Color.yellow;
            for (int i = 0; i < TmPointList.childCount; i++) {

                if (i > 0)
                    Gizmos.DrawLine(points[i - 1].transform.position, points[i].transform.position);
                if (i + 1 == TmPointList.childCount)
                    Gizmos.DrawLine(points[i].transform.position, points[0].transform.position);
            }
        }
        void DrawBorderMovement() {
            Gizmos.color = Color.green;
            int[] tableToDraw = NewTable(iRChangeRotArray, iLChangeRotArray);
            for (int i = 0; i < tableToDraw.Length; i++) {
                if (i + 1 >= tableToDraw.Length)
                    Gizmos.DrawLine(points[tableToDraw[i]].transform.position + Vector3.up, points[tableToDraw[0]].transform.position + (Vector3.up * 3));
                else
                    Gizmos.DrawLine(points[tableToDraw[i]].transform.position + Vector3.up, points[tableToDraw[i + 1]].transform.position + (Vector3.up * 3));
            }
        }
        void DrawPointToPoint() {
            for (int id = 0; id < 4; id++) {
                Gizmos.color = new Color(1f, .5f, .25f * id);
                for (int i = 0; i < TmPointList.childCount; i++) {
                    if (i > 0)
                        Gizmos.DrawLine(points[i - 1].transform.Find("StayPos").GetChild(id).position, points[i].transform.Find("StayPos").GetChild(id).position);
                    if (i + 1 == TmPointList.childCount)
                        Gizmos.DrawLine(points[i].transform.Find("StayPos").GetChild(id).position, points[0].transform.Find("StayPos").GetChild(id).position);
                }
            }
        }
#endif
        #endregion
    }
}