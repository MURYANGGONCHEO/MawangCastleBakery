using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
public class TypeFilterAttribute : PropertyAttribute
{
    public Type BaseType { get; private set; }

    public TypeFilterAttribute(Type baseType)
    {
        BaseType = baseType;
    }
}