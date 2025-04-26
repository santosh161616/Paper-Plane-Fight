using UnityEngine;

namespace Plane.Utils
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance;
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = (T)this;
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // public abstract class PersistantSingletonMonoBehaviour<T> : MonoBehaviour where T : PersistantSingletonMonoBehaviour<T>
    // {
    //     public static T Instance;
    //     public void Awake()
    //     {
    //         if (Instance == null)
    //         {
    //             Instance = (T)this;
    //             DontDestroyOnLoad(this.gameObject);
    //         }
    //         else if (Instance != null)
    //         {
    //             Destroy(this.gameObject);
    //         }
    //     }
    // }

}