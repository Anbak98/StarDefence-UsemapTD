using STARTD.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STARTD.Home
{
    public class HomeScene : BaseScene<HomeScene>
    {
        public void PlayGame()
        {
            if (GamePlayManager.PrepareGame(out var prepared, 11))
            {
                prepared.StartGame();
            }
        }
    }
}
