using FuzzPhyte.Tools.Connections;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Trashbin : MonoBehaviour
{
    public string PartTag = "PartCollider";
    public FP_PanMove MoveTool;
    public UnityEvent OnItemDeletedEvent;
    public ParticleSystem DeleteEffect;
    public float ParticleSystemRunTime = 2f;
    private WaitForSecondsRealtime waitForDelayEffect;
    protected Coroutine vfxRunningCoroutine;
    public Animator TrashBinAnimator;

    public void Start()
    {
        if (DeleteEffect != null)
        {
            waitForDelayEffect = new WaitForSecondsRealtime(ParticleSystemRunTime);
        }
    }
    public void DeleteLastSelected()
    {
        if (Itemselector.lastSelected != null)
        {
            Debug.Log("Deleting: " + Itemselector.lastSelected.name);
            Destroy(Itemselector.lastSelected);
            Itemselector.lastSelected = null; // Clear the reference
        }
        else
        {
            Debug.Log("No item selected to delete.");
        }
    }
    //John
    //reference to the move pan tool to manually reset it
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(PartTag))
        {
            Debug.Log("Item entered trashbin: " + other.gameObject.name);
            // Optionally, you can delete the item immediately
            if (other.gameObject.GetComponent<FP_CollideItem>())
            {
                Destroy(other.gameObject.GetComponent<FP_CollideItem>().MoveRotateItem.gameObject);
                OnItemDeletedEvent.Invoke();
                if (vfxRunningCoroutine==null &&DeleteEffect != null)
                {
                    vfxRunningCoroutine= StartCoroutine(DelayVFXStop());
                }
            }
            if (MoveTool!=null) 
            {
                //MoveTool.ForceDeactivateTool(); 
            }
        }
    }
    IEnumerator DelayVFXStop()
    {
        yield return waitForDelayEffect;
        DeleteEffect.Stop();
        vfxRunningCoroutine = null; // Clear the reference to the coroutine
    }
    public void ActivateTrashBinAnimation()
    {
        if (TrashBinAnimator != null)
        {
            TrashBinAnimator.SetTrigger("TrashPart");
        }
    }
}
