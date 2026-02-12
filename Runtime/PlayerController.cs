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

        private bool isMoving;
        private static bool movementEnabled = true;

        public static void EnableMovement(bool value)
        {
            movementEnabled = value;
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
            if (!isMoving) return;
            
            isMoving = false;
            OnMovementEnded?.Invoke();
        }

        public void OnMove(Vector2 moveDirection)
        {
            if(!movementEnabled)
            {
                StopMoving();
                return;
            }
            
            bool wasMoving = isMoving;
            isMoving = moveDirection.sqrMagnitude > 0.01f;

            if (isMoving != wasMoving)
            {
                if (isMoving)
                {
                    OnMovementStarted?.Invoke(isMoving, moveDirection.x < 0 ? MoveDirection.Left : MoveDirection.Right);
                }
                else
                {
                    OnMovementEnded?.Invoke();
                }
            }
            
            rb.linearVelocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
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