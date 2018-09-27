using UnityEngine;
using System.Collections;

public static class IE_ExtensionMethods
{
    public static Vector3 GetRandomized(Vector3 min, Vector3 max)
    {
        return new Vector3(
                    Random.Range(min.x, max.x),
                    Random.Range(min.y, max.y),
                    Random.Range(min.z, max.z)
                );
    }

    public static Vector3 StringToVector3(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector3(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]),
            float.Parse(sArray[2]));

        return result;
    }

    public static Vector2 StringToVector2(string sVector)
    {
        // Remove the parentheses
        if (sVector.StartsWith("(") && sVector.EndsWith(")"))
        {
            sVector = sVector.Substring(1, sVector.Length - 2);
        }

        // split the items
        string[] sArray = sVector.Split(',');

        // store as a Vector3
        Vector3 result = new Vector2(
            float.Parse(sArray[0]),
            float.Parse(sArray[1]));

        return result;
    }

    public static Color ParseColor(string col)
    {
        //Takes strings formatted with numbers and no spaces before or after the commas:
        // "1.0,1.0,.35,1.0"
        var strings = col.Split(","[0]);

        Color output = new Color(0, 0, 0, 0);
        for (int i = 0; i < 4; i++)
        {
            output[i] = System.Single.Parse(strings[i]);
        }
        return output;
    }


}
