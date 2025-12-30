using UnityEngine;

public class MonoSingle<T> : MonoBehaviour where T:MonoBehaviour 
{
    public static T instance;

    protected virtual void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this as T;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
