using STARTD.Core.Stage;
using STARTD.Game.Economy;

namespace STARTD.Core
{
    public interface IPreparedGame
    {
        Life Life { get; }
        Mineral Mineral { get; }
        Gold Gold { get; }
        Stage.Stage Stage { get; }
        void StartGame();
        void RestartGame();
        void ExitGame();
    }
}