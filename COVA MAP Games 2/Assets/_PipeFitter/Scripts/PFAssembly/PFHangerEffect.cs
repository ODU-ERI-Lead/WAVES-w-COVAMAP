using UnityEngine;

namespace PipeFitter.Assembly
{
    public class PFHangerEffect : PFEffect
    {
        [Space]
        [Header("Specific Tool Effect")]
        public PFHangerPlacementTool ToolReference;

        public override void OnEnable()
        {
            base.OnEnable();
            if (ToolReference != null)
            {
                //ToolReference.ClickedDown += RunEffect;
                ToolReference.OnHangerToolActivated.AddListener(ToolActivated);
                ToolReference.OnHangerToolDeactivated.AddListener(ToolDeactivated);
            }
        }
        public override void OnDisable()
        {
            base.OnDisable();
            if (ToolReference != null)
            {
                //ToolReference.ClickedDown -= RunEffect;
                ToolReference.OnHangerToolActivated.RemoveListener(ToolActivated);
                ToolReference.OnHangerToolDeactivated.RemoveListener(ToolDeactivated);
            }
        }
    }
}
