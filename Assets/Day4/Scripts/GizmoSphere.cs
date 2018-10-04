using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GizmoSphere : MonoBehaviour {

    #region Public Variables
    public float radius = 2;
    // Translucent Green by Default
    public Color selectedColor = new Color(0, 1, 0, .75f);
    public Color defaultColor = new Color(1, 0, 0, .75f);
    #endregion

#if UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeGameObject == gameObject)
        {            
            Gizmos.color = selectedColor;
            Gizmos.DrawSphere(transform.position, radius);
        }
        else
        {
            Gizmos.color = defaultColor;
            Gizmos.DrawSphere(transform.position, radius);
        }
    }
#endif
}
