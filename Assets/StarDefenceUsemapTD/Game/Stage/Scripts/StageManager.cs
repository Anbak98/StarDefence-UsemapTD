using STARTD.Common;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STARTD.Core.Stage
{
    public enum ETileBase
    {
        Load = 0,
        Base = 1,
        Portal = 2,
        NormalBlock = 3,
        FixBlock = 4,
        StageBlock = 5
    }

    public class StageManager : SingletonBehaviour<StageManager>
    {
        public Stage CurStage {get; private set;}

        private StageHandler _handler;

        [SerializeField] private TileBase[] tileBases;
        [field: SerializeField] public Tilemap TileMap { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _handler = new StageHandler(new StageLoader(tileBases), new StageCreator());
        }

        private void Start()
        {
            CurStage = _handler.Load(TileMap);
            TilemapDebugText debugText = new TilemapDebugText();
            debugText.Init(TileMap, CurStage);
            //_handler.Create(curStage);
        }
    }
}