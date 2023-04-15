using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace MyFps.Gameplay.FirstPerson
{
    public class CameraBehavior : MonoBehaviour
    {
        [SerializeField]
        Camera _firstPersonCamera;

        UniversalAdditionalCameraData _cameraData;
        Camera _mainCamera;

        void Awake()
        {
            _mainCamera = Camera.main;
            _cameraData = _mainCamera.GetUniversalAdditionalCameraData();
        }

        void OnEnable()
        {
            _firstPersonCamera.enabled = true;
            _cameraData.cameraStack.Add(_firstPersonCamera);
        }

        void OnDisable()
        {
            _firstPersonCamera.enabled = false;
            _cameraData.cameraStack.Remove(_firstPersonCamera);
        }
    }
}