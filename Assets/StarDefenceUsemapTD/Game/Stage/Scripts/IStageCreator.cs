namespace STARTD.Core.Stage
{
    public interface IStageCreator
    {
        bool ChangeTile(int x, int y, int tileIdx);
    }
}