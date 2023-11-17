using UnityEngine;

namespace GameCore
{
    public class DeviceAdapter : MonoBehaviour
    {
        [SerializeField] private DeviceType _adaptedTo;
        [SerializeField] private GameObject[] _controlObjects;
        [SerializeField] private MonoBehaviour[] _controlScripts;
    
        private void Awake()
        {
            if (SystemInfo.deviceType == _adaptedTo)
                return;
            
            foreach (GameObject controlObject in _controlObjects)
                if (controlObject != null)
                    controlObject.SetActive(false);

            foreach (MonoBehaviour controlScript in _controlScripts)
                if (controlScript != null)
                    controlScript.enabled = false;
        }
    }
}
