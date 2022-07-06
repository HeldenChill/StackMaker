using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

namespace StackMaker.UI
{
    public class InGameUI : MonoBehaviour
    {
        public GameObject WinGameGUI;
        public GameObject PlayerProperty;
        public TMP_Text Point;
        public event Action OnNextLevel;
        public event Action OnPlayAgain;
        public void OnNextLevelButtonClick()
        {
            OnNextLevel?.Invoke();
        }

        public void OnPlayAgainButtonClick()
        {
            OnPlayAgain?.Invoke();
        }
    }
}
