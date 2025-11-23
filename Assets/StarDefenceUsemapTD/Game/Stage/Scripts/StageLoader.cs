namespace STARTD.Core.Stage
{
    public class StageLoader : IStageLoader
    {
        public Stage LoadStage(int stageIdx)
        {
            Stage loaded = new Stage()
            {
                tileIdxs = {}
            };

            return loaded;
        }
    }
}