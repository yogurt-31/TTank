using Unity.Netcode;
using UnityEngine;

public class NetMonoSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
{
    protected static T _instance = null;
    protected static bool IsDestroyed = false;

    public static T Instance
    {
        get
        {
            if (IsDestroyed)
            {
                _instance = null;
            }

            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogWarning($"{typeof(T).Name} singleton is not exist");
                }
                else
                {
                    IsDestroyed = false;
                }
            }

            return _instance;
        }
    }

    private void OnDisable()
    {
        IsDestroyed = true;
    }
}
