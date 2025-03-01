﻿using Kingmaker.Blueprints;
using Kingmaker.ResourceLinks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EbonsContentMod.Utilities
{
    internal class AltHelpers
    {
        public static T CreateCopyAlt<T>(T original, Action<T> init = null)
        {
            var result = (T)ObjectDeepCopierAlt.Clone(original);
            init?.Invoke(result);
            return result;
        }

        private class ObjectDeepCopierAlt
        {
            private class ArrayTraverse
            {
                public int[] Position;
                private int[] maxLengths;

                public ArrayTraverse(Array array)
                {
                    maxLengths = new int[array.Rank];
                    for (int i = 0; i < array.Rank; ++i)
                    {
                        maxLengths[i] = array.GetLength(i) - 1;
                    }
                    Position = new int[array.Rank];
                }

                public bool Step()
                {
                    for (int i = 0; i < Position.Length; ++i)
                    {
                        if (Position[i] < maxLengths[i])
                        {
                            Position[i]++;
                            for (int j = 0; j < i; j++)
                            {
                                Position[j] = 0;
                            }
                            return true;
                        }
                    }
                    return false;
                }
            }
            private class ReferenceEqualityComparer : EqualityComparer<Object>
            {
                public override bool Equals(object x, object y)
                {
                    return ReferenceEquals(x, y);
                }
                public override int GetHashCode(object obj)
                {
                    if (obj == null) return 0;
                    if (obj is WeakResourceLink wrl)
                    {
                        if (wrl.AssetId == null)
                        {
                            return "WeakResourceLink".GetHashCode();
                        }
                        else
                        {
                            return wrl.GetHashCode();
                        }
                    }
                    return obj.GetHashCode();
                }
            }
            private static readonly MethodInfo CloneMethod = typeof(Object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

            public static bool IsPrimitive(Type type)
            {
                if (type == typeof(String)) return true;
                return (type.IsValueType & type.IsPrimitive);
            }
            public static Object Clone(Object originalObject, bool shallow = false)
            {
                return InternalCopy(originalObject, new Dictionary<Object, Object>(new ReferenceEqualityComparer()), shallow);
            }


            private static Object InternalCopy(Object originalObject, IDictionary<Object, Object> visited, bool shallow)
            {
                if (originalObject == null) return null;
                var typeToReflect = originalObject.GetType();
                if (IsPrimitive(typeToReflect)) return originalObject;
                if (originalObject is BlueprintReferenceBase) return originalObject;
                if (visited.ContainsKey(originalObject)) return visited[originalObject];
                if (typeof(Delegate).IsAssignableFrom(typeToReflect)) return null;
                var cloneObject = CloneMethod.Invoke(originalObject, null);
                if (!shallow)
                {
                    if (typeToReflect.IsArray)
                    {
                        var arrayType = typeToReflect.GetElementType();
                        if (IsPrimitive(arrayType) == false)
                        {
                            Array clonedArray = (Array)cloneObject;
                            ForEach(clonedArray, (array, indices) => array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited, shallow), indices));
                        }

                    }
                    visited.Add(originalObject, cloneObject);
                    CopyFields(originalObject, visited, cloneObject, typeToReflect);
                    RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
                }
                return cloneObject;

                void ForEach(Array array, Action<Array, int[]> action)
                {
                    if (array.LongLength == 0) return;
                    ArrayTraverse walker = new ArrayTraverse(array);
                    do action(array, walker.Position);
                    while (walker.Step());
                }
            }
            private static void RecursiveCopyBaseTypePrivateFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect)
            {
                if (typeToReflect.BaseType != null)
                {
                    RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                    CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType, BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
                }
            }
            private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject, Type typeToReflect, BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null)
            {
                foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
                {
                    if (filter != null && filter(fieldInfo) == false) continue;
                    if (IsPrimitive(fieldInfo.FieldType)) continue;
                    var originalFieldValue = fieldInfo.GetValue(originalObject);
                    var clonedFieldValue = InternalCopy(originalFieldValue, visited, true);
                    fieldInfo.SetValue(cloneObject, clonedFieldValue);
                }
            }
        }
    }
}
