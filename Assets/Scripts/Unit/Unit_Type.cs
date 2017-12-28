using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit {
    public class Unit_Type : MonoBehaviour {
        public enum Type { Player, Bot }
        public Type UnitType = Type.Bot;
    }
}