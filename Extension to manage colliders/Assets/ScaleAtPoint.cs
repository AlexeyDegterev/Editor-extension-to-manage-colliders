using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ScaleAtPoint : MonoBehaviour
{
    public Vector3 scaleBoxCollider;
    public BoxCollider bc;
    public Vector3 positionBoxCollider;

    public Vector3 ScaleGameObject
    {
        get
        {
            return transform.localScale;
        }
    }
    public Vector3 PositionGameObject
    {
        get
        {
            return transform.position;
        }
    }


    public void Update()
    {

        bc.center = positionBoxCollider;
        bc.size = scaleBoxCollider;
    }
}
