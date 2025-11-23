using UnityEngine;

namespace STARTD.Core
{
    public static class GamePlayManager
    {
        public static IPreparedGame preparedGame;

        public static bool PrepareGame(out IPreparedGame prepared, int stageIdx)
        {
            prepared = null;

            if(stageIdx == 11)
            {
                prepared = new PreparedGame(stageIdx);
                preparedGame = prepared;

                return true;
            }

            return false;
        }
    }
}