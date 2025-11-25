using STARTD.Game.Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace STARTD.Game
{
    public class BuyButton : MonoBehaviour
    {
        [SerializeField] private TMP_Text priceText;
        [SerializeField] private Button button;
        [SerializeField] private Image image;

        public void SetBuyButton(int price, Action buttonEvent, bool valid)
        {
            priceText.text = price.ToString();
            if(GameScene.Singleton.game.Gold.Cur >= price && valid)
            {
                image.color = Color.green;
            }
            else
            {
                image.color = Color.red;
            }

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => {
                if (GameScene.Singleton.TryRemoveGold(price))
                {
                    buttonEvent?.Invoke();
                }
                GameScene.Singleton.DisableBuyButton();
            });
        }
    }
}
