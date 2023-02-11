using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject targetFollow;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetFollow != null){
            transform.position = new Vector3 (
                targetFollow.transform.position.x,
                targetFollow.transform.position.y,
                -1000);
        }
    }
}
