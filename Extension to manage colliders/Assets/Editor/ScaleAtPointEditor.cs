using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScaleAtPoint))]
[CanEditMultipleObjects]
public class ScaleAtPointEditor : Editor
{
    public void OnSceneGUI()
    {
        ScaleAtPoint t = (target as ScaleAtPoint);

        EditorGUI.BeginChangeCheck();
        Vector3 scale = Handles.ScaleHandle(t.scaleBoxCollider, t.positionBoxCollider +
            t.PositionGameObject,  Quaternion.identity, 0.7f);
        Vector3 position = Handles.PositionHandle(t.positionBoxCollider +
            t.PositionGameObject,  Quaternion.identity);
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Scaled ScaleAt Point");
            Undo.RecordObject(target, "Changed Look Target");
            t.scaleBoxCollider = scale;
            t.positionBoxCollider = position - t.PositionGameObject;
            t.Update();
        }
    }
}
