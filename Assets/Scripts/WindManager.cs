using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WindManager : MonoBehaviour
{

    [SerializeField] float windAngle = 0;
    Vector3 direction = new Vector3(0, 90, 0);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = windAngle;
        transform.eulerAngles = direction;
    }
}
