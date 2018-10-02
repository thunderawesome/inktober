using UnityEngine;

namespace Battlerock
{
    public class AnimateExample : MonoBehaviour
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

        private float currentFrameTime = 0f;
        private int dir = 1;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            if (playOnAwake == true)
            {
                Play();
            }
        }

        private void Update()
        {
            if (frames.Length <= 0 || isPlaying == false) return;

            currentFrameTime += Time.deltaTime;

            if (currentFrameTime >= interval)
            {
                currentFrameTime = 0;
                currentFrame += dir;

                if (dir == 1 && currentFrame == frames.Length)
                {
                    if (pingPong)
                    {
                        currentFrame--;
                        dir = -1;
                    }
                    else currentFrame = 0;

                    if (loop == false)
                    {
                        Reset();
                    }
                }
                else if (dir == -1 && currentFrame == 0)
                {
                    dir = 1;
                    if (loop == false)
                    {
                        Reset();
                    }
                }

                int thisFrame = 0;

                // Loops through array of GameObjects
                // that are specified as "frames"
                for (int i = 0; i < frames.Length; i++)
                {
                    frames[i].SetActive(false);
                    if (currentFrame == thisFrame)
                    {
                        frames[i].SetActive(true);
                    }

                    thisFrame++;
                }
            }
        }

        #endregion

        #region Private Methods

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
            currentFrameTime = 0;
        }

        #endregion

    }
}