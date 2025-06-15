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
                ToolReference.AttachToolSuccess += RunEffect;
                ToolReference.OnAttachToolActivated.AddListener(ToolActivated);
                ToolReference.OnAttachToolDeactivated.AddListener(ToolDeactivated);
            }
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (ToolReference != null)
            {
                ToolReference.AttachToolSuccess -= RunEffect;
                ToolReference.OnAttachToolActivated.RemoveListener(ToolActivated);
                ToolReference.OnAttachToolDeactivated.RemoveListener(ToolDeactivated);
            }
        }

    }
}
