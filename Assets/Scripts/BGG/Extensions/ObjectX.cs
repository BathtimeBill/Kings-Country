using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


public static class ObjectX
{
    /// <summary>
    /// Toggles active or inactive all GameObjects in a list
    /// </summary>
    /// <param name="gos">The list of objects to toggle</param>
    /// <param name="state">The state to toggle the objects (true/false)</param>
    static public void ToggleObjects(List<GameObject> gos, bool state)
    {
        for (int i = 0; i < gos.Count; i++)
            gos[i].SetActive(state);
    }

    /// <summary>
    /// Toggles active or inactive all GameObjects in an array
    /// </summary>
    /// <param name="gos">The array of objects to toggle</param>
    /// /// <param name="state">The state to toggle the objects (true/false)</param>
    static public void ToggleObjects(GameObject[] gos, bool state)
    {
        for (int i = 0; i < gos.Length; i++)
            gos[i].SetActive(state);
    }

    /// <summary>
    /// Scales a GameObject to zero
    /// </summary>
    /// <param name="_go">The GameObject to scale</param>
    static public void ScaleObjectToZero(GameObject _go)
    {
        _go.transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// Scales all GameObjects in a list to zero
    /// </summary>
    /// <param name="gos">The list of objects to scale</param>
    static public void ScaleObjectsToZero(List<GameObject> gos)
    {
        for (int i = 0; i < gos.Count; i++)
            gos[i].transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// Scales all GameObjects in an array to zero
    /// </summary>
    /// <param name="gos">The array of objects to scale</param>
    static public void ScaleObjectsToZero(GameObject[] gos)
    {
        for (int i = 0; i < gos.Length; i++)
            gos[i].transform.localScale = Vector3.zero;
    }

    /// <summary>
    /// Scales all GameObjects in a list to zero
    /// </summary>
    /// <param name="_gos">The list of objects to scale</param>
    static public void ScaleObjectsToZero(List<GameObject> _gos, Vector3 _axis)
    {
        for (int i = 0; i < _gos.Count; i++)
            _gos[i].transform.localScale = _axis;
    }

    /// <summary>
    /// Rotates a GameObject to zero
    /// </summary>
    /// <param name="_go">The GameObject to rotate</param>
    static public void RotateObjectToZero(GameObject _go)
    {
        _go.transform.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Rotates all GameObjects in a list to zero
    /// </summary>
    /// <param name="gos">The list of objects to rotate</param>
    static public void RotateObjectsToZero(List<GameObject> gos)
    {
        for (int i = 0; i < gos.Count; i++)
            gos[i].transform.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Rotates all GameObjects in an array to zero
    /// </summary>
    /// <param name="gos">The array of objects to rotate</param>
    static public void RotateObjectsToZero(GameObject[] gos)
    {
        for (int i = 0; i < gos.Length; i++)
            gos[i].transform.localEulerAngles = Vector3.zero;
    }

    /// <summary>
    /// Fades all GameObjects in a list to zero
    /// </summary>
    /// <param name="gos">The list of objects to fade</param>
    static public void FadeObjectsToZero(List<GameObject> gos, float FadeTime = 0)
    {
        for (int i = 0; i < gos.Count; i++)
        {
            Color temp = gos[i].GetComponent<Renderer>().material.color;
            temp.a = 0;
            gos[i].GetComponent<Renderer>().material.color = temp;
        }
            
    }

    //
    // Copy component to another GameObject
    // http://answers.unity3d.com/answers/589400/view.html
    static public T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        CopyComponentFields(original, copy);
        return copy as T;
    }
    static public void CopyComponentFields<T>(T original, T copy) where T : Component
    {
        System.Type type = original.GetType();
        FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
        //Debug.Log("Fields ["+fields.Length+"]");
        foreach (FieldInfo field in fields)
        {
            //Debug.Log("Field ["+field+"]");
            field.SetValue(copy, field.GetValue(original));
        }
    }

    //
    // Copy component to another GameObject
    // http://answers.unity3d.com/answers/589400/view.html
    static public T GetOrAddComponent<T>(GameObject obj) where T : Component
    {
        T comp = obj.GetComponent<T>();
        if (comp == null)
            comp = obj.AddComponent<T>();
        return comp;
    }

    /// <summary>
    /// Sets all children of an objects layers to a new layer
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="layer"></param>
    public static void SetLayerRecursively(this Transform parent, int layer)
    {
        parent.gameObject.layer = layer;

        for (int i = 0, count = parent.childCount; i < count; i++)
        {
            parent.GetChild(i).SetLayerRecursively(layer);
        }
    }
}