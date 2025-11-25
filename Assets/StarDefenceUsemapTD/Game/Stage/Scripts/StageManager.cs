using STARTD.Common;
using STARTD.Game.Player;
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
        [SerializeField] private GameObject playerBase;
        [field: SerializeField] public Tilemap TileMap { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _handler = new StageHandler(new StageLoader(tileBases), new StageCreator(tileBases));
        }

        private void Start()
        {
            CurStage = _handler.Load(TileMap);
            TilemapDebugText debugText = new TilemapDebugText();
            debugText.Init(TileMap, CurStage);
            //_handler.Create(curStage);

            Vector3 spawnPos = StageManager.Singleton.TileMap.CellToWorld(
                        new Vector3Int(CurStage.player.x + StageManager.Singleton.TileMap.cellBounds.xMin, CurStage.player.y + StageManager.Singleton.TileMap.cellBounds.yMin, 0)
                    );
            // 타일의 정중앙으로 이동
            spawnPos += new Vector3(TileMap.cellSize.x, TileMap.cellSize.y, 0) * 0.5f;

            GameObject player = Instantiate(playerBase, spawnPos, Quaternion.identity);
            HealthBarManager.Singleton.CreateHealthBar(player.transform, Color.green);
            GameScene.Singleton.playerBaseTransform = player.transform;
        }

        public bool ChangeTile(int x, int y, int tileIdx)
        {
            return _handler.Change(x, y, tileIdx);
        }
    }
}