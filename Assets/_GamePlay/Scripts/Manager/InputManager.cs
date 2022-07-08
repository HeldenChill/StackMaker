using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackMaker.Management
{
    using System;
    using Utilitys;
    [DefaultExecutionOrder(-1)]
    public class InputManager : Singleton<InputManager>
    {
        public event Action<Vector2Int> InputDirection;
        public event Action<Vector2, float> OnStartTouch;
        public event Action<Vector2, float> OnEndTouch;
        [HideInInspector]
        public SwipeDetection swipeDetection;
        private TouchControls touchControls;
        [SerializeField]
        private Camera mainCamera;
        protected override void Awake()
        {
            base.Awake();
            touchControls = new TouchControls();
        }

        private void OnEnable()
        {
            touchControls.Enable();
            swipeDetection = new SwipeDetection(this);
        }

        private void OnDisable()
        {
            touchControls.Disable();
        }

        private void Start()
        {
            touchControls.Touch.PrimaryContact.started += ctx => StartTouchPrimary(ctx); 
            touchControls.Touch.PrimaryContact.canceled += ctx => EndTouchPrimary(ctx);
            touchControls.Key.Up.started += ctx => MoveUp(ctx);
            touchControls.Key.Down.started += ctx => MoveDown(ctx);
            touchControls.Key.Left.started += ctx => MoveLeft(ctx);
            touchControls.Key.Right.started += ctx => MoveRight(ctx);
        }
        private void StartTouchPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            Vector3 pos = Utils.ScreenToWorld(mainCamera, touchControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            float time = (float)ctx.startTime;         
            
            OnStartTouch?.Invoke(pos, time);
            swipeDetection.SwipeStart(pos, time);
        }
        private void EndTouchPrimary(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            Vector3 pos = Utils.ScreenToWorld(mainCamera, touchControls.Touch.PrimaryPosition.ReadValue<Vector2>());
            float time = (float)ctx.time;    
            
            OnEndTouch?.Invoke(pos, time);
            Vector2Int swipeDir = swipeDetection.SwipeEnd(pos, time);
            if(swipeDir != Vector2Int.zero)
            {
                InputDirection?.Invoke(swipeDir);
            }
        }

        private void MoveUp(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            InputDirection?.Invoke(Vector2Int.up);

        }
        private void MoveDown(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            InputDirection?.Invoke(Vector2Int.down);

        }
        private void MoveLeft(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            InputDirection?.Invoke(Vector2Int.left);
        }
        private void MoveRight(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            InputDirection?.Invoke(Vector2Int.right);
        }

    }
}