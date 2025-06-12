using UnityEngine;

namespace PipeFitter.Assembly
{
    public class PFHangerSetup : MonoBehaviour
    {
        public GameObject InitialDecal;
        public GameObject FinalDecal;
        public void SetupHangerDecals()
        {
            FinalDecal.SetActive(false);
            InitialDecal.SetActive(true);
        }
        public void HangerUpSwapDecal()
        {
            FinalDecal.SetActive(true);
            InitialDecal.SetActive(false);
        }
    }
}
