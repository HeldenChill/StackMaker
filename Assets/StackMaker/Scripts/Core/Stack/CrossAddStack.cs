using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Core
{
    public class CrossAddStack : AddStack
    {
        private readonly Quaternion[] ROTATION = new Quaternion[]
        {
            Quaternion.Euler(90,90,90),
            Quaternion.Euler(180,90,90),
            Quaternion.Euler(270,90,90),
            Quaternion.Euler(0,90,90),
        };
        private readonly Vector2Int[] DIRECTION = new Vector2Int[] { Vector2Int.right, Vector2Int.up, Vector2Int.left, Vector2Int.down };
        public enum StackDirection
        {
            Right = 0,
            Up = 1,
            Left = 2,
            Down = 3

        }

        public StackDirection Direction1;
        public StackDirection Direction2;
        public Transform Indicator;
        private void Start()
        {
            //TODO: Rotate the cross stack depend on its direction
            SetRotationIndicator();
        }
        public override bool Interact(Player player)
        {
            if (!base.Interact(player))
                return false;
            SetPlayerDirection(player);         
            return true;
        }

        private void SetPlayerDirection(Player player)
        {
            if (player.MoveDirection + DIRECTION[(int)Direction1] == Vector2Int.zero)
            {
                player.MoveDirection = DIRECTION[(int)Direction2];
            }
            else if (player.MoveDirection + DIRECTION[(int)Direction2] == Vector2Int.zero)
            {
                player.MoveDirection = DIRECTION[(int)Direction1];
            }
        }

        private void SetRotationIndicator()
        {
            Vector2Int dirRotation = DIRECTION[(int)Direction1] + DIRECTION[(int)Direction2];
            if (dirRotation == new Vector2Int(1, -1))
            {
                Indicator.localRotation = ROTATION[1];
            }
            else if (dirRotation == new Vector2Int(1, 1))
            {
                Indicator.localRotation = ROTATION[2];

            }
            else if (dirRotation == new Vector2Int(-1, 1))
            {
                Indicator.localRotation = ROTATION[3];
            }
            else if (dirRotation == new Vector2Int(-1, -1))
            {
                Indicator.localRotation = ROTATION[0];
            }
        }
    }
}