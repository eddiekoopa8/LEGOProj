using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

namespace Unity.LEGO.Behaviours.Actions {
    public class SpillOutStudsRegular : MonoBehaviour {

        [SerializeField, Tooltip("Total number of times studs can spill out of this object")]
        private int m_totalNumberOfTimesStudsSpilled = 1;

        [SerializeField, Tooltip("Total number of required Silver studs")]
        private int silverStudTotal = 10;
        [SerializeField, Tooltip("Total number of required Gold studs")]
        private int goldStudTotal = 10;
        [SerializeField, Tooltip("Total number of required Blue studs")]
        private int blueStudTotal = 0;
        [SerializeField, Tooltip("Total number of required Purple studs - recommend never using")]
        private int purpleStudTotal = 0;

        private int m_numberOfTimeStudsSpilled = 0;
        bool remainingStudsa = true;

        [SerializeField, Tooltip("Time in seconds before the trigger becomes active again.")]
        private float m_CooldownTime = 2f;

        private bool m_IsCoolingDown = false;

        [SerializeField, Tooltip("Trigger the explosion of studs")]
        public bool trigger = false;

        private Stack<GameObject> SilverStuds = new();
        private Stack<GameObject> GoldStuds = new();
        private Stack<GameObject> BlueStuds = new();
        private Stack<GameObject> PurpleStuds = new();

        private void Start() {
            IEnumerator coroutine = SpawnStuds();
            StartCoroutine(coroutine);
        }

        IEnumerator SpawnStuds() {
            while (silverStudTotal != SilverStuds.Count) {
                yield return new WaitForEndOfFrame();
                SilverStuds.Push(Instantiate(StudManagerScript.SilverStud));
                SilverStuds.Peek().SetActive(false);
            }
            while (goldStudTotal != GoldStuds.Count) {
                yield return new WaitForEndOfFrame();
                GoldStuds.Push(Instantiate(StudManagerScript.GoldStud));
                GoldStuds.Peek().SetActive(false);
            }
            while (blueStudTotal != BlueStuds.Count) {
                yield return new WaitForEndOfFrame();
                BlueStuds.Push(Instantiate(StudManagerScript.BlueStud));
                BlueStuds.Peek().SetActive(false);
            }
            while (purpleStudTotal != PurpleStuds.Count) {
                yield return new WaitForEndOfFrame();
                PurpleStuds.Push(Instantiate(StudManagerScript.PurpleStud));
                PurpleStuds.Peek().SetActive(false);
            }
        }


        protected void Update() {

            if (trigger && !m_IsCoolingDown) {
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
                Debug.Log(remainingStuds + ", " + SilverStuds.Count + ", " + m_numberOfTimeStudsSpilled + ", " + m_totalNumberOfTimesStudsSpilled);
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
            while (Time.time - startingTime < Mathf.PI) {
                for (int i = 0; i < activeStuds.Count; i++) {
                    angle = i + activeStuds[i].GetComponent<Stud>().randomoffsets.x;
                    float yPos = transform.position.y + Mathf.Abs(Mathf.Sin((Time.time - startingTime) * 4));
                    float xPos = transform.position.x + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Sin(angle);
                    float zPos = transform.position.z + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Cos(angle);
                    activeStuds[i].transform.position = new Vector3(xPos, yPos, zPos);
                    //Debug.Log(activeStuds[i].transform.position + ", " + xPos + ", " + yPos + ", " + zPos);
                }
                yield return null;
            }
            for (int i = 0; i < activeStuds.Count; i++) {
                angle = i + activeStuds[i].GetComponent<Stud>().randomoffsets.x;
                float yPos = transform.position.y;
                float xPos = transform.position.x + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Sin(angle);
                float zPos = transform.position.z + activeStuds[i].GetComponent<Stud>().randomoffsets.y * (Time.time - startingTime) * Mathf.Cos(angle);
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
            trigger = false; // Deactivate action
        }
    }
}

