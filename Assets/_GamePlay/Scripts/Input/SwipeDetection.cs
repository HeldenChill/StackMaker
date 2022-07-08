using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utilitys
{
    using StackMaker.Management;
    using System;

    public class SwipeDetection
    {
        private InputManager inputManager;
        [SerializeField]
        private const float minimumDistance = 0.005f;
        private const float maximumTime = 1f;

        private Vector2 startPosition;
        private float startTime;
        private Vector2 endPosition;
        private float endTime;

        public SwipeDetection(InputManager inputManager)
        {
            this.inputManager = inputManager;
        }

        public void SwipeStart(Vector2 position, float time)
        {
            startPosition = position;
            startTime = time;
        }

        public Vector2Int SwipeEnd(Vector2 position, float time)
        {
            endPosition = position;
            endTime = time;
            return DetectSwipe();
        }

        private Vector2Int DetectSwipe()
        {
            if(Vector3.Distance(startPosition,endPosition) >= minimumDistance &&
                (endTime - startTime) < maximumTime)
            {
                Vector2 swipeDir = endPosition - startPosition;
                
                if (Mathf.Abs(swipeDir.x) > Mathf.Abs(swipeDir.y))
                {
                    if(swipeDir.x > 0)
                    {
                        return Vector2Int.right;
                    }
                    else
                    {
                        return Vector2Int.left;
                    }
                }
                else
                {
                    if (swipeDir.y > 0)
                    {
                        return Vector2Int.up;
                    }
                    else
                    {
                        return Vector2Int.down;
                    }
                }
            }
            return Vector2Int.zero;
        }

    }
}