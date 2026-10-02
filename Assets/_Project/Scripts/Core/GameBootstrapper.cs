using Meadowbrook.CameraSystem;
using Meadowbrook.Farm;
using Meadowbrook.Inputs;
using Meadowbrook.UI;
using UnityEngine;

namespace Meadowbrook.Core
{
    /// <summary>
    /// Builds the whole Phase 1 farm at startup, in whatever scene is loaded. Configs are loaded
    /// from Resources/Configs/* if present, otherwise defaults are used.
    /// </summary>
    public static class GameBootstrapper
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        static void Boot()
        {
            if (GameObject.Find("Game") != null) return;

            ServiceLocator.Clear();
            Application.targetFrameRate = 60;
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            var gridConfig = Resources.Load<GridConfig>("Configs/GridConfig") ?? GridConfig.CreateDefault();
            var cameraConfig = Resources.Load<CameraConfig>("Configs/CameraConfig") ?? CameraConfig.CreateDefault();
            var inputConfig = Resources.Load<InputConfig>("Configs/InputConfig") ?? InputConfig.CreateDefault();

            var root = new GameObject("Game");
            Object.DontDestroyOnLoad(root);

            // Services
            var inputService = root.AddComponent<TouchInputService>();
            inputService.Init(inputConfig);
            var farm = new FarmGridService(gridConfig);
            ServiceLocator.Register<IInputService>(inputService);
            ServiceLocator.Register<IFarmGridService>(farm);

            // Camera
            var cam = Camera.main;
            if (cam == null)
            {
                var camGo = new GameObject("Main Camera") { tag = "MainCamera" };
                cam = camGo.AddComponent<Camera>();
                camGo.AddComponent<AudioListener>();
                camGo.transform.SetParent(root.transform);
            }
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = cameraConfig.BackgroundColor;

            float cs = gridConfig.CellSize;
            var worldBounds = new Rect(0f, 0f, gridConfig.Width * cs, gridConfig.Height * cs);
            var u = gridConfig.InitialUnlocked;
            var focus = new Rect(u.x * cs, u.y * cs, u.width * cs, u.height * cs);
            var camController = cam.gameObject.AddComponent<FarmCameraController>();
            camController.Init(cam, cameraConfig, inputService, worldBounds, focus);

            // World
            var gridGo = new GameObject("GridView");
            gridGo.transform.SetParent(root.transform);
            gridGo.AddComponent<GridView>().Init(farm.Grid, gridConfig);

            var highlightGo = new GameObject("SelectionHighlight");
            highlightGo.transform.SetParent(root.transform);
            highlightGo.AddComponent<SelectionHighlight>().Init(cs);

            // Interaction + debug UI
            root.AddComponent<FarmInteractionController>().Init(cam, inputService, farm);
            root.AddComponent<DebugOverlay>();
        }
    }
}
