using UnityEngine;
using UnityEngine.EventSystems;

public class HoverTextDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject textToShow;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (textToShow != null)
            textToShow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (textToShow != null)
            textToShow.SetActive(false);
    }
}