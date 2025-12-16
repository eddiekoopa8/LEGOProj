using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.UI;

namespace Unity.LEGO.Behaviours.Actions {
    public class SpillOutStuds : Action {

        [SerializeField, Tooltip("Where do the studs come from")]
        GameObject StudSpillOutPoint = null;

        [SerializeField, Tooltip("Total number of times studs can spill out of this object"), Range(0, 10)]
        private int m_totalNumberOfTimesStudsSpilled = 1;

        [SerializeField, Tooltip("Total number of required Silver studs"), Range(0, 1000)]
        private int silverStudTotal = 10;
        [SerializeField, Tooltip("Total number of required Gold studs"), Range(0, 100)]
        private int goldStudTotal = 10;
        [SerializeField, Tooltip("Total number of required Blue studs"), Range(0, 10)]
        private int blueStudTotal = 0;
        [SerializeField, Tooltip("Total number of required Purple studs - recommend never using"), Range(0, 1)]
        private int purpleStudTotal = 0;

        private int m_numberOfTimeStudsSpilled = 0;
        bool remainingStudsa = true;

        [SerializeField, Tooltip("Time in seconds before the trigger becomes active again."), Range(0, 10)]
        private float m_CooldownTime = 2f;

        private bool m_IsCoolingDown = false;

        [SerializeField, Tooltip("Trigger the explosion of studs")]
        bool trigger = false;

        private Stack<GameObject> SilverStuds = new();
        private Stack<GameObject> GoldStuds = new();
        private Stack<GameObject> BlueStuds = new();
        private Stack<GameObject> PurpleStuds = new();

        protected override void Start() {
            IEnumerator coroutine = SpawnStuds();
            StartCoroutine(coroutine);
            base.Start();
        }

        IEnumerator SpawnStuds() {
            while (silverStudTotal != SilverStuds.Count) {
                yield return new WaitForEndOfFrame();
                SilverStuds.Push(Instantiate(StudManagerScript.SilverStud));
                SilverStuds.Peek().SetActive(false);
                SilverStuds.Peek().transform.SetParent(transform);
            }
            while (goldStudTotal != GoldStuds.Count) {
                yield return new WaitForEndOfFrame();
                GoldStuds.Push(Instantiate(StudManagerScript.GoldStud));
                GoldStuds.Peek().SetActive(false);
                GoldStuds.Peek().transform.SetParent(transform);
            }
            while (blueStudTotal != BlueStuds.Count) {
                yield return new WaitForEndOfFrame();
                BlueStuds.Push(Instantiate(StudManagerScript.BlueStud));
                BlueStuds.Peek().SetActive(false);
                BlueStuds.Peek().transform.SetParent(transform);
            }
            while (purpleStudTotal != PurpleStuds.Count) {
                yield return new WaitForEndOfFrame();
                PurpleStuds.Push(Instantiate(StudManagerScript.PurpleStud));
                PurpleStuds.Peek().SetActive(false);
                PurpleStuds.Peek().transform.SetParent(transform);
            }
        }

        protected override void Reset() {
            base.Reset();

            m_AudioVolume = 0.5f; // Retaining base class audio settings if needed
            m_IconPath = "Assets/LEGO/Gizmos/LEGO Behaviour Icons/UI Action.png";
        }

        protected void Update() {

            if ((m_Active && !m_IsCoolingDown) || trigger) {
                if (remainingStudsa) {
                    SendOutStuds();
                    trigger = false;
                }
            }
        }

        public void SendOutStuds() {
            List<GameObject> activeStuds = new();
            if (StudManagerScript.thisGameObject != null) {
                int remainingStuds;
                try {
                    remainingStuds = SilverStuds.Count / (m_totalNumberOfTimesStudsSpilled - m_numberOfTimeStudsSpilled);
                }
                catch (DivideByZeroException) {
                    remainingStudsa = false;
                    return;
                }
                for (int i = 0; i < remainingStuds; i++) {
                    activeStuds.Add(SilverStuds.Pop());
                    activeStuds[^1].GetComponent<Stud>().randomoffsets = UnityEngine.Random.insideUnitCircle;
                    activeStuds[^1].SetActive(true);
                }
                try {
                    remainingStuds = GoldStuds.Count / (m_totalNumberOfTimesStudsSpilled - m_numberOfTimeStudsSpilled);
                    for (int i = 0; i < remainingStuds; i++) {
                        activeStuds.Add(GoldStuds.Pop());
                        activeStuds[^1].GetComponent<Stud>().randomoffsets = UnityEngine.Random.insideUnitCircle;
                        activeStuds[^1].SetActive(true);
                    }
                }
                catch (DivideByZeroException) { }
                try {
                    remainingStuds = BlueStuds.Count / (m_totalNumberOfTimesStudsSpilled - m_numberOfTimeStudsSpilled);
                    for (int i = 0; i < remainingStuds; i++) {
                        activeStuds.Add(BlueStuds.Pop());
                        activeStuds[^1].GetComponent<Stud>().randomoffsets = UnityEngine.Random.insideUnitCircle;
                        activeStuds[^1].SetActive(true);
                    }
                }
                catch (DivideByZeroException) { }
                try {
                    remainingStuds = PurpleStuds.Count / (m_totalNumberOfTimesStudsSpilled - m_numberOfTimeStudsSpilled);
                    for (int i = 0; i < remainingStuds; i++) {
                        activeStuds.Add(PurpleStuds.Pop());
                        activeStuds[^1].GetComponent<Stud>().randomoffsets = UnityEngine.Random.insideUnitCircle;
                        activeStuds[^1].SetActive(true);
                    }
                }
                catch (DivideByZeroException) { }
                StartCoroutine(BounceStuds(activeStuds));
                m_IsCoolingDown = true;
                m_numberOfTimeStudsSpilled++;

            }
            else {
                Debug.LogWarning("No Stud Manager");
            }
        }
        IEnumerator BounceStuds(List<GameObject> activeStuds) {
            float angle;
            float startingTime = Time.time;
            Vector3 spillOutPoint;
            while (Time.time - startingTime < Mathf.PI) {
                spillOutPoint = StudSpillOutPoint == null ? transform.position : StudSpillOutPoint.transform.position;
                for (int i = 0; i < activeStuds.Count; i++) {
                    if (activeStuds[i].transform.GetChild(0).GetComponent<Image>() != null) {
                        activeStuds.Remove(activeStuds[i]);
                        i--;
                        continue;
                    }
                    angle = i + activeStuds[i].GetComponent<Stud>().randomoffsets.x;
                    Debug.Log(Time.time - startingTime);
                    float yPos = spillOutPoint.y + Mathf.Abs(Mathf.Sin((Time.time - startingTime) * 4) / Mathf.Max(1, Time.time - startingTime));
                    float xPos = spillOutPoint.x + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Sin(angle);
                    float zPos = spillOutPoint.z + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Cos(angle);
                    activeStuds[i].transform.position = new Vector3(xPos, yPos, zPos);
                    //Debug.Log(activeStuds[i].transform.position + ", " + xPos + ", " + yPos + ", " + zPos);
                }
                yield return null;
            }
            spillOutPoint = StudSpillOutPoint == null ? transform.position : StudSpillOutPoint.transform.position;
            for (int i = 0; i < activeStuds.Count; i++) {
                angle = i + activeStuds[i].GetComponent<Stud>().randomoffsets.x;
                float yPos = spillOutPoint.y;
                float xPos = spillOutPoint.x + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Sin(angle);
                float zPos = spillOutPoint.z + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Cos(angle);
                activeStuds[i].transform.position = new Vector3(xPos, yPos, zPos);
            }
            yield return null;
            StartCooldown();
        }

        private void StartCooldown() {
            m_IsCoolingDown = true;
            Invoke(nameof(EndCooldown), m_CooldownTime);
        }

        private void EndCooldown() {
            m_IsCoolingDown = false;
            m_Active = false; // Deactivate action
        }
    }
}

