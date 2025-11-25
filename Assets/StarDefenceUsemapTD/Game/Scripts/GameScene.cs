using STARTD.Core;
using STARTD.Core.Stage;
using STARTD.Game.Tower;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace STARTD.Game.Player
{
    public class GameScene : BaseScene<GameScene>
    {
        public IPreparedGame game;
        public Transform playerBaseTransform;
        public GameObject defeatedPopup;
        public GameObject winPopup;
        [SerializeField] private TMP_Text gold;
        [SerializeField] private TMP_Text mineral;
        [SerializeField] private BuyButton buyButton;

        protected override void Awake()
        {
            if(GamePlayManager.PreparedGame != null)
            {
                game = GamePlayManager.PreparedGame;
            }  
            else
            {
                game = new PreparedGame(11);
            }

            gold.text = game.Gold.Cur.ToString();
            mineral.text = game.Mineral.Cur.ToString();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                DisableBuyButton();

                Vector3 mousePos = Input.mousePosition;
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

                Vector3Int cellPos = StageManager.Singleton.TileMap.WorldToCell(worldPos);


                // 타일맵 Bounds 가져오기
                BoundsInt bounds = StageManager.Singleton.TileMap.cellBounds;

                // 배열 인덱스로 변환 (0,0부터 시작)
                int mx = cellPos.x - bounds.xMin;
                int my = cellPos.y - bounds.yMin;

                if(mx >= 0 && my >= 0 && mx < StageManager.Singleton.CurStage.tileIdxs.GetLength(0) && my < StageManager.Singleton.CurStage.tileIdxs.GetLength(1))
                {
                    if (StageManager.Singleton.CurStage.tileIdxs[mx, my] == (int)ETileBase.NormalBlock)
                    {
                        TowerManager.Singleton.TryBuildTower(cellPos.x, cellPos.y);
                    }
                    else if (StageManager.Singleton.CurStage.tileIdxs[mx, my] == (int)ETileBase.FixBlock)
                    {
                        // 타일 월드 좌표 가져오기 (정중앙)
                        Vector3 worldPoss =
                            StageManager.Singleton.TileMap.GetCellCenterWorld(new Vector3Int(cellPos.x, cellPos.y, 0));

                        SetBuyButton(
                            300,
                            () => StageManager.Singleton.ChangeTile(cellPos.x, cellPos.y, (int)ETileBase.NormalBlock),
                            worldPoss);
                    }

                    Debug.Log($"클릭한 타일 좌표 (Tilemap): {StageManager.Singleton.CurStage.tileIdxs[mx, my]}   /   (Matrix): {mx},{my}");


                }
            }
        }

        public void PlayerLifeDown(int amount)
        {
            game.Life.Remove(amount, out bool IsZero);
            HealthBarManager.Singleton.SetHealth(playerBaseTransform, game.Life.Cur, game.Life.Max);
            if (IsZero)
            {
                LoseGame();
            }
        }

        public bool TryAddGold(int amount)
        {
            if (game.Gold.Add(amount))
            {
                gold.text = game.Gold.Cur.ToString();
                return true;
            }

            return false;
        }

        public bool TryRemoveGold(int amount)
        {
            if (game.Gold.Remove(amount, out bool IsZero))
            {
                gold.text = game.Gold.Cur.ToString();
                return true;
            }

            return false;            
        }

        public void WinGame()
        {
            winPopup.SetActive(true);
        }

        private void LoseGame()
        {
            defeatedPopup.SetActive(true);
        }

        public void RestartGame()
        {
            game.RestartGame();
        }    

        public void ExitGame()
        {
            game.ExitGame();
        }

        public void SetBuyButton(int price, Action buttonEvent, Vector3 worldPos, bool valid = true)
        {
            buyButton.SetBuyButton(price, buttonEvent, valid);
            buyButton.gameObject.transform.position = worldPos + new Vector3(0, 0.3f, 0);
            buyButton.gameObject.SetActive(true);
        }

        public void DisableBuyButton()
        {
            buyButton.gameObject.SetActive(false);
        }
    }
}
