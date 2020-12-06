using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DependencyInjector : MonoBehaviour
{
    static Dictionary<System.Type, System.Object> dependencies = new Dictionary<System.Type, System.Object>();
    public static T GetDependency<T>()
    {
        if (!dependencies.ContainsKey(typeof(T)))
        {
            Debug.LogError("Cannot find: " + typeof(T).ToString() +"");
            return default(T);
        }
        return (T)dependencies[typeof(T)];
    }
    public static void AddDependency<T>(System.Object obj)
    {
        if (dependencies.ContainsKey(typeof(T)))
        {
            Debug.Log("There's already an object of type: " + typeof(T).ToString());
            Debug.Log("Object 1: " + dependencies[typeof(T)].GetType().ToString());
            Debug.Log("Object 2: " + obj.GetType().ToString());
            dependencies.Remove(typeof(T));
        }
        dependencies.Add(typeof(T), obj);
    }
}
