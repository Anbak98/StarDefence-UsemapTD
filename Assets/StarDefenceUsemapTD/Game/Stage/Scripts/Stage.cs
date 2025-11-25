
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace STARTD.Core.Stage
{
    public struct Navigation
    {
        public int x, y;
        public int nx, ny;
    }

    public class Stage
    {
        public readonly int[,] tileIdxs;
        public readonly Navigation[,] navigations;
        public readonly List<Navigation> portalNavigations;

        public Stage(int[,] tileIdxs, Navigation[,] navigations, List<Navigation> portalNavigations)
        {
            this.tileIdxs = tileIdxs;
            this.navigations = navigations;
            this.portalNavigations = portalNavigations;
    }
    }
} 