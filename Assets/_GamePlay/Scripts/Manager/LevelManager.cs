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

        [SerializeField]
        private List<TextAsset> levelDatas;
        private int indexLevelData = 0;

        private void Awake()
        {
            if (inst == null)
            {
                inst = this;
                return;
            }
            Destroy(gameObject);
        }
        private void Start()
        {
            CurrentLevel.Initialize(levelDatas[indexLevelData]);
            UIManager.Inst.OnNextLevel += NextLevel;
        }

        private void OnDisable()
        {
            UIManager.Inst.OnNextLevel -= NextLevel;
        }

        private void NextLevel()
        {
            indexLevelData += 1;
            CurrentLevel.Data.Reset();
            CurrentLevel.Initialize(levelDatas[indexLevelData]);
        }
    }
}