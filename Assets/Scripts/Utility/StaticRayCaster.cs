using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRayCaster : MonoBehaviour
{
    // SECTION - Method =========================================================
    #region REGION - Line Raycast
    static private void RaycastLineDebugger(Vector3 origin, Vector3 direction, float distance, bool isDebugOn)
    {
        if (isDebugOn)
        {
            float endX = origin.x + direction.x * distance;
            float endY = origin.y + direction.y * distance;
            float endZ = origin.z + direction.z * distance;
            Vector3 end = new(endX, endY, endZ);

            Debug.DrawLine(origin, end, Color.yellow);
        }
    }

    static public RaycastHit IsLineCastTouching(Vector3 origin, Vector3 direction, float distance, LayerMask mask, bool isDebugOn = false)
    {
        RaycastLineDebugger(origin, direction, distance, isDebugOn);

        Physics.Raycast(origin, direction, out RaycastHit hit, distance, mask);
        return hit;
    }

    static public RaycastHit[] IsLineCastTouchingMultiple(Vector3 origin, Vector3 direction, float distance, LayerMask mask, bool isDebugOn = false)
    {
        RaycastLineDebugger(origin, direction, distance, isDebugOn);

        return Physics.RaycastAll(origin, direction, distance, mask);
    }

    #endregion

    #region REGION - Sphere Raycast
    public static void RaycastSphereDebugger(Transform originTransform, float distance, bool isDebugOn)
    {
        if (isDebugOn)
        {
            // Get lengths of lines
            Vector3 endforwardPositive = originTransform.position + Vector3.forward * distance;
            Vector3 endforwardNegative = originTransform.position + -Vector3.forward * distance;

            Vector3 endUpPositive = originTransform.position + Vector3.up * distance;
            Vector3 endUpNegative = originTransform.position + -Vector3.up * distance;

            Vector3 endRightPositive = originTransform.position + Vector3.right * distance;
            Vector3 endRightNegative = originTransform.position + -Vector3.right * distance;


            // PrintSimple lines
            Debug.DrawLine(originTransform.position, endforwardPositive, Color.yellow);
            Debug.DrawLine(originTransform.position, endforwardNegative, Color.yellow);

            Debug.DrawLine(originTransform.position, endUpPositive, Color.blue);
            Debug.DrawLine(originTransform.position, endUpNegative, Color.blue);

            Debug.DrawLine(originTransform.position, endRightPositive, Color.red);
            Debug.DrawLine(originTransform.position, endRightNegative, Color.red);
        }
    }

    public static void RaycastSphereDebugger(Vector3 position, float distance, bool isDebugOn)
    {
        if (isDebugOn)
        {
            // Get lengths of lines
            Vector3 endforwardPositive = position + Vector3.forward * distance;
            Vector3 endforwardNegative = position + -Vector3.forward * distance;

            Vector3 endUpPositive = position + Vector3.up * distance;
            Vector3 endUpNegative = position + -Vector3.up * distance;

            Vector3 endRightPositive = position + Vector3.right * distance;
            Vector3 endRightNegative = position + -Vector3.right * distance;


            // PrintSimple lines
            Debug.DrawLine(position, endforwardPositive, Color.yellow);
            Debug.DrawLine(position, endforwardNegative, Color.yellow);

            Debug.DrawLine(position, endUpPositive, Color.blue);
            Debug.DrawLine(position, endUpNegative, Color.blue);

            Debug.DrawLine(position, endRightPositive, Color.red);
            Debug.DrawLine(position, endRightNegative, Color.red);
        }
    }


    static public Collider[] IsOverlapSphereTouching(Transform originTransform, float radius, LayerMask mask, bool isDebugOn = false)
    {
        RaycastSphereDebugger(originTransform, radius, isDebugOn);

        return Physics.OverlapSphere(originTransform.position, radius, mask);
    }

    static public Collider[] IsOverlapSphereTouching(Vector3 position, float radius, LayerMask mask, bool isDebugOn = false)
    {
        RaycastSphereDebugger(position, radius, isDebugOn);

        return Physics.OverlapSphere(position, radius, mask);
    }
    #endregion
}
