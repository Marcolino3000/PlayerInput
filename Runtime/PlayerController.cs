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
        [SerializeField] private float positionStuckTimeout;
        [SerializeField] private bool debugLogs;

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
            // if (!isMoving) return;
            
            isMoving = false;
            OnMovementEnded?.Invoke();
        }

        public void MoveInDirection(Vector2 direction)
        {
            OnMove(direction);
        }
        
        public IEnumerator MoveToTargetPositionCoroutine(Vector2 targetPosition)
        {
            if(moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(MoveByPosition(targetPosition));

            yield return moveCoroutine;
        }
        
        public void MoveToTargetPosition(Vector2 targetPosition)
        {
            if(moveCoroutine != null)
                StopCoroutine(moveCoroutine);

            moveCoroutine = StartCoroutine(MoveByPosition(targetPosition));
        }

        private IEnumerator MoveByPosition(Vector2 targetPosition)
        {
            Vector3 lastPosition = transform.position;
            float lastPositionChangeTime = Time.time;
            
            while (Vector2.Distance(targetPosition, Position) > distanceThreshold)
            {
                Vector2 direction = (targetPosition - Position).normalized;
                OnMove(direction);
                
                if (CheckIfStuck(ref lastPosition, ref lastPositionChangeTime)) 
                    yield break;
                
                yield return null;
            }
            StopMoving();
        }

        private bool CheckIfStuck(ref Vector3 lastPosition, ref float lastPositionChangeTime)
        {
            if ((transform.position - lastPosition).sqrMagnitude > 0.01f)
            {
                lastPosition = transform.position;
                lastPositionChangeTime = Time.time;
            }
            else if (Time.time - lastPositionChangeTime > positionStuckTimeout)
            {
                StopMoving();
                
                if(debugLogs)
                    Debug.LogWarning("Movement was stopped because Character was stuck in position");
                
                return true;
            }

            return false;
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