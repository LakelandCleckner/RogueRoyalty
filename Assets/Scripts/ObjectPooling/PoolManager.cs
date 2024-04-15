/*
 * Source File Name: PoolManager.cs
 * Author Name: Alexander Maynard
 * Student Number: 301170707
 * Creation Date: March 28th, 2024
 * 
 * Last Modified by: Alexander Maynard
 * Last Modified Date: March 28th, 2024
 * 
 * **NOTE: This script was made by using the GenericPoolManager.cs sript from the COMP397 in-class lab project as a REFERENCE**
 * 
 * 
 * Program Description: 
 *      
 *      This script implements the Object Pooling pattern for various objects.
 * 
 * 
 * Revision History:
 *      -> March 28th, 2024
 *          -Created this script and fully implemented it.
 *          -Added the proper documentation.
 */


using System.Collections.Generic;
using UnityEngine;

public abstract class PoolManager<T> : PersistSingleton<PoolManager<T>> where T : Component
{
    [Header("Size of the object pool")]
    [SerializeField] private int _initialPoolSize;
    [Header("Object to be in the pool")]
    [SerializeField] private T _prefabToBePooled;
    private Queue<T> _prefabPool = new Queue<T>();


    //initially populate the pool before hand to a set size
    private void Start()
    {
        for (int i = 0; i < _initialPoolSize; i++)
        {
            AddPrefabToPool();
        }   
    }

    //to get a pooled item and remove it from the queue
    public T GetPrefabFromPool()
    {
        if(_prefabPool.Count == 0)
        {
            AddPrefabToPool();
        }

        return _prefabPool.Dequeue();
    }

    //in case the pool is too small for the object
    private void AddPrefabToPool()
    {
        //instantiate an object (type T) and add it to the queue as deactivated
        var prefabToBeAdded = Instantiate(_prefabToBePooled);
        prefabToBeAdded.gameObject.SetActive(false);
        _prefabPool.Enqueue(prefabToBeAdded);
    }

    //to return an item and put it in the object pool as deactivated
    public void ReturnPrefabToPool(T prefabToBeReturned)
    {
        prefabToBeReturned.gameObject.SetActive(false);
        _prefabPool.Enqueue(prefabToBeReturned);
    }
}
