using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CinematicPerspective
{
    public static class ExtensionMethdos
    {

        /// <summary>
        /// Remueve la primera instancia de un string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="remove"></param>
        /// <returns></returns>
        public static string RemoveFirst(this string source, string remove)
        {
            int index = source.IndexOf(remove);
            return (index < 0)
                ? source
                : source.Remove(index, remove.Length);
        }

        /// <summary>
        /// Get a trasnform by name recursively from this (parent) transform 
        /// </summary>
        /// <param name="t">This (parent) transform</param>
        /// <param name="n">Name to find</param>
        /// <returns>The first transform with name n</returns>
        public static Transform FindChildRecursive(this Transform t, string n)
        {
            foreach (Transform s in t.GetComponentsInChildren<Transform>())
            {
                if (s.name == n)
                    return s;
            }
            return null;
        }

        /// <summary>
        /// Get all components in children without parent
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Transform[] GetComponentsInChildrenMinusParent(this Transform t)
        {
            List<Transform> tArray = new List<Transform>();

            foreach (Transform s in t.GetComponentsInChildren<Transform>())
            {
                if (s.GetInstanceID() != t.GetInstanceID())
                    tArray.Add(s);
            }

            return tArray.ToArray();
        }

        public static Vector3 WithX(this Vector3 v, float x)
        {
            return new Vector3(x, v.y, v.z);
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            return new Vector3(v.x, y, v.z);
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            return new Vector3(v.x, v.y, z);
        }

        /// <summary>
        /// The distance between 2 vectors 3 in xz, with y=0
        /// </summary>
        /// <param name="a">First Vector</param>
        /// <param name="b">Second Vector</param>
        /// <returns></returns>
        public static float DistanceY0(this Vector3 a, Vector3 b)
        {
            a = new Vector3(a.x, 0, a.z);
            b = new Vector3(b.x, 0, b.z);

            return Vector3.Distance(a, b);
        }

        /// <summary>
        /// Para poder iterar por los enums
        /// </summary>
        public static T Next<T>(this T src) where T : struct
        {
            if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(T).FullName));

            T[] Arr = (T[])Enum.GetValues(src.GetType());
            int j = Array.IndexOf<T>(Arr, src) + 1;
            return (Arr.Length == j) ? Arr[0] : Arr[j];
        }
    }
}

