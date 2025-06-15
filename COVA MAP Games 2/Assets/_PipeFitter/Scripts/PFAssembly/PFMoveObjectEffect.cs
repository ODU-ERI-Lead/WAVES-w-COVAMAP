using UnityEngine;
using FuzzPhyte.Tools.Samples;
using FuzzPhyte.Tools.Connections;
namespace PipeFitter.Assembly
{
    public class PFMoveObjectEffect : PFEffect
    {
        [Space]
        [Header("Specific Tool Effect")]
        public FP_PanMove ToolReference;

        public override void OnEnable()
        {
            base.OnEnable();
            if (ToolReference != null)
            {
                ToolReference.OnToolActivatedUnityEvent.AddListener(ToolActivated);
                ToolReference.OnToolDeactivatedUnityEvent.AddListener(ToolDeactivated);
            }
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (ToolReference != null)
            {
                ToolReference.OnToolActivatedUnityEvent.RemoveListener(ToolActivated);
                ToolReference.OnToolDeactivatedUnityEvent.RemoveListener(ToolDeactivated);
            }
        }

       
    }
}
