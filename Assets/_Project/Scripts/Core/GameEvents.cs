using Meadowbrook.Farm;

namespace Meadowbrook.Core
{
    /// <summary>The grid model has been created and is ready to be displayed.</summary>
    public struct GridBuiltEvent
    {
        public int Width;
        public int Height;
    }

    /// <summary>The player tapped a cell inside the grid bounds.</summary>
    public struct CellTappedEvent
    {
        public GridCoord Coord;
    }

    /// <summary>The selected cell changed.</summary>
    public struct CellSelectedEvent
    {
        public GridCoord Coord;
        public GridCell Cell;
    }
}
