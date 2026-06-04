using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Runtime.Scripts.PlayerInput
{
    public class InputDispatcher : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private int secondsUntilGameReset;
        [SerializeField] private bool debugLogs;
        
        [SerializeField] private UnityEvent OnGameReset;
        [SerializeField] private UnityEvent<Vector2> OnMoveEvent;
        [SerializeField] private UnityEvent OnInteractEvent;
        [SerializeField] private UnityEvent OnMouseClickEvent;
        [SerializeField] private UnityEvent OnActivateRadarEvent;
        [SerializeField] private UnityEvent OnToggleMenuEvent;
        [SerializeField] private UnityEvent OnToggleTagebuchEvent;
        [SerializeField] private UnityEvent OnToggleMapEvent;
        [SerializeField] private UnityEvent OnToggleZweitesLog;

        private float _timeOfLastInput;

        private void Awake()
        {
            if(secondsUntilGameReset < 10)
                Debug.LogError("Time until game reset is very short.");
            
            _timeOfLastInput = Time.time;
        }

        private void Update()
        {
            if (Time.time - _timeOfLastInput > secondsUntilGameReset)
            {
                if(debugLogs) 
                    Debug.Log("Resetting Game due to inactivity");
                
                OnGameReset?.Invoke();
                _timeOfLastInput = Time.time;
            }
        }

        private void OnMove(InputValue value)
        {
            if(debugLogs) 
                Debug.Log("OnMove");
            
            _timeOfLastInput = Time.time;
            OnMoveEvent?.Invoke(value.Get<Vector2>());
        }

        private void OnInteract()
        {
            if(debugLogs) 
                Debug.Log("OnInteract");
            
            _timeOfLastInput = Time.time;
            OnInteractEvent?.Invoke();
        }
        
        private void OnActivateRadar()
        {
            if(debugLogs) 
                Debug.Log("OnActivateRadar");
            
            _timeOfLastInput = Time.time;
            OnActivateRadarEvent?.Invoke();
        }

        private void OnClickObject()
        {
            if(debugLogs) 
                Debug.Log("OnClickObject");
            
            _timeOfLastInput = Time.time;
            OnMouseClickEvent?.Invoke();
        }

        private void OnToggleMenu()
        {
            if(debugLogs) 
                Debug.Log("OnToggleMenu");
            
            _timeOfLastInput = Time.time;
            OnToggleMenuEvent?.Invoke();
        }

        private void OnToggleTagebuch()
        {
            if(debugLogs) 
                Debug.Log("OnToggleTagebuch");
            
            _timeOfLastInput = Time.time;
            OnToggleTagebuchEvent?.Invoke();
        }

        private void OnToggleMap()
        {
            if(debugLogs) 
                Debug.Log("OnToggleMap");
            
            _timeOfLastInput = Time.time;
            OnToggleMapEvent?.Invoke();
        }

        private void OnLog()
        {
            if(debugLogs) 
                Debug.Log("OnLog");
            
            _timeOfLastInput = Time.time;
            OnToggleZweitesLog?.Invoke();
        }
    }
    
}