using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public GameObject ground;
    private void Update()
    {
        transform.GetComponent<Camera>().orthographicSize = ground.transform.localScale.x/2;
    }
}
