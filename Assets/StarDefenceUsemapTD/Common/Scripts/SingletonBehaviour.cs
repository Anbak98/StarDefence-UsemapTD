using UnityEngine;

namespace STARTD.Common
{
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        //==================================================
        // Fields
        //==================================================
        private static T _instance = null;
        protected bool dontDestroyOnLoad = false;

        //==================================================
        // Properties
        //==================================================
        public static T Singleton
        {
            get
            {
                if (_instance == null)
                {
                    Create();
                }

                return _instance;
            }
        }

        //==================================================
        // Unity Lifecycle
        //==================================================
        protected virtual void Awake()
        {
            Create();

            if (_instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(this);
                }
            }
        }

        protected virtual void OnDestroy()
        {
            _instance = null;
        }

        //==================================================
        // Methods
        //==================================================
        protected static void Create()
        {
            if (_instance == null)
            {
                T[] objects = FindObjectsByType<T>(FindObjectsSortMode.InstanceID);

                if (objects.Length > 0)
                {
                    _instance = objects[0];

                    for (int i = 1; i < objects.Length; ++i)
                    {
                        if (Application.isPlaying)
                        {
                            Destroy(objects[i].gameObject);
                        }
                        else
                        {
                            DestroyImmediate(objects[i].gameObject);
                        }
                    }
                }
                else
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
            }
        }
    }
}
