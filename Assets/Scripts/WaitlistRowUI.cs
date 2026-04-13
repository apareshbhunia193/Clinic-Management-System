using UnityEngine;
using TMPro;
using UnityEngine.UI; // Required for the Button

public class WaitlistRowUI : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI tokenText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI timeText;

    [Header("Action Button")]
    public Button consultButton; // Drag your Mint Green button here in the prefab

    private int myTokenID;
    private WaitlistManager myManager;

    // We updated this method to accept the raw integer ID and the Manager script
    public void SetupRow(int tokenID, string patientName, string arrivalTime, WaitlistManager manager)
    {
        myTokenID = tokenID;
        myManager = manager;

        if (tokenText != null) tokenText.text = tokenID.ToString();
        if (nameText != null) nameText.text = patientName;
        if (timeText != null) timeText.text = arrivalTime;

        // Clear any old clicks (good practice) and assign the new click event via code
        consultButton.onClick.RemoveAllListeners();
        consultButton.onClick.AddListener(OnConsultClicked);
    }

    private void OnConsultClicked()
    {
        // Tell the manager to start the consult, and pass THIS visual GameObject to be destroyed
        if (myManager != null)
        {
            myManager.StartConsultation(myTokenID, this.gameObject);
        }
    }
}