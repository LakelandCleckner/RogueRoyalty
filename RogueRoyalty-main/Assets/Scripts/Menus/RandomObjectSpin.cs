using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// AUTHOR: Alexander Maynard (301170707)
/// NO DOCUMENTATION NEEDED HERE. THE FILE WILL BE DELETED BEFORE SUBMISSION
/// </summary>
/// 
public class RandomObjectSpin : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Vector3 _rotationVelocity;


    // Start is called before the first frame update
    void Start()
    {
        this._rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0,100) * Time.deltaTime);
        _rigidbody.MoveRotation(_rigidbody.rotation * rotation);      
    }
}
