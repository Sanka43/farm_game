using Meadowbrook.Inputs;
using UnityEngine;

namespace Meadowbrook.CameraSystem
{
    /// <summary>Pan (with inertia) and pinch-zoom for an orthographic camera, clamped to the farm bounds.</summary>
    public sealed class FarmCameraController : MonoBehaviour
    {
        const float CameraZ = -10f;

        Camera cam;
        CameraConfig config;
        IInputService input;
        Rect bounds;
        Vector2 inertia; // world units / second
        bool dragging;

        public void Init(Camera camera, CameraConfig cameraConfig, IInputService inputService, Rect worldBounds, Rect focusRect)
        {
            cam = camera;
            config = cameraConfig;
            input = inputService;
            bounds = new Rect(
                worldBounds.xMin - config.BoundsPadding,
                worldBounds.yMin - config.BoundsPadding,
                worldBounds.width + 2f * config.BoundsPadding,
                worldBounds.height + 2f * config.BoundsPadding);

            input.DragBegan += OnDragBegan;
            input.Drag += OnDrag;
            input.DragEnded += OnDragEnded;
            input.Pinch += OnPinch;

            FitTo(focusRect);
        }

        void OnDestroy()
        {
            if (input == null) return;
            input.DragBegan -= OnDragBegan;
            input.Drag -= OnDrag;
            input.DragEnded -= OnDragEnded;
            input.Pinch -= OnPinch;
        }

        void FitTo(Rect r)
        {
            float size = Mathf.Max(r.width * 0.5f / cam.aspect, r.height * 0.5f) + config.FitMargin;
            cam.orthographicSize = Mathf.Clamp(size, config.MinOrthoSize, config.MaxOrthoSize);
            transform.position = new Vector3(r.center.x, r.center.y, CameraZ);
            ClampPosition();
        }

        Vector3 ScreenToWorld(Vector2 screen) => cam.ScreenToWorldPoint(new Vector3(screen.x, screen.y, -CameraZ));

        void OnDragBegan(Vector2 pos)
        {
            dragging = true;
            inertia = Vector2.zero;
        }

        void OnDrag(Vector2 pos, Vector2 deltaPx)
        {
            // Keep the world point under the finger fixed.
            Vector3 worldDelta = ScreenToWorld(pos) - ScreenToWorld(pos - deltaPx);
            transform.position -= new Vector3(worldDelta.x, worldDelta.y, 0f);
            ClampPosition();
        }

        void OnDragEnded(Vector2 velocityPx)
        {
            dragging = false;
            float worldPerPixel = 2f * cam.orthographicSize / Screen.height;
            inertia = -velocityPx * worldPerPixel;
        }

        void OnPinch(Vector2 center, float ratio)
        {
            Vector3 before = ScreenToWorld(center);
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize / ratio, config.MinOrthoSize, config.MaxOrthoSize);
            Vector3 after = ScreenToWorld(center);
            transform.position += before - after; // zoom towards the pinch centre
            ClampPosition();
        }

        void Update()
        {
            if (dragging || inertia.sqrMagnitude < 0.0001f) return;
            float dt = Time.unscaledDeltaTime;
            transform.position += new Vector3(inertia.x, inertia.y, 0f) * dt;
            inertia *= Mathf.Exp(-config.InertiaDamping * dt);
            ClampPosition();
        }

        void ClampPosition()
        {
            float halfH = cam.orthographicSize;
            float halfW = halfH * cam.aspect;
            Vector3 p = transform.position;
            p.x = bounds.width <= 2f * halfW ? bounds.center.x : Mathf.Clamp(p.x, bounds.xMin + halfW, bounds.xMax - halfW);
            p.y = bounds.height <= 2f * halfH ? bounds.center.y : Mathf.Clamp(p.y, bounds.yMin + halfH, bounds.yMax - halfH);
            p.z = CameraZ;
            transform.position = p;
        }
    }
}
