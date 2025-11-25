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
        public bool Create(Stage data) => creator.CreateStageTiles(data);
    }
}