using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    public IObjectPool Origin { get; set; }

    public void PrepareToUse(Vector3 _position, Quaternion _rotation);
    public void ReturnToPool();
}
