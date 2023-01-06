using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class StringReference
{
    [SerializeField] private bool UseConstant = true;
    [SerializeField] private string ConstantValue;
    [SerializeField] private StringVariable Variable;

    public string Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set { if (UseConstant) ConstantValue = value; else Variable.Value = value; }
    }
}
