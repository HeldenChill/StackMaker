using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace StackMaker.UI
{
    public class InGameUI : MonoBehaviour
    {
        public event Action OnNextLevel;
        public void OnNextLevelButtonClick()
        {
            OnNextLevel?.Invoke();
        }
    }
}
