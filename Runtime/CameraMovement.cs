using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

namespace Runtime.Scripts.Animation
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField] private float _duration;
        [SerializeField] private AnimationCurve _ease;
        [SerializeField] private Vector3 _dialogOffset;
        [SerializeField] private bool toggleOnDialog = true;
        
        private Vector3 _followOffset;
        private CinemachineFollow _follow;
        private bool _isInDialogMode;
        private float _progress;

        private void Awake()
        {
            _follow = GetComponent<CinemachineFollow>();
            _followOffset = _follow.FollowOffset; 
        }
    
        public void ToggleDialogMode(bool isDialogRunning)
        {
            if (!toggleOnDialog)
                return;

            _isInDialogMode = isDialogRunning;
            
            StartCoroutine(DoProgress());
        }
        
        public void ToggleDialogMode(bool isDialogRunning, Vector3 secondCharacterPosition)
        {
            if (!toggleOnDialog)
                return;

            _isInDialogMode = isDialogRunning;

            if (_follow != null && _follow.FollowTarget != null)
            {
                Vector3 mainTargetPosition = _follow.FollowTarget.position;
                float midpointX = (mainTargetPosition.x + secondCharacterPosition.x) * 0.5f;
                _dialogOffset = new Vector3(midpointX, _dialogOffset.y, _dialogOffset.z);
            }

            StartCoroutine(DoProgress());
        }

        private IEnumerator DoProgress()
        {
            while (_progress < 1.0f)
            {
                _progress += Time.deltaTime / _duration;
            
                // onValueChanged?.Invoke(_progress);
                TweenUpdate(_ease.Evaluate(_progress));
                yield return _progress;
            }
        
            _progress = 0.0f;
            _isInDialogMode = !_isInDialogMode;
        }

        private void TweenUpdate(float progress)
        {
            _follow.FollowOffset = _isInDialogMode ? 
                Vector3.Lerp(_dialogOffset, _followOffset, progress) : 
                Vector3.Lerp(_followOffset, _dialogOffset, progress);
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(500, 10, 140, 30), "Toggle Cam DialogMode"))
            {
                _isInDialogMode = !_isInDialogMode;
                ToggleDialogMode(_isInDialogMode);
            }
        }
    }
}
