using UnityEngine;

public class AppManager : MonoBehaviour
{
    [Header("Performance Settings")]
    [Tooltip("30 FPS is highly recommended for UI-only desktop applications.")]
    public int targetFPS = 30;

    void Awake()
    {
        // CRITICAL STEP: You must turn off VSync in code. 
        // If VSync is left on, Unity will completely ignore your targetFrameRate 
        // and force the app to run at the monitor's refresh rate (often 60 or 144).
        QualitySettings.vSyncCount = 0;

        // Apply the frame rate cap
        Application.targetFrameRate = targetFPS;

        // PRO-TIP FOR DESKTOP SOFTWARE:
        // By default, Unity apps freeze or pause when you click on another window.
        // Setting this to true ensures the clock keeps ticking and data keeps syncing 
        // even if minimizes the app to check Google or open an email.
        Application.runInBackground = true;
    }
}