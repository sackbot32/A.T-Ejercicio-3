using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabObject;
    [SerializeField] private int objectNumberOnStart;
    private List<GameObject> objectsPool = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreateObjects(objectNumberOnStart);

    }
    /// <summary>
    /// Create the objects needed at the beggining of the game
    /// </summary>
    /// <param name="objectNumber"></param>
    private void CreateObjects(int objectNumber)
    {
        for (int i = 0; i < objectNumber; i++)
        {
            CreateNewObj();
        }
    }


    /// <summary>
    /// Instasiate new object and added to the list
    /// </summary>
    /// <returns></returns>
    private GameObject CreateNewObj()
    {
        GameObject newObject = Instantiate(prefabObject, Vector3.zero, Quaternion.identity);
        newObject.SetActive(false);
        objectsPool.Add(newObject);
        return newObject;
    }
    /// <summary>
    /// Take from the list an avaible object
    /// if there are not, create new one
    /// </summary>
    /// <returns></returns>
    public GameObject GetGameObject()
    {
        GameObject gottenObject = null;
        gottenObject = objectsPool.Find(x => x.activeInHierarchy == false);
        if(gottenObject == null)
        {
            gottenObject = CreateNewObj();
        }

        return gottenObject;
    }



}
