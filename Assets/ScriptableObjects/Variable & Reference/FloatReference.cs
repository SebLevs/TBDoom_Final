using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FloatReference
{
    [SerializeField] private bool UseConstant = true;
    [SerializeField] private float ConstantValue;
    [SerializeField] private FloatVariable Variable;

    public float Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { if (UseConstant) ConstantValue = value; else Variable.Value = value; }
    }
}
