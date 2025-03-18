using UnityEngine;
using UnityEngine.UI;
using TMPro;
using FuzzPhyte.Utility;

public class FractionText : MonoBehaviour
{
    [SerializeField] public TMP_Text fractiontext;
    [SerializeField] public int Totalclicks ;
  

    public void DisplayUpdated(int clicks, int OutOfValue)
    {
        Debug.LogWarning($"Austin finish this! {clicks} out of {OutOfValue}");
        fractiontext.text = clicks.ToString() + "/"+OutOfValue.ToString();
        //fractionText.text = some magical fraction thing you created based on the stuff coming in
    }
}
