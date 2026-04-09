using UnityEngine;
using TMPro;
using System;
using System.Collections;

public class ClockManager : MonoBehaviour
{
    [Header("UI Text References")]
    [Tooltip("Drag the TimeText object here")]
    public TMP_Text timeText;

    [Tooltip("Drag the DateText object here")]
    public TMP_Text dateText;

    [Tooltip("Drag the DayText object here")]
    public TMP_Text dayText;

    void Start()
    {
        // Start the highly optimized coroutine loop
        StartCoroutine(UpdateDateTimeRoutine());
    }

    private IEnumerator UpdateDateTimeRoutine()
    {
        // Infinite loop, but it yields, so it doesn't freeze the game
        while (true)
        {
            // Fetch the current system date and time
            DateTime now = DateTime.Now;

            // 1. Format Time: "h:mm tt" generates "11:11 AM" or "2:05 PM"
            // (The single 'h' ensures there is no leading zero for single-digit hours)
            if (timeText != null)
                timeText.text = now.ToString("h:mm tt");

            // 2. Format Date: "d MMM, yyyy" generates "1 Jan, 2026"
            if (dateText != null)
                dateText.text = now.ToString("d MMM, yyyy");

            // 3. Format Day: "dddd" generates the full day name like "Friday"
            if (dayText != null)
                dayText.text = now.ToString("dddd");

            // Put the coroutine to sleep for 1 second before running the loop again.
            // This is drastically more efficient than using the Update() method.
            yield return new WaitForSeconds(1f);
        }
    }
}