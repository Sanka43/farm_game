using Meadowbrook.Core;
using UnityEngine;

namespace Meadowbrook.UI
{
    /// <summary>Phase 1 debug text: selected cell and FPS. Remove or hide in later phases.</summary>
    public sealed class DebugOverlay : MonoBehaviour
    {
        string cellText = "Tap a tile";
        float fps;
        GUIStyle style;

        void OnEnable() => EventBus.Subscribe<CellSelectedEvent>(OnSelected);
        void OnDisable() => EventBus.Unsubscribe<CellSelectedEvent>(OnSelected);

        void OnSelected(CellSelectedEvent e)
        {
            cellText = "Cell " + e.Coord + " - " + e.Cell.TileTypeId + " - " + (e.Cell.IsUnlocked ? "Unlocked" : "Locked");
        }

        void Update()
        {
            fps = Mathf.Lerp(fps, 1f / Mathf.Max(Time.unscaledDeltaTime, 0.0001f), 0.1f);
        }

        void OnGUI()
        {
            if (style == null) style = new GUIStyle(GUI.skin.label);
            style.fontSize = Mathf.Max(14, Screen.height / 45);
            style.normal.textColor = Color.white;
            float pad = style.fontSize * 0.6f;
            float topInset = Screen.height - (Screen.safeArea.y + Screen.safeArea.height); // notch / status bar
            GUI.Label(new Rect(pad, pad + topInset, Screen.width - 2 * pad, style.fontSize * 3f),
                cellText + "\nFPS " + Mathf.RoundToInt(fps), style);
        }
    }
}
