using System.Collections;
using UnityEngine;

namespace Battlerock
{
    public class AnimateCoroutineExample : MonoBehaviour
    {
        #region Public Variables

        /// <summary>
        /// Array of gameobjects that will be
        /// used as "frames" in the animation.
        /// </summary>
        public GameObject[] frames;

        /// <summary>
        /// Current frame (index) that the
        /// animation is on.
        /// </summary>
        public int currentFrame = 0;

        public bool isPlaying = false;
        public bool playOnAwake = true;
        public bool loop = true;
        public bool pingPong = false;

        /// <summary>
        /// How quickly to move through the animation.
        /// </summary>
        public float interval = 0.1f;

        #endregion

        #region Private Variables

        private int dir = 1;

        private IEnumerator m_coroutine;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (playOnAwake == true)
            {
                Play();
                m_coroutine = Animate();
                StartCoroutine(m_coroutine);
            }
        }

        private void OnEnable()
        {
            // Only want to assign it here if playOnAwake is not true and no coroutine has been assigned yet.
            if (m_coroutine == null && playOnAwake == false)
            {
                m_coroutine = Animate();
            }
        }

        private void OnDestroy()
        {
            StopCoroutine(Animate());
        }

        private void OnDisable()
        {
            StopCoroutine(Animate());
        }

        #endregion

        #region Private Methods

        private IEnumerator Animate()
        {
            if (frames.Length <= 0 || isPlaying == false)
            {
                Debug.LogWarningFormat("Either there are no frames to animate (Frames: {0}) OR the isPlaying is false (isPlaying: {1}). Exiting Coroutine early.", frames.Length, isPlaying);
                yield break;
            }

            while (isPlaying == true)
            {
                yield return new WaitForSeconds(interval);

                currentFrame += dir;

                if (dir == 1 && currentFrame == frames.Length)
                {
                    if (pingPong == true && loop == false)
                    {
                        currentFrame--;
                        dir = -1;

                        if (currentFrame == 0)
                        {
                            pingPong = false;
                            Reset();
                            yield break;
                        }
                    }
                    else if (pingPong == true && loop == true)
                    {
                        currentFrame--;
                        dir = -1;
                    }
                    else if (pingPong == false && loop == true)
                    {
                        currentFrame = 0;
                        dir = 1;
                        //yield break;
                    }
                    else if(pingPong == false && loop == false)
                    {
                        ClampToLastFrame();
                        yield break;
                    }
                }
                else if (dir == -1 && currentFrame == 0)
                {
                    dir = 1;
                    if (loop == false)
                    {
                        Reset();
                        yield break;
                    }
                }               

                int thisFrame = 0;
                for (int i = 0; i < frames.Length; i++)
                {
                    frames[i].SetActive(false);
                    if (currentFrame == thisFrame)
                    {
                        frames[i].SetActive(true);
                    }

                    thisFrame++;
                }

                // Without this, the while loop would go on indefinitely and crash unity.
                yield return null;
            }
        }

        private void Play()
        {
            isPlaying = true;
        }

        private void Pause()
        {
            isPlaying = false;
        }

        private void Reset()
        {
            isPlaying = false;
            currentFrame = 0;

            // Loops through and disables all Frames and enables the first frame.
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i].SetActive(false);
            }

            frames[currentFrame].SetActive(true);
        }

        private void ClampToLastFrame()
        {
            isPlaying = false;
            currentFrame = frames.Length - 1;
            // Loops through and disables all Frames and enables the first frame.
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i].SetActive(false);
            }

            frames[currentFrame].SetActive(true);
        }

        #endregion


    }
}