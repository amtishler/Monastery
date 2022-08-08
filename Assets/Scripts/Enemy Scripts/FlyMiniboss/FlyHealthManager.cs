using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlyHealthManager : EnemyHealthManager {

    // Start is called before the first frame update
    void Start() {
        //Debug.Log(transform==null);
        config = this.transform.parent.GetComponent<FlyConfig>();
        healthBar = this.transform.Find("HealthBar").GetComponent<RectTransform>();
        damageBar = this.transform.Find("DamageBar").GetComponent<RectTransform>();
        stunBar = this.transform.Find("StunBar").GetComponent<RectTransform>();
        stunFull = this.transform.Find("StunFull").GetComponent<RectTransform>();
        upright = transform.rotation;
    }

    void LateUpdate(){
        transform.rotation = upright;
    }
}