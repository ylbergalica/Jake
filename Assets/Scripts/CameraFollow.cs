using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject follow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (follow != null){
            transform.position = new Vector3 (follow.transform.position.x, follow.transform.position.y, -1000);
        }
    }
}
