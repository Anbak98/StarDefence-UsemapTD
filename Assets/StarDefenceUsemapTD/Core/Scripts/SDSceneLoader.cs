using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace STARTD.Core
{
    public static class SDSceneLoader
    {
        public static void LoadScene()
        {
            SceneManager.LoadScene(1);
        }
    }
}