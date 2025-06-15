using UnityEngine;
using FuzzPhyte.Tools.Samples;
namespace PipeFitter.Assembly
{
    public class PFGrindEffect : PFEffect
    {
        [Space]
        [Header("Specific Tool Effect")]
        public FP_DetachTool ToolReference;

        public override void OnEnable()
        {
            base.OnEnable();
            if (ToolReference != null)
            {
                ToolReference.DetachToolSuccess += RunEffect;
                ToolReference.OnDetachToolActivated.AddListener(ToolActivated);
                ToolReference.OnDetachToolDeactivated.AddListener(ToolDeactivated);
            }
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (ToolReference != null)
            {
                ToolReference.DetachToolSuccess -= RunEffect;
                ToolReference.OnDetachToolActivated.RemoveListener(ToolActivated);
                ToolReference.OnDetachToolDeactivated.RemoveListener(ToolDeactivated);
            }
        }
    }
}
