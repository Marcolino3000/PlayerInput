using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Runtime.Scripts.PlayerInput
{
    public class PlayerController : MonoBehaviour
    {
        public event Action OnInteractionTriggered;
    
        [SerializeField] private float speed = 30f;

        private Rigidbody rb;

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
            rb.linearVelocity = new Vector3(moveDirection.x * speed, 0, moveDirection.y * speed);
        }
    }
}
