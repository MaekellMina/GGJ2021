
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SystemHelper
{   
    //performs deep copy on any object type
    //use this method if you wish to create a deep copy of an object instead of shallow copy
	public static T DeepClone<T>(this T obj)
    {
		string json = JsonUtility.ToJson(obj);

		return JsonUtility.FromJson<T>(json);
    }

	public static void TryChangeParent(Transform child, Transform parent)
	{
		if(child.parent != parent)
		{
			child.SetParent(parent);
			child.localScale = Vector3.one;
		}
	}
}
