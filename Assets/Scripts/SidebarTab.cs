using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class SidebarTab : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Visuals")]
    [Tooltip("Drag the Image component of THIS specific panel here")]
    public Image targetBackground;

    [Header("Tab Colors (CRITICAL: Make sure Alpha (A) is 255!)")]
    public Color normalColor = new Color32(10, 25, 20, 0); // Default to transparent so it blends with the parent
    public Color hoverColor = new Color32(20, 45, 35, 255);  // Solid Lighter Green
    public Color activeColor = new Color32(35, 75, 55, 255); // Solid Highlighted Green

    [Header("Tab Settings")]
    public bool isDefaultTab = false;

    [Header("Button Functionality")]
    public UnityEvent onTabSelected;

    private bool isActiveTab = false;
    private static SidebarTab currentActiveTab;

    void Awake()
    {
        // Safety check: If you forgot to drag the image in the inspector, try to find it
        if (targetBackground == null)
        {
            targetBackground = GetComponent<Image>();
        }

        // Apply normal color on startup
        if (targetBackground != null && !isActiveTab)
        {
            targetBackground.color = normalColor;
        }
    }

    void Start()
    {
        if (isDefaultTab)
        {
            SelectTab();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isActiveTab && targetBackground != null)
            targetBackground.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isActiveTab && targetBackground != null)
            targetBackground.color = normalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectTab();
    }

    public void SelectTab()
    {
        if (currentActiveTab != null && currentActiveTab != this)
        {
            currentActiveTab.Deselect();
        }

        isActiveTab = true;
        if (targetBackground != null)
            targetBackground.color = activeColor;

        currentActiveTab = this;
        onTabSelected.Invoke();
    }

    public void Deselect()
    {
        isActiveTab = false;
        if (targetBackground != null)
            targetBackground.color = normalColor;
    }
}