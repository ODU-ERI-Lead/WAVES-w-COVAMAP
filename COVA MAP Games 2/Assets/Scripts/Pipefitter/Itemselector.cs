using UnityEngine;

public class Itemselector : MonoBehaviour
{
    public static GameObject lastSelected;
    public LayerMask SelectableLayer;

    //potential item slect call to use.
    //public void SelectThisItem()
    // {
    //    lastSelected = gameObject;
    //    Debug.Log("Selected item: " + lastSelected.name);
    // }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f, SelectableLayer))
            {
                GameObject clickedObject = hit.collider.transform.root.gameObject;
                lastSelected = clickedObject;
                Debug.Log("Selected: " + lastSelected.name);
                Debug.LogWarning("Clicked: " + hit.collider.name);
                Debug.LogWarning("Selected root: " + hit.collider.transform.root.name);
            }
            
        }
    }




}
