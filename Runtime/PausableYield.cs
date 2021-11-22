using UnityEngine;

namespace Phoder1.UnityCallbacks
{
    public abstract class PausableYield : CustomYieldInstruction
    {
        public bool IsPaused { get; protected set; }
        public void Pause()
        {
            if (IsPaused) return;

            OnPause();
            IsPaused = true;
        }
        public abstract void OnPause();
        public void Resume()
        {
            if(!IsPaused) return;

            OnResume();
            IsPaused = false;
        }
        public abstract void OnResume();
    }
    public class WaitForSecondsPausable : PausableYield
    {
        private double _timeToWait;
        private double _totalCountedTime = 0;
        private double _lastUpdateTime;

        public WaitForSecondsPausable(float timeToWait)
        {
            _timeToWait = timeToWait;
            _lastUpdateTime = Time.time;
        }

        public override void OnPause() => UpdateTotalTime();

        public override void OnResume() => _lastUpdateTime = Time.time;
        private void UpdateTotalTime()
        {
            _totalCountedTime += Time.time - _lastUpdateTime;
            _lastUpdateTime = Time.time;
        }
        public override bool keepWaiting
        {
            get
            {
                if (_totalCountedTime > _timeToWait)
                    return false;

                if(IsPaused)
                    return true;

                return _totalCountedTime + (Time.time - _lastUpdateTime) < _timeToWait;
            }
        }
    }
}
