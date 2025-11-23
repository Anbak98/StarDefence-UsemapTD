using STARTD.Core.Stage;

namespace STARTD.Core
{
    public interface IPreparedGame
    {
        Stage.Stage Stage { get; }
        void StartGame();   
    }
}