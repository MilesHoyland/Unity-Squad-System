using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GizmoExample))]
public class GizmosEditor : Editor
{
    #region Public Variables
    GizmoExample targetScript;
    #endregion

    #region Private Variables
    #endregion


    #region Main Methods
    private void OnEnable()
    {
        targetScript = (GizmoExample)target;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // to show public data members in the inspector
    }
    #endregion

    #region Utility Methods

    private void OnDrawGizmos()
    {
       
    }

    private void OnDrawGizmosSelected()
    {

    }

    #endregion
}
