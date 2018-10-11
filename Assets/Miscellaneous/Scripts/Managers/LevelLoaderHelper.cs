using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Battlerock
{
    public enum LEVEL
    {
        MAINMENU,
        LEVELONE
    }

    public static class LevelLoaderHelper
    {
        #region Public Static Variables

        /// <summary>
        /// This is used to detect which level was selected (slashed) in the menu screen.
        /// </summary>
        public static int selectedLevel = 0;

        #endregion

        #region Public Static Methods

        public static void LoadScene(int id, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(id, mode);
            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadScene(LEVEL id, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene((int)id, mode);
            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadScene(Scene scene, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene.name, mode);
            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
        }

        public static void LoadScene(string sceneName, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(sceneName, mode);
            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
        }

        public static AsyncOperation LoadSceneAsync(int id, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.LoadSceneAsync(id, mode);
        }

        public static AsyncOperation LoadSceneAsync(LEVEL id, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.LoadSceneAsync((int)id, mode);
        }

        public static AsyncOperation LoadSceneAsync(Scene scene, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.LoadSceneAsync(scene.name, mode);
        }

        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode mode)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.LoadSceneAsync(sceneName, mode);
        }

        public static AsyncOperation UnloadSceneAsync(int id)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.UnloadSceneAsync(id);
        }

        public static AsyncOperation UnloadSceneAsync(LEVEL id)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.UnloadSceneAsync((int)id);
        }

        public static AsyncOperation UnloadSceneAsync(Scene scene)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.UnloadSceneAsync(scene.name);
        }

        public static AsyncOperation UnloadSceneAsync(string sceneName)
        {
            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
            return SceneManager.UnloadSceneAsync(sceneName);
        }

        #region Fade then Load

        /// <summary>
        /// Fades for a given amount of time then loads the selected scene.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static IEnumerator FadeThenLoad(this LEVEL level, float timer)
        {
            yield return new WaitForSeconds(timer);

            var async = LoadSceneAsync(level, LoadSceneMode.Additive);

            while (!async.isDone)
            {
                yield return null;
            }

            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;

            var asyncUnload = UnloadSceneAsync((int)GameManager.Instance.previousLevel);

            while (!asyncUnload.isDone)
            {
                yield return null;
            }

        }

        /// <summary>
        /// Fades for a given amount of time then loads the selected scene.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static IEnumerator FadeThenLoad(float timer, LEVEL additionalLevelToLoad = LEVEL.MAINMENU)
        {
            yield return new WaitForSeconds(timer);

            LoadScene(selectedLevel, LoadSceneMode.Single);

            AsyncOperation asyncLoad = LoadSceneAsync(additionalLevelToLoad, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
        }

        /// <summary>
        /// Fades for a given amount of time then loads the selected scene.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static IEnumerator FadeThenLoadAdditiveOnly(float timer, LEVEL additionalLevelToLoad = LEVEL.MAINMENU)
        {
            yield return new WaitForSeconds(timer);

            GameManager.Instance.previousLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;

            AsyncOperation asyncLoad = LoadSceneAsync(additionalLevelToLoad, LoadSceneMode.Additive);

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            GameManager.Instance.currentLevel = (LEVEL)SceneManager.GetActiveScene().buildIndex;
        }

        #endregion

        #endregion
    }
}