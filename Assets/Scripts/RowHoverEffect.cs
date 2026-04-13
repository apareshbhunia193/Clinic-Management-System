using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RowHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Row Background Image")]
    public Image rowBackgroundImage;

    [Header("Hover Colors")]
    [Tooltip("Make sure Alpha is 0 if you want it to be transparent normally")]
    public Color normalColor = new Color32(20, 45, 35, 0);

    [Tooltip("A subtle lighter color for the highlight with Alpha at 255")]
    public Color hoverColor = new Color32(30, 65, 50, 255);

    void Awake()
    {
        // Auto-grab the image if you forget to assign it
        if (rowBackgroundImage == null)
        {
            rowBackgroundImage = GetComponent<Image>();
        }

        // Ensure it starts at the normal color
        if (rowBackgroundImage != null)
        {
            rowBackgroundImage.color = normalColor;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (rowBackgroundImage != null)
            rowBackgroundImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (rowBackgroundImage != null)
            rowBackgroundImage.color = normalColor;
    }
}