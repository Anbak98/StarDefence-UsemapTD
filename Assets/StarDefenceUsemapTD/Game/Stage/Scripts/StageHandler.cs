using UnityEngine.Tilemaps;

namespace STARTD.Core.Stage
{
    public class StageHandler
    {
        private readonly IStageLoader loader;
        private readonly IStageCreator creator;

        public StageHandler(IStageLoader loader, IStageCreator creator)
        {
            this.loader = loader;
            this.creator = creator;
        }
        public Stage Load(Tilemap tiles) => loader.LoadStage(tiles);
        public bool Change(int x, int y, int tileIdx) => creator.ChangeTile(x, y, tileIdx);
    }
}