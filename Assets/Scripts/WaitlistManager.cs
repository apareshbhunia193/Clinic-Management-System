using UnityEngine;
using TMPro;
using System;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class PatientData
{
    public int tokenID;
    public string patientName;
    public string arrivalTime;
}

[System.Serializable]
public class WaitlistSaveData
{
    public string savedDate;
    public int savedTokenCounter;
    public List<PatientData> savedQueue;
}

public class WaitlistManager : MonoBehaviour
{
    [Header("Top Metrics UI")]
    public TextMeshProUGUI waitingNowText; // <--- NEW: Link your Top Panel "99" here!

    [Header("UI References")]
    public TMP_InputField nameInputField;
    public GameObject addPatientPanel;
    public Transform waitingListContentArea;
    public GameObject waitingListRowPrefab;

    [Header("Current Patient Banner UI")]
    public TextMeshProUGUI currentBannerTokenText;
    public TextMeshProUGUI currentBannerNameText;

    private List<PatientData> activeQueue = new List<PatientData>();
    private int dailyTokenCounter = 1;
    private string saveFilePath;

    private void Awake()
    {
        addPatientPanel.SetActive(false);
        saveFilePath = Application.persistentDataPath + "/clinic_waitlist.json";
        LoadData();
    }

    void Start()
    {
        nameInputField.onSubmit.AddListener(delegate { AddPatient(); });
    }

    public void onClickAddtoWaitingList()
    {
        addPatientPanel.SetActive(true);
    }

    // --- NEW HELPER METHOD ---
    // Updates the top panel number based on exactly how many people are in the C# list
    private void UpdateWaitingNowCount()
    {
        if (waitingNowText != null)
        {
            waitingNowText.text = activeQueue.Count.ToString();
        }
    }

    public void AddPatient()
    {
        string newName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogWarning("Patient name cannot be empty.");
            return;
        }

        PatientData newPatient = new PatientData
        {
            tokenID = dailyTokenCounter,
            patientName = newName,
            arrivalTime = DateTime.Now.ToString("h:mm tt")
        };

        activeQueue.Add(newPatient);
        dailyTokenCounter++;

        SpawnRowInUI(newPatient);

        // --- NEW: Update the metric counter when adding ---
        UpdateWaitingNowCount();

        SaveData();

        nameInputField.text = "";
        addPatientPanel.SetActive(false);
    }

    private void SpawnRowInUI(PatientData data)
    {
        GameObject newRow = Instantiate(waitingListRowPrefab, waitingListContentArea);
        WaitlistRowUI rowScript = newRow.GetComponent<WaitlistRowUI>();
        if (rowScript != null)
        {
            // Pass 'this' manager to the row so the Consult button works
            rowScript.SetupRow(data.tokenID, data.patientName, data.arrivalTime, this);
        }
    }

    public void CancelAddPatient()
    {
        nameInputField.text = "";
        addPatientPanel.SetActive(false);
    }

    public void StartConsultation(int clickedTokenID, GameObject rowUIObject)
    {
        PatientData patientToConsult = activeQueue.Find(p => p.tokenID == clickedTokenID);

        if (patientToConsult != null)
        {
            if (currentBannerTokenText != null)
                currentBannerTokenText.text = patientToConsult.tokenID.ToString();

            if (currentBannerNameText != null)
                currentBannerNameText.text = patientToConsult.patientName;

            activeQueue.Remove(patientToConsult);

            // --- NEW: Update the metric counter when removing ---
            UpdateWaitingNowCount();

            SaveData();
            Destroy(rowUIObject);
        }
    }

    // ==========================================
    // DATA PERSISTENCE (SAVE / LOAD)
    // ==========================================

    private void SaveData()
    {
        WaitlistSaveData dataToSave = new WaitlistSaveData
        {
            savedDate = DateTime.Now.ToString("yyyy-MM-dd"),
            savedTokenCounter = dailyTokenCounter,
            savedQueue = activeQueue
        };

        string json = JsonUtility.ToJson(dataToSave, true);
        File.WriteAllText(saveFilePath, json);
    }

    private void LoadData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            WaitlistSaveData loadedData = JsonUtility.FromJson<WaitlistSaveData>(json);

            if (loadedData.savedDate == DateTime.Now.ToString("yyyy-MM-dd"))
            {
                activeQueue = loadedData.savedQueue;
                dailyTokenCounter = loadedData.savedTokenCounter;

                foreach (PatientData patient in activeQueue)
                {
                    SpawnRowInUI(patient);
                }

                // --- NEW: Set correct metric number after loading from hard drive ---
                UpdateWaitingNowCount();
            }
            else
            {
                activeQueue.Clear();
                dailyTokenCounter = 1;

                // --- NEW: Reset metric to 0 on a new day ---
                UpdateWaitingNowCount();
                SaveData();
            }
        }
        else
        {
            // If the file doesn't exist at all (very first time running the app)
            UpdateWaitingNowCount();
        }
    }
}