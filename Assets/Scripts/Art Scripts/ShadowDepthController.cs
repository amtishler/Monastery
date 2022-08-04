using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowDepthController : MonoBehaviour
{
    SpriteRenderer parentSR;
    SpriteRenderer sR;
    int parentSROrder = 0;

    // Start is called before the first frame update
    void Start()
    {
        sR = GetComponent<SpriteRenderer>();
        parentSR = transform.parent.GetComponent<SpriteRenderer>();
        parentSROrder = parentSR.sortingOrder;
        //Debug.Log(parentSR.sortingLayerName);
    }

    // Update is called once per frame
    void Update()
    {
        sR.sortingLayerName = parentSR.sortingLayerName;
        sR.sortingOrder = parentSROrder - 1;
    }
}
