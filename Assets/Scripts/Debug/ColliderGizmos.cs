using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;

public class ColliderGizmos : MonoBehaviour
{
    public Color gizmoColor;
    public FillType fillType;

    private void OnDrawGizmos()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        Gizmos.color = gizmoColor;
        var bcTrf = bc.transform;
        var rotationMatrix = Matrix4x4.TRS(bcTrf.position, bcTrf.rotation, bcTrf.lossyScale);
        Gizmos.matrix = rotationMatrix;
        if (fillType == FillType.wiredCube)
            Gizmos.DrawWireCube(bc.center, bc.size);
        else if (fillType == FillType.filledCube)
            Gizmos.DrawCube(bc.center, bc.size);
    }


    public enum FillType
    {
        wiredCube,
        filledCube
    }
}       