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
        private const float cameraParameter = 0.6f;
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
        PlayerData Data;
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
        private void Awake()
        {
            Data = ScriptableObject.CreateInstance("PlayerData") as PlayerData;
        }
        void Start()
        {
            destination = new Vector2Int((int)transform.localPosition.x,(int)transform.localPosition.z);
            InputManager.Inst.InputDirection += GetPlayerInput;
            SetScore(0);
        }
        private void OnEnable()
        {
            Anim.UpdateEventAnimationState += EventUpdate;
        }

        // Update is called once per frame


        private void FixedUpdate()
        {
            UpdateMoveDirection();
        }
        public void TakeStack(AbstractStack stack)
        {

            stacks.Push(stack);
            cameraLookAt.transform.localPosition -= Vector3.up * Level.TileHeight * cameraParameter;
            if(animationState != 1)
            {
                animationState = 1;
                Anim.SetState(Anim.PLAYER_ANIM_STATE, animationState);                
            }
            
        }

        public AbstractStack ReturnStack()
        {
            if (stacks.Count <= 0)
            {
                moveDirection = Vector2Int.zero;
                return null;
            }

            cameraLookAt.transform.localPosition += Vector3.up * Level.TileHeight * cameraParameter;
            return (AbstractStack)stacks.Pop();
        }
        public void RemoveAllStack()
        {
            while(stacks.Count > 0)
            {
                AbstractStack stack = (AbstractStack)stacks.Pop();
                PrefabManager.Inst.PushToPool(stack.gameObject, PrefabManager.Inst.ADDSTACK);
                gameObject.transform.localPosition -= new Vector3(0, Level.TileHeight, 0);
                cameraLookAt.transform.localPosition += Vector3.up * Level.TileHeight * cameraParameter;
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
            SetScore(0);
        }
        public void AddScore(int addScore)
        {
            Data.Score += addScore;
            SetScore(Data.Score);
        }
        private void SetScore(int score)
        {
            Data.Score = score;
            UIManager.Inst.SetScore(score);
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
        private void GetPlayerInput(Vector2Int moveDirection)
        {
            if (this.moveDirection != Vector2.zero)
                return;
            //Test Input
            SetMoveDirAndDestination = moveDirection;
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
            InputManager.Inst.InputDirection -= GetPlayerInput;
        }


    }
}