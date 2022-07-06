using StackMaker.Management;
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
            Down = 3,
            None = 4
        }
        public Transform Indicator;
        public GameObject AddStackModel;

        StackDirection Direction1;
        StackDirection Direction2;
        private void Start()
        {
            //TODO: Rotate the cross stack depend on its direction
            SetRotationIndicator();
        }
        public override bool Interact(Player player)
        {
            SetPlayerDirection(player);
            AddStackModel.SetActive(false);
            if (!base.Interact(player))
                return false;

            GameObject addStack = PrefabManager.Inst.PopFromPool(PrefabManager.Inst.ADDSTACK);
            player.TakeStack(addStack.gameObject.GetComponent<AbstractStack>());

            addStack.transform.localScale = Vector3.one;
            addStack.transform.parent = player.transform;
            player.transform.localPosition += new Vector3(0, Level.TileHeight, 0);
            addStack.transform.localPosition = player.Benchmark.localPosition - new Vector3(0, Level.TileHeight * (player.NumOfStack), 0);
            return true;
        }
        public void SetStackDirection(Vector2Int dir1,Vector2Int dir2)
        {
            Direction1 = Vector2Direction(dir1);
            Direction2 = Vector2Direction(dir2);
            SetRotationIndicator();
        }

        public override void StackReset()
        {
            AddStackModel.SetActive(true);
        }

        private StackDirection Vector2Direction(Vector2Int dir)
        {
            if(dir == Vector2Int.right)
            {
                return StackDirection.Right;
            }
            else if(dir == Vector2Int.up)
            {
                return StackDirection.Up;
            }
            else if (dir == Vector2Int.left)
            {
                return StackDirection.Left;

            }
            else if (dir == Vector2Int.down)
            {
                return StackDirection.Down;
            }
            return StackDirection.None;
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