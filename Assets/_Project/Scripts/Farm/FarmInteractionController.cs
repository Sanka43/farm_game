using Meadowbrook.Core;
using Meadowbrook.Inputs;
using UnityEngine;

namespace Meadowbrook.Farm
{
    /// <summary>Turns taps into cell events. Later phases (crops etc.) react to these events.</summary>
    public sealed class FarmInteractionController : MonoBehaviour
    {
        Camera cam;
        IInputService input;
        IFarmGridService farm;

        public void Init(Camera camera, IInputService inputService, IFarmGridService farmService)
        {
            cam = camera;
            input = inputService;
            farm = farmService;
            input.Tap += OnTap;
        }

        void OnDestroy()
        {
            if (input != null) input.Tap -= OnTap;
        }

        void OnTap(Vector2 screenPos)
        {
            var world = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, -cam.transform.position.z));
            var coord = GridCoordUtility.WorldToCell(world, farm.Config.CellSize);
            var cell = farm.Grid.GetCell(coord);
            if (cell == null) return;

            EventBus.Publish(new CellTappedEvent { Coord = coord });
            EventBus.Publish(new CellSelectedEvent { Coord = coord, Cell = cell });
        }
    }
}
