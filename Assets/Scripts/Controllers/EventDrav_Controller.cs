using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Main {
    [System.Serializable]
    public struct EventDravData {
        public string sValue;
        public float fTime;

        public EventDravData(string sValue, float fTime) {
            this.sValue = sValue;
            this.fTime = fTime;
        }
    }
    public class EventDrav_Controller : MonoBehaviour {
        public Canvas EventPopUp;

        public List<EventDravData> events = new List<EventDravData>();
        private float fNextEvent;
        

        public void AddEvent(EventDravData eventData) {
            events.Add(eventData);
            DrawEvent();
        }
        public void AddEvent<T>(T sValue, float fTime) {
            events.Add(new EventDravData(sValue.ToString(), fTime));
            DrawEvent();
        }
        public void RemoveAllEvents() {
            events = new List<EventDravData>();

        }
        private void Update() {
            DrawEvent();
            if(events.Count <= 0) {
                EventPopUp.enabled = false;
            }
        }
        void DrawEvent() {
            EventPopUp.enabled = true;
            if (events.Count > 0 && fNextEvent < Time.time) {
                var text = EventPopUp.transform.GetChild(0).GetChild(0)?.GetComponent<Text>();
                if (text != null) {
                    text.text = events[0].sValue;
                    Invoke("RemoveZero", events[0].fTime);
                } else
                    Debug.LogError("Text not found!");
            }
        }

        void RemoveZero() {
            if (events.Count > 0)
                events.RemoveAt(0);
        }
    }
}