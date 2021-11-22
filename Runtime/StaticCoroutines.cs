using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Phoder1.UnityCallbacks
{
    public static class StaticCoroutines
    {
        private static readonly List<(IEnumerator coroutine, Func<bool> keepAlive)> _staticCoroutines = new();

        static StaticCoroutines()
        {
            UnityCallbacks.Update += Update;
            Application.quitting += StopAllStaticCoroutines;
        }

        private static void Update()
        {
            if (!Application.isPlaying)
                return;

            for (int i = 0; i < _staticCoroutines.Count; i++)
            {
                try
                {
                    IEnumerator coroutine = _staticCoroutines[i].coroutine;

                    if (coroutine == null)
                        RemoveCoroutine();

                    else if (!_staticCoroutines[i].keepAlive?.Invoke() ?? false)
                        RemoveCoroutine();
                }
                catch (Exception ex)
                {
                    RemoveCoroutine();
                    Debug.LogException(ex);
                }

                void RemoveCoroutine()
                {
                    Debug.Log("Removed coroutine!");
                    (_staticCoroutines[i].coroutine as IDisposable)?.Dispose();

                    _staticCoroutines.RemoveAt(i);
                    i--;

                }
            }
        }

        public static void StartStaticCoroutine(IEnumerator coroutine)
        {
            if (coroutine == null)
                return;

            if (coroutine.Current == null)
            {
                if (coroutine.MoveNext())
                    _staticCoroutines.Add(GetElement());

                return;
            }

            _staticCoroutines.Add(GetElement());

            if (coroutine.Current is IEnumerator topCoroutine)
            {
                while (topCoroutine.Current is IEnumerator innerCoroutine)
                    topCoroutine = innerCoroutine;

                (topCoroutine as PausableYield)?.Resume();
            }

            (IEnumerator, Func<bool>) GetElement() => (coroutine, KeepAlive(coroutine));
        }
        public static void StopStaticCoroutine(IEnumerator coroutine)
        {
            if (coroutine == null)
                return;

            _staticCoroutines.RemoveAll((x) => x.coroutine == coroutine);

            if (coroutine.Current != null && coroutine.Current is IEnumerator topCoroutine)
            {
                while (topCoroutine.Current is IEnumerator innerCoroutine)
                    topCoroutine = innerCoroutine;

                (topCoroutine as PausableYield)?.Pause();
            }
        }
        public static void StopAllStaticCoroutines() => _staticCoroutines.Clear();

        //I generate a Func<bool> only once every MoveNext() to reduce Castings.
        private static Func<bool> KeepAlive(IEnumerator coroutine)
        {
            Func<bool> keepAlive = coroutine.Current switch
            {
                null => () => false,

                CustomYieldInstruction cyi => () => cyi.keepWaiting,

                AsyncOperation op => () => !op.isDone,

                IEnumerator innerCoroutin => KeepAlive(innerCoroutin),

                _ => () => false,
            };
            return () =>
            {
                if (keepAlive())
                    return true;

                if (coroutine.MoveNext())
                {
                    keepAlive = KeepAlive(coroutine);
                    return true;
                }

                return false;
            };
        }
    }
}
