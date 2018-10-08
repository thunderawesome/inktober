using System.Collections;
using UnityEngine;

namespace Battlerock
{
    public class GameManager : Singleton<GameManager>
    {      
        #region Private Variables

        private IEnumerator m_coroutine = null;

        #endregion

        #region Unity Methods

        // Unity's built-in update loop method
        private void Update()
        {
            PauseOrResume();
        }

        /// <summary>
        /// Toggle between pausing and resuming the game.
        /// </summary>
        private void PauseOrResume()
        {
            if (Input.GetKeyDown(KeyCode.Escape) == true || Input.GetKeyDown(KeyCode.P) == true)
            {
                if (m_coroutine != null)
                {
                    StopCoroutine(m_coroutine);
                }

                if (AdjustTimeScale.isGamePaused == false)
                {
                    m_coroutine = AdjustTimeScale.ReduceTimeScale(AdjustTimeScale.INITIAL_VALUE, AdjustTimeScale.MINIMUM_TIME, AdjustTimeScale.TIME_MODIFIER);
                }
                else
                {
                    m_coroutine = AdjustTimeScale.RestoreTimeScale(AdjustTimeScale.TIME_MODIFIER);
                }

                StartCoroutine(m_coroutine);
            }
        }

        #endregion
    }
}