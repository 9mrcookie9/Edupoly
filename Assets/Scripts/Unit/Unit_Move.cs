using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Unit {
    public class Unit_Move : MonoBehaviour {
        [System.Serializable]
        public class Data {
            public float fMovSpeed;
            public float fRotSpeed;
            public int iMovePos;
            public bool bMove;
            public bool bFirstMoved;
            public bool bThisUnitIsAllowedToMove;

            public bool bMoveBackward;
            public Functions.Handle.NoArgs OnEnd;
            public List<Transform> tmTarget = new List<Transform>();
            public Transform FirstTarget => tmTarget.Count > 0 ? tmTarget[0] : null;
            public Transform LastTarget;
            public Data RemoveFirstTarget() {
                if (tmTarget.Count > 0)
                    tmTarget.RemoveAt(0);
                else
                    bMoveBackward = false;
                return this;
            }
            public float fRotSpeedCalc;
            public Vector3 vecLastEulerAngles;
            public bool bLastRotSaved;
            public float fRootTarget;
        }
        public Data data;
        private Unit_Main main;

        void Awake() {
            main = GetComponent<Unit_Main>();
        }
        void Update() {
            bool bMoveEnded = false;
            if (data.bMove) {
                if (data.FirstTarget != null) {
                    var target = data.FirstTarget.GetChild(data.iMovePos);
                    if (!data.bLastRotSaved) {
                        data.vecLastEulerAngles = transform.localEulerAngles;
                        data.bLastRotSaved = true;
                        float distance = main.GetMaxDistanceBetweenPointsTm();
                        if (target.parent.parent.GetNextChildren() != null)
                            if (target.GetChildId() >= 0)
                                distance = Vector3.Distance(target.position ,target.parent.parent.GetNextChildren().Find("StayPos").GetChild(target.GetChildId()).position);

                        var transToTargetDist = Vector3.Distance(transform.position ,target.position);
                        data.fRotSpeedCalc = data.fRotSpeed * (Mathf.Max(distance ,transToTargetDist) / Mathf.Min(distance ,transToTargetDist));
                    }
                    transform.position = Vector3.MoveTowards(transform.position ,target.position ,Time.deltaTime * data.fMovSpeed);
                    if ((int)transform.localEulerAngles.y != (int)(data.vecLastEulerAngles + new Vector3(0 ,main.RotationSide())).y)
                        transform.localEulerAngles = Vector3.MoveTowards(transform.localEulerAngles ,data.vecLastEulerAngles + new Vector3(0 ,main.RotationSide()) ,Time.deltaTime * data.fRotSpeedCalc);
                    if (transform.position == target.position) {
                        ResetLastEulerAndSpeed();
                        Rotate(main.RotationSide());
                        main.NextPoint();
                        data.RemoveFirstTarget();
                    }
                    if (!data.FirstTarget) {
                        bMoveEnded = true;
                        StopMove();
                        if (data.OnEnd != null) {
                            data.OnEnd?.Invoke();
                        } else {
                            MoveEnded();
                        }
                    }
                } else {
                    ResetLastEulerAndSpeed();
                    Rotate(main.RotationSide());
                    StopMove();
                    Debug.Log("STOP");
                    //    bMoveEnded = true;
                }
            } else {
                //  if (tmTarget.Count <= 0)
                //      bMoveEnded = true;
                ResetLastEulerAndSpeed();

            }
            if (bMoveEnded && bRotationDone()) {
                if (data.OnEnd != null) {
                    data.OnEnd?.Invoke();
                } else {
                    MoveEnded();
                }
            }

        }
        bool bRotationDone() {
            return ((int)transform.localEulerAngles.y == (int)(data.vecLastEulerAngles + new Vector3(0 ,main.RotationSide())).y);
        }
        void ResetLastEulerAndSpeed() {
            data.fRotSpeedCalc = data.fRotSpeed;
            data.bLastRotSaved = false;
        }
        void Rotate(float degress) {
            if ((int)transform.localEulerAngles.y != (int)(data.vecLastEulerAngles.y + degress))
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x ,data.vecLastEulerAngles.y + degress ,transform.localEulerAngles.z);
        }
        public void MoveEnded() {
            data.bThisUnitIsAllowedToMove = false;
            main.GameController.UnitMoveDoneHandle(main);
        }
        public void ChangeTarget(List<Transform> target) {
            data.tmTarget = target ?? new List<Transform>();
            data.LastTarget = target[target.Count - 1].parent;
        }
        public void ForceStopMove() {
            data.bMove = false;
            ChangeTarget(null);
        }
        public void StopMove() {
            data.bMove = false;
        }
        public void StartMove() {
            data.bMove = true;
            data.bThisUnitIsAllowedToMove = true;
        }
    }
}
