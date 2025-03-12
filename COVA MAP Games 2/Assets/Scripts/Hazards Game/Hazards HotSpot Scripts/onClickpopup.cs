using UnityEngine;
using TMPro;

public class onClickpopup : MonoBehaviour
{
  
    public GameObject popupTextPrefab; // Assign a TextMeshProUGUI prefab
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        ShowPopupText();
    }

    void ShowPopupText()
    {
        if (popupTextPrefab != null)
        {
            // Instantiate the pop-up text
            GameObject popupText = Instantiate(popupTextPrefab, transform.position, Quaternion.identity, transform.parent);

            // Convert world position to screen position
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position);
            popupText.transform.position = screenPosition;

            // Get TextMeshProUGUI component and set text
            TMP_Text textComponent = popupText.GetComponent<TMP_Text>();
            if (textComponent != null)
            {
                textComponent.text = "You clicked me!";
            }

            // Destroy the popup after 2 seconds
            Destroy(popupText, 2f);
        }
    }
}
