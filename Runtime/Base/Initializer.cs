using System;
using System.Linq;
using UnityEngine;

namespace StateMachineSystem.Base
{
    public class Initializer : MonoBehaviour
    {
        public static event Action OnCreate
        {
            add
            {
                //Debug.LogFormat("GenericInitializer OnCreate {1} {0}", value.Target, onCreate == null || !onCreate.GetInvocationList().Contains(value) ? "Accepted" : "Rejected");

                if (onCreate == null || !onCreate.GetInvocationList().Contains(value))
                    onCreate += value;
            }
            remove => onCreate -= value;
        }
        public static event Action OnInit
        {
            add
            {
                //Debug.LogFormat("GenericInitializer OnInit {1} {0}", value.Target, onInit == null || !onInit.GetInvocationList().Contains(value) ? "Accepted" : "Rejected");

                if (onInit == null || !onInit.GetInvocationList().Contains(value))
                    onInit += value;
            }
            remove => onInit -= value;
        }
        public static event Action OnStart
        {
            add
            {
                //Debug.LogFormat("GenericInitializer OnStart {1} {0}", value.Target, onStart == null || !onStart.GetInvocationList().Contains(value) ? "Accepted" : "Rejected");

                if (onStart == null || !onStart.GetInvocationList().Contains(value))
                    onStart += value;
            }
            remove => onStart -= value;
        }
        public static event Action OnApplicationClose
        {
            add
            {
                //Debug.LogFormat("GenericInitializer OnStart {1} {0}", value.Target, onStart == null || !onStart.GetInvocationList().Contains(value) ? "Accepted" : "Rejected");

                if (onApplicationClose == null || !onApplicationClose.GetInvocationList().Contains(value))
                    onApplicationClose += value;
            }
            remove => onApplicationClose -= value;
        }

        private static event Action onCreate;
        private static event Action onInit;
        private static event Action onStart;
        private static event Action onApplicationClose;

        private static Initializer Instance;

        void Awake()
        {
            if (Instance && Instance != this)
                enabled = false;
            else
                Instance = this;
        }

        private void Start()
        {
            Debug.Log("Initializer - Start");
            try
            {
                onCreate?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError("Initializer onCreate Error");
                Debug.LogException(e);
                throw;
            }
            try
            {
                onInit?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError("Initializer onInit Error");
                Debug.LogException(e);
                throw;
            }
            try
            {
                onStart?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError("Initializer onStart Error");
                Debug.LogException(e);
                throw;
            }
        }

        void OnDisable()
        {
            onApplicationClose?.Invoke();
        }

        private static void CheckInstance()
        {
            if (!Instance) CreateInstance();
        }

        private static void CreateInstance()
        {
            Debug.Log("Initializer - Create Instance");
            Instance = FindObjectOfType<Initializer>() ?? new GameObject("GenericInitializer").AddComponent<Initializer>();
            Application.onBeforeRender -= CheckInstance;
        }

        static Initializer()
        {
            Debug.Log("Initializer - Check Instance Scheduled");
            Application.onBeforeRender += CheckInstance;
        }
    }
}