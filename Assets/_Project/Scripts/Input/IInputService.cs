using System;
using UnityEngine;

namespace Meadowbrook.Inputs
{
    /// <summary>
    /// Device-independent gestures. Everything in the game consumes this, never raw touches,
    /// so the backend (legacy Input, new Input System, tests) can be swapped.
    /// All positions/deltas are in screen pixels.
    /// </summary>
    public interface IInputService
    {
        /// <summary>Short press without movement.</summary>
        event Action<Vector2> Tap;
        /// <summary>Finger moved past the drag threshold.</summary>
        event Action<Vector2> DragBegan;
        /// <summary>Position and per-frame delta while dragging.</summary>
        event Action<Vector2, Vector2> Drag;
        /// <summary>Release velocity in pixels/second (zero if the pan was interrupted).</summary>
        event Action<Vector2> DragEnded;
        /// <summary>Pinch centre and scale ratio since last frame (>1 = zoom in). Mouse wheel also raises this.</summary>
        event Action<Vector2, float> Pinch;
    }
}
