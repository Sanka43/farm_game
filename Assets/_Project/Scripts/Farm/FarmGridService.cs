using Meadowbrook.Core;

namespace Meadowbrook.Farm
{
    public interface IFarmGridService
    {
        FarmGrid Grid { get; }
        GridConfig Config { get; }
    }

    /// <summary>Builds and owns the FarmGrid from a GridConfig.</summary>
    public sealed class FarmGridService : IFarmGridService
    {
        public FarmGrid Grid { get; }
        public GridConfig Config { get; }

        public FarmGridService(GridConfig config)
        {
            Config = config;
            Grid = new FarmGrid(config.Width, config.Height, config.DefaultTileId);
            var r = config.InitialUnlocked;
            Grid.SetUnlockedRect(r.x, r.y, r.width, r.height, true);
            EventBus.Publish(new GridBuiltEvent { Width = config.Width, Height = config.Height });
        }
    }
}
