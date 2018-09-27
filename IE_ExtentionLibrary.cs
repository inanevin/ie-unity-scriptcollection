using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text;
using UnityEngine;

public static class IE_ExtentionLibrary {

    /// <summary>
    /// Shuffle the list in place using the Fisher-Yates method.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Return a random item from the list.
    /// Sampling with replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RandomItem<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot select a random item from an empty list");
        return list[UnityEngine.Random.Range(0, list.Count)];
    }

    /// <summary>
    /// Removes a random item from the list, returning that item.
    /// Sampling without replacement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RemoveRandom<T>(this IList<T> list)
    {
        if (list.Count == 0) throw new System.IndexOutOfRangeException("Cannot remove a random item from an empty list");
        int index = UnityEngine.Random.Range(0, list.Count);
        T item = list[index];
        list.RemoveAt(index);
        return item;
    }

    public static string Truncate(this string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return value;
        return value.Length <= maxLength ? value : value.Substring(0, maxLength);
    }

    // Named format strings from object attributes. Eg:
    // string blaStr = aPerson.ToString("My name is {FirstName} {LastName}.")
    public static string ToString(this object anObject, string aFormat)
    {
        return ToString(anObject, aFormat, null);
    }

    public static string ToString(this object anObject, string aFormat, IFormatProvider formatProvider)
    {
        StringBuilder sb = new StringBuilder();
        Type type = anObject.GetType();
        Regex reg = new Regex(@"({)([^}]+)(})", RegexOptions.IgnoreCase);
        MatchCollection mc = reg.Matches(aFormat);
        int startIndex = 0;
        foreach (Match m in mc)
        {
            Group g = m.Groups[2]; //it's second in the match between { and }
            int length = g.Index - startIndex - 1;
            sb.Append(aFormat.Substring(startIndex, length));

            string toGet = string.Empty;
            string toFormat = string.Empty;
            int formatIndex = g.Value.IndexOf(":"); //formatting would be to the right of a :
            if (formatIndex == -1) //no formatting, no worries
            {
                toGet = g.Value;
            }
            else //pickup the formatting
            {
                toGet = g.Value.Substring(0, formatIndex);
                toFormat = g.Value.Substring(formatIndex + 1);
            }

            //first try properties
            PropertyInfo retrievedProperty = type.GetProperty(toGet);
            Type retrievedType = null;
            object retrievedObject = null;
            if (retrievedProperty != null)
            {
                retrievedType = retrievedProperty.PropertyType;
                retrievedObject = retrievedProperty.GetValue(anObject, null);
            }
            else //try fields
            {
                FieldInfo retrievedField = type.GetField(toGet);
                if (retrievedField != null)
                {
                    retrievedType = retrievedField.FieldType;
                    retrievedObject = retrievedField.GetValue(anObject);
                }
            }

            if (retrievedType != null) //Cool, we found something
            {
                string result = string.Empty;
                if (toFormat == string.Empty) //no format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, null) as string;
                }
                else //format info
                {
                    result = retrievedType.InvokeMember("ToString",
                        BindingFlags.Public | BindingFlags.NonPublic |
                        BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.IgnoreCase
                        , null, retrievedObject, new object[] { toFormat, formatProvider }) as string;
                }
                sb.Append(result);
            }
            else //didn't find a property with that name, so be gracious and put it back
            {
                sb.Append("{");
                sb.Append(g.Value);
                sb.Append("}");
            }
            startIndex = g.Index + g.Length + 1;
        }
        if (startIndex < aFormat.Length) //include the rest (end) of the string
        {
            sb.Append(aFormat.Substring(startIndex));
        }
        return sb.ToString();
    }

    public static int WithRandomSign(this int value, float negativeProbability = 0.5f)
    {
        return UnityEngine.Random.value < negativeProbability ? -value : value;
    }

    //********************* ALL VECTORS ********************* 

    public static Vector2 IE_xy(this Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }

    public static Vector2 IE_yz(this Vector3 v)
    {
        return new Vector2(v.y, v.z);
    }

    public static Vector2 IE_xz(this Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }
    
    public static Vector3 IE_ChangeX(this Vector3 v, float x)
    {
        return new Vector3(x, v.y, v.z);
    }

    public static Vector3 IE_ChangeY(this Vector3 v, float y)
    {
        return new Vector3(v.x, y, v.z);
    }

    public static Vector3 IE_ChangeZ(this Vector3 v, float z)
    {
        return new Vector3(v.x, v.y, z);
    }

    public static Vector3 IE_FlattenX(this Vector3 vector)
    {
        return new Vector3(0.0f, vector.y, vector.z);
    }

    public static Vector3 IE_FlattenY(this Vector3 vector)
    {
        return new Vector3(vector.x, 0.0f, vector.z);
    }

    public static Vector3 IE_FlattenZ(this Vector3 vector)
    {
        return new Vector3(vector.x, vector.y, 0.0f);
    }

    public static Vector2 IE_FlattenX(this Vector2 vector)
    {
        return new Vector2(0.0f, vector.y);
    }

    public static Vector2 IE_FlattenY(this Vector2 vector)
    {
        return new Vector2(vector.x, 0.0f);
    }

    public static Vector3 IE_GetRandom(this Vector3 v, float min, float max)
    {
        return new Vector3(
            UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max));
    }

    public static Vector3 IE_GetRandom(this Vector3 v, float minX, float maxX, float minY, float maxY, float minZ, float maxZ)
    {
        return new Vector3(
            UnityEngine.Random.Range(minX, maxX),
            UnityEngine.Random.Range(minY, maxY),
            UnityEngine.Random.Range(minZ, maxZ));
    }

    public static Vector3 IE_GetRandom(this Vector3 v, float range)
    {
        return new Vector3(
            UnityEngine.Random.Range(-range, range),
            UnityEngine.Random.Range(-range, range),
            UnityEngine.Random.Range(-range, range));
    }

    public static Vector3 IE_GetRandom(this Vector3 v, float rangeX, float rangeY, float rangeZ)
    {
        return new Vector3(
            UnityEngine.Random.Range(-rangeX, rangeX),
            UnityEngine.Random.Range(-rangeY, rangeY),
            UnityEngine.Random.Range(-rangeZ, rangeZ));
    }

    public static Vector2 IE_GetRandom(this Vector2 v, float min, float max)
    {
        return new Vector2(
            UnityEngine.Random.Range(min, max),
            UnityEngine.Random.Range(min, max));
    }

    public static Vector2 IE_GetRandom(this Vector2 v, float minX, float maxX, float minY, float maxY)
    {
        return new Vector2(
            UnityEngine.Random.Range(minX, maxX),
            UnityEngine.Random.Range(minY, maxY));
    }

    public static Vector2 IE_GetRandom(this Vector2 v, float range)
    {
        return new Vector2(
            UnityEngine.Random.Range(-range, range),
            UnityEngine.Random.Range(-range, range));
    }

    // axisDirection - unit vector in direction of an axis (eg, defines a line that passes through zero)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnAxis(this Vector3 axisDirection, Vector3 point, bool isNormalized = false)
    {
        if (!isNormalized) axisDirection.Normalize();
        var d = Vector3.Dot(point, axisDirection);
        return axisDirection * d;
    }

    // lineDirection - unit vector in direction of line
    // pointOnLine - a point on the line (allowing us to define an actual line in space)
    // point - the point to find nearest on line for
    public static Vector3 NearestPointOnLine(this Vector3 lineDirection, Vector3 point, Vector3 pointOnLine, bool isNormalized = false)
    {
        if (!isNormalized) lineDirection.Normalize();
        var d = Vector3.Dot(point - pointOnLine, lineDirection);
        return pointOnLine + (lineDirection * d);
    }

    // floats
    public static float IE_DistanceFlattenedX(this Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin.IE_FlattenX(), destination.IE_FlattenX());
    }

    public static float IE_DistanceFlattenedY(this Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin.IE_FlattenY(), destination.IE_FlattenY());
    }

    public static float IE_DistanceFlattenedZ(this Vector3 origin, Vector3 destination)
    {
        return Vector3.Distance(origin.IE_FlattenZ(), destination.IE_FlattenZ());
    }

    public static float LinearRemap(this float value, float valueRangeMin, float valueRangeMax, float newRangeMin, float newRangeMax)
    {
        return (value - valueRangeMin) / (valueRangeMax - valueRangeMin) * (newRangeMax - newRangeMin) + newRangeMin;
    }

    public static Transform FindDeepChild(this Transform aParent, string aName)
    {
        var result = aParent.Find(aName);
        if (result != null)
            return result;
        foreach (Transform child in aParent)
        {
            result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    public static Transform FindDeepChildUp(this Transform aParent, string aName)
    {
        foreach (Transform child in aParent)
        {
            if (child.name == aName)
                return child;
            var result = child.FindDeepChild(aName);
            if (result != null)
                return result;
        }
        return null;
    }

    public static void IE_LookAtOnlyX(this Transform transform, Vector3 point)
    {
        var lookPos = new Vector3(point.x, transform.position.y, transform.position.z) - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    public static void IE_LookAtOnlyY(this Transform transform, Vector3 point)
    {
        var lookPos = new Vector3(transform.position.x, point.y, transform.position.z) - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    public static void IE_LookAtOnlyZ(this Transform transform, Vector3 point)
    {
        var lookPos = new Vector3(transform.position.x, transform.position.y, point.z) - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    public static void IE_LookAtOnlyXY(this Transform transform, Vector3 point)
    {
        var lookPos = new Vector3(point.x, point.y, transform.position.z) - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    public static void IE_LookAtOnlyXZ(this Transform transform, Vector3 point)
    {
        var lookPos = new Vector3(point.x, transform.position.y, point.z) - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    public static void IE_LookAtOnlyYZ(this Transform transform, Vector3 point)
    {
        var lookPos = new Vector3(transform.position.x, point.y, point.z) - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;
    }

    public static void SetLayerRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
            t.gameObject.SetLayerRecursively(layer);
    }

    public static void SetCollisionRecursively(this GameObject gameObject, bool tf)
    {
        Collider[] colliders = gameObject.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
            collider.enabled = tf;
    }

    public static void SetVisualRecursively(this GameObject gameObject, bool tf)
    {
        Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
            renderer.enabled = tf;
    }

    public static T[] GetComponentsInChildrenWithTag<T>(this GameObject gameObject, string tag)
        where T : Component
    {
        List<T> results = new List<T>();

        if (gameObject.CompareTag(tag))
            results.Add(gameObject.GetComponent<T>());

        foreach (Transform t in gameObject.transform)
            results.AddRange(t.gameObject.GetComponentsInChildrenWithTag<T>(tag));

        return results.ToArray();
    }

    public static T GetComponentInParents<T>(this GameObject gameObject)
        where T : Component
    {
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
                return result;
        }

        return null;
    }

    public static T[] GetComponentsInParents<T>(this GameObject gameObject)
        where T : Component
    {
        List<T> results = new List<T>();
        for (Transform t = gameObject.transform; t != null; t = t.parent)
        {
            T result = t.GetComponent<T>();
            if (result != null)
                results.Add(result);
        }

        return results.ToArray();
    }

    public static int GetCollisionMask(this GameObject gameObject, int layer = -1)
    {
        if (layer == -1)
            layer = gameObject.layer;

        int mask = 0;
        for (int i = 0; i < 32; i++)
            mask |= (Physics.GetIgnoreLayerCollision(layer, i) ? 0 : 1) << i;

        return mask;
    }

    public static Color WithAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }
}
