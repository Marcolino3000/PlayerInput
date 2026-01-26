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

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void OnInteract()
        {
            OnInteractionTriggered?.Invoke();
        }

        public void OnMove(Vector2 moveDirection)
        {
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