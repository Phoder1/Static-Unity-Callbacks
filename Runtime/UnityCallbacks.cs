using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace Phoder1.UnityCallbacks
{
    public enum UpdateEvent
    {
        EarlyUpdate,
        NormalUpdate,
        LateUpdate,
        FixedUpdate,
        Every10Frames
    }

    /// <summary>
    /// <see cref="https://blog.beardphantom.com/post/190674647054/unity-2018-and-playerloop"/>
    /// </summary>
    public static class UnityCallbacks
    {
        #region Events
        public static event Action EarlyUpdate;
        public static event Action Update;
        public static event Action Every10Frames;
        public static event Action LateUpdate;
        public static event Action FixedUpdate;
        #endregion
        #region Class Data
        private static bool _initialized = false;
        #endregion
        #region Constructors
        static UnityCallbacks()
        {
            Enable();
        }


        #endregion

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Enable()
        {
            if (_initialized)
                return;

            _initialized = true;

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                if (playerLoop.subSystemList[i].type == typeof(PreUpdate))
                    playerLoop.subSystemList[i].updateDelegate += UnityCallbacks_PreUpdate;
                else if (playerLoop.subSystemList[i].type == typeof(Update))
                    playerLoop.subSystemList[i].updateDelegate += UnityCallbacks_Update;
                else if (playerLoop.subSystemList[i].type == typeof(PreLateUpdate))
                    playerLoop.subSystemList[i].updateDelegate += UnityCallbacks_LateUpdate;
                else if (playerLoop.subSystemList[i].type == typeof(FixedUpdate))
                    playerLoop.subSystemList[i].updateDelegate += UnityCallbacks_FixedUpdate;
            }
            PlayerLoop.SetPlayerLoop(playerLoop);

            Application.quitting += Disable;
        }
        private static void Disable()
        {
            if (!_initialized)
                return;

            _initialized = false;

            var playerLoop = PlayerLoop.GetCurrentPlayerLoop();
            for (int i = 0; i < playerLoop.subSystemList.Length; i++)
            {
                if (playerLoop.subSystemList[i].type == typeof(PreUpdate))
                    playerLoop.subSystemList[i].updateDelegate -= UnityCallbacks_PreUpdate;
                else if (playerLoop.subSystemList[i].type == typeof(Update))
                    playerLoop.subSystemList[i].updateDelegate -= UnityCallbacks_Update;
                else if (playerLoop.subSystemList[i].type == typeof(PreLateUpdate))
                    playerLoop.subSystemList[i].updateDelegate -= UnityCallbacks_LateUpdate;
                else if (playerLoop.subSystemList[i].type == typeof(FixedUpdate))
                    playerLoop.subSystemList[i].updateDelegate -= UnityCallbacks_FixedUpdate;
            }
            PlayerLoop.SetPlayerLoop(playerLoop);

            Application.quitting -= Disable;
        }
        /// <summary> 
        /// Add event (Action) to UpdateManager
        /// </summary>
        public static void AddUpdateEvent(UpdateEvent updateEvent, Action updateAction)
        {
            if (updateAction == null)
                return;

            switch (updateEvent)
            {
                case UpdateEvent.NormalUpdate:
                    Update += updateAction;
                    break;
                case UpdateEvent.EarlyUpdate:
                    EarlyUpdate += updateAction;
                    break;
                case UpdateEvent.LateUpdate:
                    LateUpdate += updateAction;
                    break;
                case UpdateEvent.FixedUpdate:
                    FixedUpdate += updateAction;
                    break;
                case UpdateEvent.Every10Frames:
                    Every10Frames += updateAction;
                    break;
            }
        }

        /// <summary> 
        /// Remove event (Action) from UpdateManager
        /// </summary>
        public static void RemoveUpdateEvent(UpdateEvent updateEvent, Action updateAction)
        {
            if (updateAction == null)
                return;

            switch (updateEvent)
            {
                case UpdateEvent.NormalUpdate:
                    Update -= updateAction;
                    break;
                case UpdateEvent.EarlyUpdate:
                    EarlyUpdate -= updateAction;
                    break;
                case UpdateEvent.LateUpdate:
                    LateUpdate -= updateAction;
                    break;
                case UpdateEvent.FixedUpdate:
                    FixedUpdate -= updateAction;
                    break;
                case UpdateEvent.Every10Frames:
                    Every10Frames -= updateAction;
                    break;
            }
        }
        private static void UnityCallbacks_PreUpdate() => EarlyUpdate?.Invoke();
        private static void UnityCallbacks_Update()
        {
            Update?.Invoke();

            if (Time.frameCount % 10 == 0)
                Every10Frames?.Invoke();
        }
        private static void UnityCallbacks_LateUpdate() => LateUpdate?.Invoke();
        private static void UnityCallbacks_FixedUpdate() => FixedUpdate?.Invoke();
    }
}
