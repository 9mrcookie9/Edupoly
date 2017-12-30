using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Main {

    [System.Serializable]
    public class Spawn_Controller {
        public int iPlayers;
        public int iBots;
        public int iPlayerAndBots;
        public int iPlayerMaxPoints;
        public int iMoveChances;
        public bool bTeachMode;
        public bool bFakeData;
        public int iStartMoney;
        public float fInterest;
        public void Start() {
            GetNewGameData();
        }
        void GetNewGameData() {
            fInterest = 0.5f;
            if (bFakeData) {
                if (iPlayers == 0 && iBots == 0)
                    iPlayers = 1;
                if (iBots == 0 && iPlayers == 0)
                    iBots = 3;
                if (iStartMoney == 0)
                    iStartMoney = 5000;
                if (iPlayerMaxPoints == 0)
                    iPlayerMaxPoints = 1;
            } else {
                iPlayers = PlayerPrefs.GetInt("iPlayersCount");
                iBots = PlayerPrefs.GetInt("iBotsCount");
                iStartMoney = PlayerPrefs.GetInt("iStartMoney");
                iPlayerMaxPoints = PlayerPrefs.GetInt("iActionsCount");
                iMoveChances = PlayerPrefs.GetInt("iChances");
                if (PlayerPrefs.GetInt("iTeachMode") >= 1)
                    bTeachMode = true;
                else
                    bTeachMode = false;
            }
            iPlayerAndBots = iPlayers + iBots;
        }
    }
    [System.Serializable]
    public class Prefabs {
        public GameObject[] players;
        public GameObject[] bots;
    }
    [System.Serializable]
    public class ActualUnit {
        public int iActualUnit = 0;
#pragma warning disable IDE1006 // Style nazewnictwa
        public Unit.UnitData unit { get { return main.GetUnit(iActualUnit); } }
#pragma warning restore IDE1006 // Style nazewnictwa
        private Main_Controller main;
        private int iCount;
        public void Init(Main_Controller Main) {
            main = Main;
            iCount = main.spawn.iPlayerAndBots;
        }
        public ActualUnit UpdateCount(int count) {
            iCount = count;
            return this;
        }
        public ActualUnit NextUnit() {
            iActualUnit = (iActualUnit + 1 < iCount) ? iActualUnit + 1 : 0;
            return this;
        }
    }
    public class Main_Controller: MonoBehaviour {
        public Spawn_Controller spawn;
        public Prefabs prefabs;
        public ActualUnit actualUnit;
        public Points.Points_Controller PointsController { get; private set; }
        public Unit.Units_Controller unitsController { get; private set; }
        public Card_Draw_Controller drawCardController { get; private set; }
        public NextMove nextMoveController { get; private set; }
        public Bones_Push BonesController { get; private set; }
        public Drav_Controller dravController { get; private set; }
        public EventDrav_Controller EventDravController { get; private set; }
        public delegate void GetPlayer(Unit.UnitData unit);

        public List<GetPlayer> subscribeForUnitList = new List<GetPlayer>();
        public Transform tmMovementTarget;

        public bool bCanMove = false;
        public System.Random randomGenerator = new System.Random();

        public bool bWaitOnEnd;
        public float fLastUnitDone;
        void Awake() {
            Random.InitState((int)System.DateTime.Now.Ticks);
            PointsController = GetComponent<Points.Points_Controller>();
            unitsController = GetComponent<Unit.Units_Controller>();
            drawCardController = GetComponent<Card_Draw_Controller>();
            BonesController = GetComponent<Bones_Push>();
            nextMoveController = GetComponent<NextMove>();
            dravController = GetComponent<Drav_Controller>();
            EventDravController = GetComponent<EventDrav_Controller>();
        }

        void Start() {
            Init();
            Debug.Log(unitsController.units[1].transform.position + " | " + PointsController.points[10].transform.position);
            Debug.Log(PointsController.GetPointAt(unitsController.units[1].transform.position));
        }

        void Init() {
            spawn.Start();
            actualUnit.Init(this);
            PointsController.Init();
            PointsController.SetPointsList();
            Spawn();
            for (int i = 0; i < subscribeForUnitList.Count; i++) {
                subscribeForUnitList[i](actualUnit.unit);
            }
            drawCardController.SetTextCard(0);
            BonesController.SetText("Kliknij aby rzucić kostką");
            UpdateTexts();
            actualUnit.unit.Main.info.iActionPoints = spawn.iPlayerMaxPoints;
            actualUnit.unit.Main.CallStart();
            actualUnit.unit.Move.data.bFirstMoved = true;
            //Movement test
            //MoveActualUnit(2);
        }

        void CheckUnitDebetAndDelete() {
            if (actualUnit.unit.Main.info.iMoney <= -500) {
                actualUnit.unit.Main.CallDefateAction();
                if (actualUnit.unit.Main.info.iMoney <= -500) {
                    Debug.Log("Defate: " + actualUnit.unit.Main.sName);
                    TestUnmarkAllPoints();
                    unitsController.DelUnit(actualUnit.unit.Main.SetActive(false).transform);
                }
            }
        }

        void TestUnmarkAllPoints() {
            for (int i = 0; i < PointsController.points.Count; i++) {
                if (PointsController.points[i].data.tmOwner == actualUnit.unit.transform) {
                    Debug.Log("RESTORE: " + actualUnit.unit.Main.sName);
                    PointsController.points[i].RestoreBackup();
                }
            }
        }

        void Spawn() {
            int counter = 0;
            for (int i = 0; i < spawn.iPlayers; i++, counter++) {
                GameObject Go = Instantiate(prefabs.players[0]);
                unitsController.AddUnit(Go.transform);
                Go.transform.position = PointsController.points[0].UnitPosition(counter);
            }
            for (int i = 0; i < spawn.iBots; i++, counter++) {
                GameObject Go = Instantiate(prefabs.bots[0]);
                unitsController.AddUnit(Go.transform);
                Go.transform.position = PointsController.points[0].UnitPosition(counter);
            }
            for (int i = 0; i < unitsController.units.Count; i++) {
                string name = (unitsController.units[i].transform.GetComponent<Unit.Unit_Type>().UnitType == Unit.Unit_Type.Type.Bot) ? "Bot " : "Gracz ";
                unitsController.units[i].transform.SetName(name + (i + 1));
                unitsController.units[i].Main.Init(this, i);
                Color32 color = new Color32((byte)Random.Range(0, 255), (byte)Random.Range(0, 255), (byte)Random.Range(0, 255), 1);
                unitsController.units[i].transform?.GetComponent<Unit.Unit_Outfit>()?.SetChestColor(color).SetHeadColor(color);
                unitsController.units[i].transform.GetComponent<Unit.Unit_Move>().data.iMovePos = i;
            }
        }

        public void NextRound() {
            Functions.CallBack.NoArgs OnPass = Functions.CallBack.Empty;
            Functions.CallBack.NoArgs OnFail = Functions.CallBack.Empty;
            int movesCount = (spawn.bTeachMode) ? spawn.iMoveChances : -1;

            drawCardController.SetTextCard();
            drawCardController.HideCard();
            BonesController.RemoveAllTexts().SetText("Kliknij aby rzucić kostką");

            CheckUnitDebetAndDelete();
            actualUnit.UpdateCount(unitsController.units.Count).NextUnit();
            SendActualUnit();


            if (actualUnit.unit.Main.info.iInJail > 0) {
                actualUnit.unit.Main.info.iInJail--;

                EventDravController.AddEvent(actualUnit.unit.transform.name + " zostało '" + actualUnit.unit.Main.info.iInJail + "' kolejek w więzieniu!", 2f);
                this.SetTimeout(3f, NextRound);
                return;
            }

            bWaitOnEnd = false;
            actualUnit.unit.Main.CallStart().info.iActionPoints = spawn.iPlayerMaxPoints;

            OnPass = () => {
                UpdateTexts();
                nextMoveController.nextMove.RemoveCallbacks(OnPass, OnFail);
            };
            OnFail = () => {
                NextRound();
                nextMoveController.nextMove.RemoveCallbacks(OnPass, OnFail);
            };
            EventDravController.RemoveAllEvents();
            if (unitsController.units.Count == 1) {
                Time.timeScale = 1;
                EventDravController.AddEvent("Wygrał: " + actualUnit.unit.Main.sName, 10000);
                OnPass();
                Debug.Log("Win: " + actualUnit.unit.Main.sName);
                //     return actualUnit.unit;
            }
            if (movesCount > 0 && actualUnit.unit.Move.data.bFirstMoved && actualUnit.unit.transform.GetComponent<Unit.Unit_Type>().UnitType == Unit.Unit_Type.Type.Player) {
                nextMoveController.ShowNextMoveTest(movesCount);
                nextMoveController.nextMove.SetCallbacks(OnPass, OnFail);
            } else {
                actualUnit.unit.Move.data.bFirstMoved = true;
                OnPass();
            }
            //   return actualUnit.unit;
        }
        public void MoveActualUnit(int iPointsToMove) {
            var startPointId = PointsController.GetPointAt(actualUnit.unit.transform.position).transform.GetChildId();

            var points = PointsController.MovePath(startPointId, iPointsToMove);
            tmMovementTarget.position = points[points.Count - 1].GetChild(actualUnit.unit.Move.data.iMovePos).position + (Vector3.up / 2);
            tmMovementTarget.GetComponent<MeshRenderer>()?.SetMaterial(actualUnit.unit.Outfit.body.Chest.GetComponent<MeshRenderer>().material);
            actualUnit.unit.Move.ChangeTarget(points);
            actualUnit.unit.Main.CallMoveStart();
            Points.Point lastPoint = actualUnit.unit.Move.data.LastTarget.GetComponent<Points.Point>();
            if (lastPoint == null)
                PointsController.GetPointAt(actualUnit.unit.transform.position);

            if (!actualUnit.unit.Move.data.bMoveBackward)
                lastPoint.RunOnEventPoints((object Event) => {
                    Points.PointEvent data = Event as Points.PointEvent;
                    data?.OnPlayerEndPointSelect(this, actualUnit.unit);
                });
            else
                lastPoint.RunOnEventPoints((object Event) => {
                    Points.PointEvent data = Event as Points.PointEvent;
                    data?.OnPlayerEndPointSelectBackward(this, actualUnit.unit);
                });
            drawCardController.SetTextCard(startPointId + iPointsToMove >= PointsController.points.Count ? (startPointId + iPointsToMove) - PointsController.points.Count - 1 : startPointId + iPointsToMove);
        }
        void SendActualUnit() {
            for (int i = 0; i < subscribeForUnitList.Count; i++) {
                subscribeForUnitList[i](actualUnit.unit);
            }
            //sendController.Send(actualUnit.unit);
        }
        public List<Points.Point> GetAllPointsOfUnit(Transform unit) {
            List<Points.Point> list = new List<Points.Point>();
            for (int i = 0; i < PointsController.points.Count; i++) {
                if (PointsController.points[i].data.tmOwner == unit)
                    list.Add(PointsController.points[i]);
            }
            return list;
        }
        public void UpdateTexts() {
            dravController.
                SetPlayerText(actualUnit?.unit.Main?.sName).
                SetBalanceText(actualUnit?.unit.Main?.info.iMoney).
                SetBankBalanceText(0);
        }
        public int iMoveCount = 0;
        public void NextPoint() {
            try {
                if (!actualUnit.unit.Move.data.bMoveBackward)
                    PointsController.points[actualUnit.unit.Main.info.iActualPoint]?.RunOnEventPoints((object Event) => {
                        Points.PointEvent data = Event as Points.PointEvent;
                        data?.OnPlayerExit(this, actualUnit.unit);
                    });
                else
                    PointsController.points[actualUnit.unit.Main.info.iActualPoint]?.RunOnEventPoints((object Event) => {
                        Points.PointEvent data = Event as Points.PointEvent;
                        data?.OnPlayerExitBackward(this, actualUnit.unit);
                    });
                actualUnit.unit.NextPoint();
                if (!actualUnit.unit.Move.data.bMoveBackward)
                    PointsController.points[actualUnit.unit.Main.info.iActualPoint]?.RunOnEventPoints((object Event) => {
                        Points.PointEvent data = Event as Points.PointEvent;
                        data?.OnPlayerEnter(this, actualUnit.unit);
                    });
                else
                    PointsController.points[actualUnit.unit.Main.info.iActualPoint]?.RunOnEventPoints((object Event) => {
                        Points.PointEvent data = Event as Points.PointEvent;
                        data?.OnPlayerEnterBackward(this, actualUnit.unit);
                    });
            } catch (System.Exception e) {
                Debug.LogException(e, this);
            }
        }
        public void UnitMoveDoneHandle(Unit.Unit_Main UnitMain) {
            if (Time.time != fLastUnitDone) {
                fLastUnitDone = Time.time;
                tmMovementTarget.position = new Vector3(9999, 9999, 9999);
                var actualPoint = PointsController.GetPointAt(actualUnit.unit.transform.position);
                if (!actualPoint)
                    Debug.LogError("Point at UNIT(" + actualUnit.unit.Main.ID + ") not found", actualUnit.unit.Main.transform);
                if (!actualUnit.unit.Move.data.bMoveBackward)
                    actualPoint.RunOnEventPoints((object Event) => {
                        Points.PointEvent data = Event as Points.PointEvent;
                        Debug.Log(data.name, data);
                        data?.OnPlayerEndRoundAtPoint(this, actualUnit.unit);
                    });
                else
                    actualPoint.RunOnEventPoints((object Event) => {
                        Points.PointEvent data = Event as Points.PointEvent;
                        data?.OnPlayerEndRoundAtPointBackward(this, actualUnit.unit);
                    });
                bWaitOnEnd = true;
                BonesController.SetText("Kliknij aby zakończyć rundę");
                actualUnit.unit.Main.CallEnd();
            }
        }

        public void SubscribeForUnit(GetPlayer callBack) {
            subscribeForUnitList.Add(callBack);
        }

        public void UnSubscribeForUnit(GetPlayer callBack) {
            subscribeForUnitList.Remove(callBack);
        }

        public Unit.UnitData GetUnit(int id) {
            return unitsController.GetUnit(id);
        }
        public float RootationSide() {
            for (int i = 0; i < PointsController.iRChangeRotArray.Length; i++)
                if (actualUnit.unit.Main.info.iActualPoint + 1 == PointsController.iRChangeRotArray[i])
                    return 90;
            for (int i = 0; i < PointsController.iLChangeRotArray.Length; i++)
                if (actualUnit.unit.Main.info.iActualPoint + 1 == PointsController.iLChangeRotArray[i])
                    return -90;
            return 0.0f;
        }
    }
}
