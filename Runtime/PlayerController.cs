using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Scripts.PlayerInput
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnInteractionTriggered;
        public event Action<MoveDirection> OnMovementStarted;
        public event Action OnMovementEnded; 
        
        [SerializeField] private float speed = 30f;
        [SerializeField] private float distanceThreshold;
        [SerializeField] private Rigidbody rb;

        private bool isMoving;
        private Coroutine moveCoroutine;
        private MoveDirection lastMoveDirection;
        private static bool movementEnabled = true;
        private Vector2 Position => new(transform.position.x, transform.position.z);

        public static void EnableMovement(bool value)
        {
            movementEnabled = value;
        }

        private void Update()
        {
            if(isMoving && !movementEnabled)
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
            if (!isMoving) return;
            
            isMoving = false;
            OnMovementEnded?.Invoke();
        }

        public void MoveInDirection(Vector2 direction)
        {
            OnMove(direction);
        }
        
        public IEnumerator MoveToTargetPosition(Vector2 targetPosition)
        {
            yield return MoveByPosition(targetPosition);
        }

        private IEnumerator MoveByPosition(Vector2 targetPosition)
        {
            while (Vector2.Distance(targetPosition, Position) > distanceThreshold)
            {
                Vector3 direction = (targetPosition - Position);
                Vector2 moveInput = new Vector2(direction.x, direction.z).normalized;
                OnMove(moveInput);
                yield return null;
            }
            
            StopMoving();
        }
        

        private void OnMove(Vector2 moveDirection)
        {
            if(!movementEnabled)
            {
                StopMoving();
                return;
            }
            
            var direction = moveDirection.x < 0 ? MoveDirection.Left : MoveDirection.Right;

            bool wasMoving = isMoving;
            isMoving = moveDirection.sqrMagnitude > 0.01f;

            if (!isMoving)
            {
                StopMoving();
                return;
            }
            
            if (!wasMoving || direction != lastMoveDirection)
            {
                OnMovementStarted?.Invoke(direction);
            }
            
            rb.linearVelocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
            lastMoveDirection = direction;
        }
    }

    public struct AnimationState
    {
        public bool IsWalking;
        public MoveDirection MoveDirection;
    }

    public enum MoveInputType
    {
        Direction,
        Position
    }

    public enum MoveDirection
    {
        Left,
        Right
    }
}