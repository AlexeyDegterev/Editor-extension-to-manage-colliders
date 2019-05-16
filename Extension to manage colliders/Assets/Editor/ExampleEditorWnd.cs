using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;

public class ExampleEditorWnd : EditorWindow
{
    [MenuItem("Window/ExtensionToManageColliders")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ExampleEditorWnd));
    }

    public GameObject gameObject;
    List<BoxCollider> listBoxColliders;
    BoxCollider currentBoxCollider;

    //bool ShowHide = true;
    int nBoxColliderIndex = 0;
    
    private void OnGUI()
    {
        gameObject = (GameObject)EditorGUILayout.ObjectField("Current Game Object",gameObject, typeof(GameObject), true);
        
        if (GUILayout.Button("GET Box Colliders"))
        {
            listBoxColliders = new List<BoxCollider>();
            foreach (var boxCollider in gameObject.GetComponents<BoxCollider>())
            {
                listBoxColliders.Add(boxCollider);
            }
        }
        if (GUILayout.Button("ADD box collider"))
        {
            currentBoxCollider = gameObject.AddComponent<BoxCollider>() as BoxCollider;
            listBoxColliders.Add(currentBoxCollider);
        }

        if (GUILayout.Button("CLONE this box collider"))
        {
            BoxCollider newBoxCollider = gameObject.AddComponent<BoxCollider>() as BoxCollider;
            listBoxColliders = new List<BoxCollider>();
            foreach (var boxCollider in gameObject.GetComponents<BoxCollider>())
            {
                listBoxColliders.Add(boxCollider);
            }
            newBoxCollider.center = listBoxColliders[nBoxColliderIndex].center;
            newBoxCollider.size = listBoxColliders[nBoxColliderIndex].size;
        }
        if (GUILayout.Button("DELETE this box collider"))
        {
            if (listBoxColliders.Count == 0)
                return;
            currentBoxCollider = listBoxColliders[nBoxColliderIndex];
            DestroyImmediate(currentBoxCollider);
            listBoxColliders.RemoveAt(nBoxColliderIndex);
        }

        if (GUILayout.Button("NEXT Box Collider"))
            NextBoxCollider();
        if (GUILayout.Button("PREVIOUS Box Collider"))
            PreviousBoxCollider();
        EditorGUILayout.IntField("Current Box Collider Index", nBoxColliderIndex);        
    }

    void ShowBoxColliderInInspector(int nColliderIndex)
    {
        for (int c = 0; c < listBoxColliders.Count; c++)
        {
            if(c!= nColliderIndex)
            {
                listBoxColliders[c].hideFlags = HideFlags.HideInInspector;
            }else
            {
                listBoxColliders[c].hideFlags = HideFlags.None;
            }
        }
    }

    void EditCollider(BoxCollider bc, int nColliderIndex)
    {
        EditMode.ChangeEditMode(EditMode.SceneViewEditMode.None, new Bounds(), null);
        Type colliderEditorBase = System.Type.GetType("UnityEditor.ColliderEditorBase,UnityEditor.dll");
        Editor[] colliderEditors = Resources.FindObjectsOfTypeAll(colliderEditorBase) as Editor[];
        Editor[] reverseColliderEditors = new Editor[colliderEditors.Length];
        for (int c = 0; c < colliderEditors.Length; c++)
            reverseColliderEditors[c] = colliderEditors[colliderEditors.Length - c - 1];
        
        EditMode.ChangeEditMode(EditMode.SceneViewEditMode.Collider, 
            bc.bounds,
            reverseColliderEditors[nColliderIndex]);
    }

    void PreviousBoxCollider()
    {
        nBoxColliderIndex--;
        nBoxColliderIndex = nBoxColliderIndex < 0 ? listBoxColliders.Count - 1 : nBoxColliderIndex;
        //ShowBoxColliderInInspector(nBoxColliderIndex);
        EditCollider(listBoxColliders[nBoxColliderIndex], nBoxColliderIndex);
        SceneView.RepaintAll();
        SetScaleAtPointProperties();
    }

    void NextBoxCollider()
    {
        nBoxColliderIndex++;
        if (nBoxColliderIndex > listBoxColliders.Count - 1)
            nBoxColliderIndex = 0;
        //ShowBoxColliderInInspector(nBoxColliderIndex);
        SceneView.RepaintAll();
        EditCollider(listBoxColliders[nBoxColliderIndex], nBoxColliderIndex);
        SetScaleAtPointProperties();
    }

    void SetScaleAtPointProperties()
    {
        gameObject.GetComponent<ScaleAtPoint>().bc = listBoxColliders[nBoxColliderIndex];
        gameObject.GetComponent<ScaleAtPoint>().scaleBoxCollider = listBoxColliders[nBoxColliderIndex].size;
        gameObject.GetComponent<ScaleAtPoint>().positionBoxCollider = listBoxColliders[nBoxColliderIndex].center;
    }

}
