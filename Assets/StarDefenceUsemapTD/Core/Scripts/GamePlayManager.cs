using UnityEngine;

namespace STARTD.Core
{
    public static class GamePlayManager
    {
        public static IPreparedGame PreparedGame;

        public static bool PrepareGame(out IPreparedGame prepared, int stageIdx)
        {
            prepared = null;

            if(stageIdx == 11)
            {
                prepared = new PreparedGame(stageIdx);
                PreparedGame = prepared;

                return true;
            }

            return false;
        }
    }
}