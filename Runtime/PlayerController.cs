using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Scripts.PlayerInput
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnInteractionTriggered;
        public event Action<bool> OnMovementStateChanged;
        
        [SerializeField] private float speed = 30f;
        [SerializeField] private Rigidbody rb;

        private bool isMoving;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void OnInteract()
        {
            // Debug.Log("Interact");
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
                    OnMovementStateChanged?.Invoke(isMoving);
                    // Debug.Log("Movement Started");
                }
                else
                {
                    OnMovementStateChanged?.Invoke(isMoving);
                    // Debug.Log("Movement Ended");
                }
            }
            
            rb.linearVelocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
        }
    }
}