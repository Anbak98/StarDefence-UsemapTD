using STARTD.Core.Stage;
using STARTD.Game.Economy;
using UnityEngine;

namespace STARTD.Core
{
    public class PreparedGame : IPreparedGame
    {
        public Life Life { get; private set; }
        public Mineral Mineral { get; private set; }
        public Gold Gold { get; private set; }

        public PreparedGame(int stageIdx)
        {
            Life = new Life(1000);
            Mineral = new Mineral(0);
            Gold = new Gold(1000);
        }

        public Stage.Stage Stage => throw new System.NotImplementedException();

        public void StartGame()
        {
            SDSceneLoader.LoadScene();
        }
    }
}
