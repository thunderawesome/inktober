using System.Collections;
using UnityEngine;

namespace Battlerock
{
    public static class AdjustTimeScale
    {
        #region Public Variables

        public const float INITIAL_VALUE = 1f;
        public const float MINIMUM_TIME = .001f;
        public const float TIME_MODIFIER = 10f;

        public static bool isGamePaused = false;

        #endregion

        #region Public Methods
        /// <summary>
        /// Sets the Time.timeScale to the time value provided.
        /// </summary>
        /// <param name="time">Value that Time.timeScale will be set to.</param>
        public static void SetTimeScale(float time)
        {
            Time.timeScale = time;
        }

        /// <summary>
        /// Reduces the Time.timeScale of the whole game.
        /// </summary>
        /// <param name="time">Time value used to affect Time.timeScale</param>
        /// <param name="minimumTime">Minimum time needed to break out of the coroutine loop.</param>
        /// <param name="reduceSpeedMod">How quickly/slowly time will be reduced.</param>
        /// <returns></returns>
        public static IEnumerator ReduceTimeScale(float time, float minimumTime, float reduceSpeedMod)
        {
            isGamePaused = true;

            while (time > minimumTime)
            {
                time -= reduceSpeedMod * Time.deltaTime;
                Time.timeScale = time;

#if UNITY_EDITOR

                Debug.LogFormat("<color=red>Reducing Time: {0} to minimum time: {1} with a modifier of {2}</color>", time, minimumTime, reduceSpeedMod);

#endif

                yield return null;
            }


            if (Time.timeScale <= minimumTime)
            {
                Time.timeScale = time = minimumTime;
            }
        }

        /// <summary>
        /// Restores Time.timeScale back to normal (value of 1) over time.
        /// </summary>
        /// <param name="reverseSpeedMod">How quickly/slowly time retruns back to normal (value of 1)</param>
        /// <returns></returns>
        public static IEnumerator RestoreTimeScale(float reverseSpeedMod)
        {
            isGamePaused = false;

            const float MAX_TIMESCALE_VALUE = 1;
            while (Time.timeScale < MAX_TIMESCALE_VALUE)
            {
                Time.timeScale += reverseSpeedMod * Time.deltaTime;

#if UNITY_EDITOR

                Debug.LogFormat("<color=green>Restoring Time: {0} to normal time: 1 with a modifier of {1}</color>", Time.timeScale, reverseSpeedMod);

#endif
                yield return null;
            }

            if (Time.timeScale >= MAX_TIMESCALE_VALUE)
            {
                Time.timeScale = MAX_TIMESCALE_VALUE;
            }
        }

        public static IEnumerator PingPongTimeScale(float time, float minimumTime, float reduceSpeedMod, float reverseSpeedMod, bool isWaitTime = false, float waitForSeconds = 1)
        {
            //slow time
            yield return ReduceTimeScale(time, minimumTime, reduceSpeedMod);
            //wait
            if (isWaitTime)
            {
                yield return new WaitForSeconds(waitForSeconds / 10);
            }
            //restore
            yield return RestoreTimeScale(reverseSpeedMod);
        }
        #endregion
    }
}