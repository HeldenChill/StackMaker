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
        Vector2Int destination;
        public Stack Stacks = new Stack();
        Vector2Int SetMoveDirAndDestination
        {
            set
            {
                Vector2Int playerPos = Level.GetPosition(transform.localPosition);
                Vector2Int nextPos = playerPos + value;
                if (LevelManager.Inst.CurrentLevel.Data.CheckPosStackData(nextPos))
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
            destination = new Vector2Int((int)transform.localPosition.x,(int)transform.localPosition.z);
        }

        // Update is called once per frame
        void Update()
        {
            if (moveDirection != Vector2.zero)
                return;
            //Test Input
            if (Input.GetKeyDown(KeyCode.A))
            {
                SetMoveDirAndDestination = Vector2Int.left;
            }
            else if (Input.GetKeyDown(KeyCode.W))
            {
                SetMoveDirAndDestination = Vector2Int.up;

            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                SetMoveDirAndDestination = Vector2Int.right;

            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                SetMoveDirAndDestination = Vector2Int.down;

            }
            //--


        }

        private void FixedUpdate()
        {
            if (moveDirection != Vector2Int.zero)
            {
                Vector2Int playerPos = Level.GetPosition(transform.localPosition);
                Vector2Int nextPos = playerPos + moveDirection;
                Vector3 direction = new Vector3(nextPos.x - playerPos.x, 0, nextPos.y - playerPos.y);
                transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);
            }

            //NOTE: Check character reach destination or not
            if((new Vector2(transform.localPosition.x,transform.localPosition.z) - destination).magnitude < Time.fixedDeltaTime * speed)
            {
                //NOTE: Add Interact with Stack here
                transform.localPosition = new Vector3(destination.x, transform.localPosition.y, destination.y);
                if (moveDirection != Vector2Int.zero)
                {
                    LevelManager.Inst.CurrentLevel.Data.GetPosStackData(destination).Value.Interact(this);
                }
                //NOTE: Proverty check move direction and setup destination
                SetMoveDirAndDestination = moveDirection; 
            }
        }
    }
}