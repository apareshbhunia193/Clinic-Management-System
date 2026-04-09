using UnityEngine;

public class LogoutHandler : MonoBehaviour
{
    [Tooltip("Refrence of logout panel")]
    [SerializeField] GameObject logoutPanel;
    bool isActive = false;

    private void Awake()
    {
        logoutPanel.SetActive(isActive);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickDP()
    {
        logoutPanel.SetActive(!isActive);
        isActive = !isActive;
    }

    public void OnClickLogout() { 
        Application.Quit();
    }
}
