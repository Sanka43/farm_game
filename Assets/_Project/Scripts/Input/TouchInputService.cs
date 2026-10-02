using System;
using UnityEngine;

namespace Meadowbrook.Inputs
{
    /// <summary>
    /// Touch gestures (tap, drag, pinch) using Unity's legacy Input API, with mouse emulation
    /// in the editor/desktop. Swap this class for a new-Input-System version later without
    /// changing any consumer, because they only see IInputService.
    /// </summary>
    public sealed class TouchInputService : MonoBehaviour, IInputService
    {
        public event Action<Vector2> Tap;
        public event Action<Vector2> DragBegan;
        public event Action<Vector2, Vector2> Drag;
        public event Action<Vector2> DragEnded;
        public event Action<Vector2, float> Pinch;

        enum Phase { Began, Moved, Ended }

        InputConfig config;
        bool pressing, dragging, ignoreSingle, pinching;
        Vector2 startPos, lastPos, velocity;
        float startTime, lastMoveTime, prevPinchDist;

        public void Init(InputConfig inputConfig)
        {
            config = inputConfig;
            Input.simulateMouseWithTouches = false;
            Input.multiTouchEnabled = true;
        }

        float DragThresholdPx => config.DragThresholdInches * (Screen.dpi > 0f ? Screen.dpi : 160f);

        void Update()
        {
            int count = Input.touchCount;

            if (count >= 2)
            {
                HandlePinch();
                return;
            }

            if (count == 1)
            {
                pinching = false;
                if (ignoreSingle) return; // finger left over after a pinch
                var t = Input.GetTouch(0);
                var phase = t.phase == TouchPhase.Began ? Phase.Began
                    : (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) ? Phase.Ended
                    : Phase.Moved;
                HandlePointer(phase, t.position);
                return;
            }

            pinching = false;
            ignoreSingle = false;
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouse();
#endif
        }

        void HandleMouse()
        {
            Vector2 m = Input.mousePosition;
            if (Input.GetMouseButtonDown(0)) HandlePointer(Phase.Began, m);
            else if (Input.GetMouseButtonUp(0)) HandlePointer(Phase.Ended, m);
            else if (Input.GetMouseButton(0)) HandlePointer(Phase.Moved, m);

            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
                Pinch?.Invoke(m, Mathf.Max(0.1f, 1f + scroll * config.ScrollZoomStep));
        }

        void HandlePointer(Phase phase, Vector2 pos)
        {
            switch (phase)
            {
                case Phase.Began:
                    pressing = true;
                    dragging = false;
                    startPos = lastPos = pos;
                    startTime = lastMoveTime = Time.unscaledTime;
                    velocity = Vector2.zero;
                    break;

                case Phase.Moved:
                    if (!pressing) return;
                    if (!dragging && (pos - startPos).magnitude > DragThresholdPx)
                    {
                        dragging = true;
                        lastPos = pos; // no jump when the drag starts
                        DragBegan?.Invoke(pos);
                        return;
                    }
                    if (dragging)
                    {
                        Vector2 delta = pos - lastPos;
                        float dt = Mathf.Max(Time.unscaledDeltaTime, 0.0001f);
                        velocity = Vector2.Lerp(velocity, delta / dt, 0.35f);
                        lastMoveTime = Time.unscaledTime;
                        Drag?.Invoke(pos, delta);
                        lastPos = pos;
                    }
                    break;

                case Phase.Ended:
                    if (!pressing) return;
                    pressing = false;
                    if (dragging)
                    {
                        dragging = false;
                        bool stale = Time.unscaledTime - lastMoveTime > 0.08f;
                        DragEnded?.Invoke(stale ? Vector2.zero : velocity);
                    }
                    else if (Time.unscaledTime - startTime <= config.TapMaxSeconds)
                    {
                        Tap?.Invoke(pos);
                    }
                    break;
            }
        }

        void HandlePinch()
        {
            if (pressing)
            {
                if (dragging) DragEnded?.Invoke(Vector2.zero);
                pressing = false;
                dragging = false;
            }
            ignoreSingle = true;

            var a = Input.GetTouch(0);
            var b = Input.GetTouch(1);
            float dist = Vector2.Distance(a.position, b.position);
            if (pinching && prevPinchDist > 1f && dist > 1f)
                Pinch?.Invoke((a.position + b.position) * 0.5f, dist / prevPinchDist);
            prevPinchDist = dist;
            pinching = true;
        }
    }
}
