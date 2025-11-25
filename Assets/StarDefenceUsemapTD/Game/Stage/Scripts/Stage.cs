
using STARTD.Game.Tower;
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
        public readonly int[,] towerIdxs;
        public readonly List<TowerBehaviour>[,] towerRangeIdxs;
        public readonly Navigation[,] navigations;
        public readonly List<Navigation> portalNavigations;
        public readonly Navigation player;

        public Stage(int[,] tileIdxs, int[,] towerIdxs, Navigation[,] navigations, List<Navigation> portalNavigations, Navigation player)
        {
            this.tileIdxs = tileIdxs;
            this.towerIdxs = towerIdxs;
            this.towerRangeIdxs = new List<TowerBehaviour>[towerIdxs.GetLength(0), towerIdxs.GetLength(1)];

            for (int x = 0; x < towerIdxs.GetLength(0); x++)
            {
                for (int y = 0; y < towerIdxs.GetLength(1); y++)
                {
                    this.towerRangeIdxs[x, y] = new List<TowerBehaviour>();
                }
            }
            this.navigations = navigations;
            this.portalNavigations = portalNavigations;
            this.player = player;
        }
    }
} 