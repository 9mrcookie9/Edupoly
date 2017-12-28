using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unit {
    [System.Serializable]
    public class Body {
        public Transform Head;
        public Transform Chest;
    }

    public class Unit_Outfit : MonoBehaviour {
        public Body body;

        public Unit_Outfit SetHeadColor(Color color) {
            var logError = body.Head?.GetComponent<Renderer>()?.SetMaterialColor(color);
            if (!logError)
                Debug.LogError("HEAD EMPTY");
            return this;
        }
        public Unit_Outfit SetChestColor(Color color) {
            var logError = body.Chest?.GetComponent<Renderer>()?.SetMaterialColor(color);
            if (!logError)
                Debug.LogError("Chest EMPTY");
            return this;
        }
    }
}