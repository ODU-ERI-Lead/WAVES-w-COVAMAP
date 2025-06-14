using UnityEngine;
using FuzzPhyte.Tools.Samples;
namespace PipeFitter.Assembly
{
    public class PFWeldEffect : PFEffect
    {
        [Space]
        [Header("Specific Tool Effect")]
        public FP_AttachTool ToolReference;
       
        public override void OnEnable()
        {
            base.OnEnable();
            if (ToolReference != null) 
            {
                ToolReference.AttachToolSuccess += GotToolData;
            }
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (ToolReference != null)
            {
                ToolReference.AttachToolSuccess -= GotToolData;
            }
        }

        public void GotToolData(Vector3 worldStartLocation)
        {
            RunEffect(worldStartLocation);
        }
    }
}
