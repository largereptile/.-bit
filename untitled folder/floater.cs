// Script controlling the small circle that follows the cursor

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 posVector = Input.mousePosition - transform.position;
        float angle = Mathf.Atan(posVector.y / posVector.x) / (2 * Mathf.PI) * 360;
        transform.rotation = Quaternion.Euler(0, 0, angle*1.5f);

    }
}
