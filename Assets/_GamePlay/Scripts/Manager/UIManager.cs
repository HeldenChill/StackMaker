using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Management
{
    using System;
    using UI;
    public class UIManager : MonoBehaviour
    {
        public event Action OnNextLevel;
        public event Action OnPlayAgain;
        public static UIManager Inst = null;

        [SerializeField]
        private InGameUI inGameUI;
        private void Awake()
        {
            if (Inst == null)
            {
                Inst = this;
                return;
            }
            Destroy(gameObject);
        }
        private void Start()
        {
            inGameUI.OnNextLevel += NextLevel;
            inGameUI.OnPlayAgain += PlayAgain;
            LevelManager.Inst.CurrentLevel.OnWinGame += WinGame;
        }

        private void OnDisable()
        {
            inGameUI.OnNextLevel -= NextLevel;
            inGameUI.OnPlayAgain -= PlayAgain;
            LevelManager.Inst.CurrentLevel.OnWinGame -= WinGame;
        }

        private void NextLevel()
        {
            OnNextLevel?.Invoke();
            inGameUI.WinGameGUI.SetActive(false);
        }
        private void PlayAgain()
        {
            OnPlayAgain?.Invoke();
            inGameUI.WinGameGUI.SetActive(false);
        }
        private void WinGame()
        {
            inGameUI.WinGameGUI.SetActive(true);
        }

        public void SetScore(int score)
        {
            inGameUI.Point.text = "SCORE: " + score;
        }
    }
}
