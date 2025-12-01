using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Runtime.Scripts.PlayerInput
{
    public class InputDispatcher : MonoBehaviour
    {
        [SerializeField] private UnityEvent<InputValue> OnMoveEvent;
        [SerializeField] private UnityEvent OnInteractEvent;
        [SerializeField] private UnityEvent OnMouseClickEvent;
        [SerializeField] private UnityEvent OnActivateRadarEvent;
        
        private void OnMove(InputValue value)
        {
            OnMoveEvent?.Invoke(value);
        }

        private void OnInteract()
        {
            OnInteractEvent?.Invoke();
        }
        
        private void OnActivateRadar()
        {
            OnActivateRadarEvent?.Invoke();
        }

        private void OnClickObject()
        {
            OnMouseClickEvent?.Invoke();
        }
        
    }
}