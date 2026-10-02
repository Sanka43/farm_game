using UnityEngine;

namespace Meadowbrook.Inputs
{
    [CreateAssetMenu(menuName = "Meadowbrook/Input Config")]
    public sealed class InputConfig : ScriptableObject
    {
        [Tooltip("Max press time (seconds) to count as a tap.")]
        public float TapMaxSeconds = 0.35f;
        [Tooltip("Movement (in inches, DPI-scaled) before a press becomes a drag.")]
        public float DragThresholdInches = 0.08f;
        [Tooltip("Mouse wheel zoom step per notch (editor/desktop).")]
        public float ScrollZoomStep = 0.1f;

        public static InputConfig CreateDefault() => CreateInstance<InputConfig>();
    }
}
