using UnityEngine;

namespace Battlerock
{
    /// <summary>
    /// Sets the renderer's material color to a random color.
    /// </summary>
    public class SetRandomColor : MonoBehaviour
    {
        // Initializes things
        void Start()
        {
            gameObject.GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}