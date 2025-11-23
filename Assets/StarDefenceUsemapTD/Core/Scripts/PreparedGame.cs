using STARTD.Core.Stage;
using UnityEngine;

namespace STARTD.Core
{
    public class PreparedGame : IPreparedGame
    {
        public PreparedGame(int stageIdx)
        {

        }

        public Stage.Stage Stage => throw new System.NotImplementedException();

        public void StartGame()
        {
            throw new System.NotImplementedException();
        }
    }
}
