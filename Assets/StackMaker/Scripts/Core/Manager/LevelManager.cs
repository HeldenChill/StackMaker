using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Management
{
    using Core;
    public class LevelManager : MonoBehaviour
    {
        private static LevelManager inst;
        public static LevelManager Inst
        {
            get => inst;
        }
        [SerializeField]
        private Level currentLevel;
        public Level CurrentLevel { 
            get => currentLevel; 
        }

        void Awake()
        {
            if (inst == null)
            {
                inst = this;
                return;
            }
            Destroy(gameObject);
        }
    }
}