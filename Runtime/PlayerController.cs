using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Scripts.PlayerInput
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnInteractionTriggered;
        public event Action<bool, MoveDirection> OnMovementStarted;
        public event Action OnMovementEnded; 
        
        [SerializeField] private float speed = 30f;
        [SerializeField] private Rigidbody rb;

        private bool currentMovingState;
        private Coroutine moveByClickCoroutine;
        private MoveDirection lastMoveDirection;
        private static bool movementEnabled = true;

        public static void EnableMovement(bool value)
        {
            movementEnabled = value;
        }

        private void Update()
        {
            if(currentMovingState && !movementEnabled)
                StopMoving();
        }

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void OnInteract()
        {
            OnInteractionTriggered?.Invoke();
        }
        
        private void StopMoving()
        {
            rb.linearVelocity = Vector3.zero;
            if (!currentMovingState) return;
            
            currentMovingState = false;
            OnMovementEnded?.Invoke();
        }

        public void OnMove(Vector2 target, Coroutine moveCoroutine = null)
        {
            StopCoroutine(moveByClickCoroutine);
            moveByClickCoroutine = moveCoroutine;
            
            if(!movementEnabled)
            {
                StopMoving();
                return;
            }

            var previousMovingState = currentMovingState;
            currentMovingState = target.sqrMagnitude > 0.01f;
            var moveDirection = target.x < 0 ? MoveDirection.Left : MoveDirection.Right;

            
            
            if (lastMoveDirection != moveDirection && currentMovingState) 
            {
                OnMovementStarted?.Invoke(previousMovingState, moveDirection);
            }

            else
            {
                if (currentMovingState != previousMovingState)
                {
                    if (currentMovingState)
                    {
                        OnMovementStarted?.Invoke(currentMovingState, moveDirection);
                        lastMoveDirection = moveDirection;
                    }
                    else
                    {
                        OnMovementEnded?.Invoke();
                    }
                }    
            }
            
            
            rb.linearVelocity = new Vector3(target.x * speed, 0, target.y * speed);
        }
    }

    public struct AnimationState
    {
        public bool IsWalking;
        public MoveDirection MoveDirection;
    }

    public enum MoveDirection
    {
        Left,
        Right
    }
}