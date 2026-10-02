using UnityEngine;

namespace Meadowbrook.CameraSystem
{
    [CreateAssetMenu(menuName = "Meadowbrook/Camera Config")]
    public sealed class CameraConfig : ScriptableObject
    {
        public float MinOrthoSize = 3f;
        public float MaxOrthoSize = 12f;
        [Tooltip("Extra world units shown around the starter farm on start.")]
        public float FitMargin = 0.75f;
        [Tooltip("World units of empty space allowed around the whole grid.")]
        public float BoundsPadding = 2f;
        [Tooltip("How quickly pan momentum fades (higher = stops sooner).")]
        public float InertiaDamping = 6f;
        public Color BackgroundColor = new Color(0.36f, 0.62f, 0.40f);

        public static CameraConfig CreateDefault() => CreateInstance<CameraConfig>();
    }
}
