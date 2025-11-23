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

        public Stage Load(int stageIdx) => loader.LoadStage(stageIdx);
        public bool Create(Stage data) => creator.CreateStageTiles(data);
    }
}