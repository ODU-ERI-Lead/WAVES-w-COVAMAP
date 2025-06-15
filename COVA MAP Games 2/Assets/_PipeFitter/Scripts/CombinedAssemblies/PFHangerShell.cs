using UnityEngine;
using UnityEngine.Events;
using PipeFitter.Assembly;
public class PFHangerShell : MonoBehaviour
{
    public delegate void OnCorrectHangerPlacement(Vector3 location);
    public event OnCorrectHangerPlacement CorrectHangerPlacement;
    public PFHangerEffect TheEffect;
    public void HangerPlaced(Vector3 location)
    {
        this.transform.position = location;
        CorrectHangerPlacement?.Invoke(location);
        TheEffect.RunEffect(location);
    }
}
