using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    using System;
    public class WinPosition : MonoBehaviour
    {
        // Start is called before the first frame update       
        GameObject chestClose;
        GameObject chestOpen;       
        public void OnGameWin()
        {
            Debug.Log("GAME WIN");
        }
    }
}
