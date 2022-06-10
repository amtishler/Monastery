using UnityEngine;
using Cinemachine;

public class CameraRegister : MonoBehaviour
{
    public void OnEnable()
    {
        CameraSwitcher.Register(GetComponent<CinemachineVirtualCamera>());
    }

    public void OnDisnable()
    {
        CameraSwitcher.Unregister(GetComponent<CinemachineVirtualCamera>());
    }
}
