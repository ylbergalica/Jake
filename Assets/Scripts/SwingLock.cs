using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingLock : MonoBehaviour
{
    public GameObject itemReference;

    private Quaternion lockRot;
    private Quaternion lockRotParent;
    // private Vector3 lockPos;

    // Start is called before the first frame update
    void Start()
    {
        lockRotParent = transform.parent.rotation;
        lockRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.parent.rotation = lockRotParent;
        transform.rotation = lockRot;
    }
}
