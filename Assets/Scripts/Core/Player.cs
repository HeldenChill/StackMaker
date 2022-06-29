using UnityEngine;

namespace StackMaker.Core
{
    using Management;
    using System.Collections;

    public class Player : MonoBehaviour
    {
        // Start is called before the first frame update     
        [SerializeField]
        float speed = 5;
        [SerializeField]
        Transform benchmark;
        public Transform Benchmark => benchmark;
        Vector2Int moveDirection = Vector2Int.zero;   
        Vector2 destination;

        public Stack Stacks = new Stack();
        Vector2Int MoveDirection
        {
            get => moveDirection;
            set
            {
                Vector2Int playerPos = Level.GetPosition(transform.localPosition);
                Vector2Int nextPos = playerPos + value;
                if (LevelManager.Inst.CurrentLevel.Data.PosToStack.ContainsKey(nextPos))
                {
                    moveDirection = value;
                    destination = nextPos;
                }
                else
                {
                    moveDirection = Vector2Int.zero;
                    return;
                }
            }
        }
        void Start()
        {
            destination = transform.localPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if (moveDirection != Vector2.zero)
                return;
            //Test Input
            if (Input.GetKeyDown(KeyCode.A))
            {
                MoveDirection = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                MoveDirection = Vector2Int.up;

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                MoveDirection = Vector2Int.right;

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveDirection = Vector2Int.down;

            }
            //--


        }

        private void FixedUpdate()
        {
            if (MoveDirection != Vector2Int.zero)
            {
                Vector2Int playerPos = Level.GetPosition(transform.localPosition);
                Vector2Int nextPos = playerPos + MoveDirection;
                Vector3 direction = new Vector3(nextPos.x - playerPos.x, 0, nextPos.y - playerPos.y);
                transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);
            }

            //NOTE: Check character reach destination or not
            if((new Vector2(transform.localPosition.x,transform.localPosition.z) - destination).magnitude < 0.001f)
            {
                MoveDirection = moveDirection; //NOTE: Proverty check move direction and destination
            }
        }
    }
}