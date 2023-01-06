using UnityEngine;

public class StaticEnemyAnimHandler : MonoBehaviour
{
    // SECTION - Method ============================================================
    static public float GetAngle(Transform targetTransform, Transform myTransform)
    {
        Vector3 targetDir = targetTransform.position - myTransform.position;
        return Vector3.SignedAngle(targetDir, myTransform.forward, Vector3.up);
    }

    static public int GetIndex(float angle, int lastIndex = 0)
    {
        // https://www.youtube.com/watch?v=qcXEcZmZ8kA

        ///<NOTE>
        ///
        ///     - Cardinals are from player's forward perspective 
        ///     
        ///</NOTE>

        // Note
        //      - Cardinals are from player's forward perspective

        // Front Sprites
        if (angle > -22.5f && angle < 22.6f) // South
            return 0;
        if (angle >= 22.5f && angle < 67.5f) // South-East
            return 7;
        if (angle >= 67.5f && angle < 112.5f) // East
            return 6;
        if (angle >= 112.5f && angle < 157.5f) // North-East
            return 5;

        // Back Sprites
        if (angle <= -157.5f || angle >= 157.5f) // North
            return 4;
        if (angle >= -157.4f && angle < -112.5f) // North-West
            return 3;
        if (angle >= -112.5f && angle < -67.5f) // West
            return 2;
        if (angle >= -67.5f && angle <= -22.5f) // South-West
            return 1;

        return lastIndex;
    }

    static public void SetSpriteFlip(Transform mySpriteTransform, float angle)
    {
        Vector3 tempLocalScale = Vector3.one;
        if (angle > 0)
            tempLocalScale.x *= -1.0f;

        mySpriteTransform.localScale = tempLocalScale;
    }
}
