using UnityEngine;

namespace StackMaker.Core
{
    using Management;
    using System;
    using Utilitys;
    using System.Collections;

    public class Player : MonoBehaviour
    {
        public event Action OnPlayerReachDes;
        // Start is called before the first frame update     
        [SerializeField]
        float speed = 5;
        [SerializeField]
        Transform benchmark;
        [SerializeField]
        Transform cameraLookAt;
        [SerializeField]
        AnimationModule Anim;
        [SerializeField]
        CheckCollide collide;
        public Transform Benchmark => benchmark;
        Vector2Int moveDirection = Vector2Int.zero;   
        Vector2Int destination;
        private Stack stacks = new Stack();
        
        Vector2Int SetMoveDirAndDestination
        {
            set
            {
                Vector2Int playerPos = Level.GetPosition(transform.localPosition);
                Vector2Int nextPos = playerPos + value;
                if (LevelManager.Inst.CurrentLevel.Data.ContainsKeyStackData(nextPos))
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
        public int NumOfStack
        {
            get => stacks.Count;
        }
        public Vector2Int MoveDirection
        {
            get => moveDirection;
            set
            {
                SetMoveDirAndDestination = value;
            }
        }

        //Temp Variable
        int animationState = 0;

        void Start()
        {
            destination = new Vector2Int((int)transform.localPosition.x,(int)transform.localPosition.z);
        }
        private void OnEnable()
        {
            Anim.UpdateEventAnimationState += EventUpdate;
        }

        // Update is called once per frame
        void Update()
        {
            GetPlayerInput();
        }

        private void FixedUpdate()
        {
            UpdateMoveDirection();
        }
        public void TakeStack(AbstractStack stack)
        {
            stacks.Push(stack);
            cameraLookAt.transform.localPosition -= Vector3.up * Level.TileHeight;
            if(animationState != 1)
            {
                animationState = 1;
                Anim.SetState(Anim.PLAYER_ANIM_STATE, animationState);                
            }
            
        }

        public AbstractStack ReturnStack()
        {
            cameraLookAt.transform.localPosition += Vector3.up * Level.TileHeight;
            return (AbstractStack)stacks.Pop();
        }
        public void RemoveAllStack()
        {
            while(stacks.Count > 0)
            {
                AbstractStack stack = (AbstractStack)stacks.Pop();
                PrefabManager.Inst.PushToPool(stack.gameObject, PrefabManager.Inst.ADDSTACK);
                gameObject.transform.localPosition -= new Vector3(0, Level.TileHeight, 0);
                cameraLookAt.transform.localPosition += Vector3.up * Level.TileHeight;
            }
        }
        public void WinLevel()
        {
            animationState = 2;
            Anim.SetState(Anim.PLAYER_ANIM_STATE, animationState);

            directionToWin = moveDirection;
            RemoveAllStack();
            gameObject.transform.localPosition += new Vector3(directionToWin.x, 0, directionToWin.y) * 4;
            moveDirection = Vector2Int.zero;
            //TEST: Debug.Log(directionToWin);
            OnPlayerReachDes?.Invoke();
        }

        public void ResetPlayer()
        {
            animationState = 0;
            Anim.SetState(Anim.PLAYER_ANIM_STATE, animationState);
            Vector2Int moveDirection = Vector2Int.zero;
            destination = Vector2Int.zero;
            directionToWin = Vector2Int.zero;
        }
        private void UpdateMoveDirection()
        {
            if (moveDirection != Vector2Int.zero)
            {
                Vector2Int playerPos = Level.GetPosition(transform.localPosition);
                Vector2Int nextPos = playerPos + moveDirection;
                Vector3 direction = new Vector3(nextPos.x - playerPos.x, 0, nextPos.y - playerPos.y);
                transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);
            }

            //NOTE: Check character reach destination or not
            if ((new Vector2(transform.localPosition.x, transform.localPosition.z) - destination).magnitude < Time.fixedDeltaTime * speed)
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
        private void GetPlayerInput()
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

        private void EventUpdate(string code)
        {
            if (code.IndexOf("EndAnimation") != -1)
            {
                if(code.IndexOf("TakeStack") != -1)
                {
                    animationState = 0;
                    Anim.SetState(Anim.PLAYER_ANIM_STATE, animationState);
                }
            }
        }

        Vector2Int directionToWin = Vector2Int.zero;       
        private void OnDisable()
        {
            Anim.UpdateEventAnimationState -= EventUpdate;
        }

        
    }
}