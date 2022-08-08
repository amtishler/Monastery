using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour {

    const float HEALTH_WIDTH = 2.380001f;
    const float HEALTH_HEIGHT = 0.5f;

    const float STUN_WIDTH = 2.330002f;
    const float STUN_HEIGHT = 0.5f;

    [Header("Variables")]
    [SerializeField] float healthBarSpeed;
    [SerializeField] float damageBarSpeed;
    protected CharacterConfig config;
    protected RectTransform healthBar;
    protected RectTransform damageBar;
    protected RectTransform stunBar;
    protected RectTransform stunFull;
    protected float currentHealth;
    protected float currentDamage;
    protected float enemyHealth;

    protected float enemyStun;

    protected Quaternion upright;

    // Start is called before the first frame update
    void Start() {
        //Debug.Log(transform==null);
        config = this.transform.parent.GetComponent<CharacterConfig>();
        healthBar = this.transform.Find("HealthBar").GetComponent<RectTransform>();
        damageBar = this.transform.Find("DamageBar").GetComponent<RectTransform>();
        stunBar = this.transform.Find("StunBar").GetComponent<RectTransform>();
        stunFull = this.transform.Find("StunFull").GetComponent<RectTransform>();
        upright = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
        enemyHealth = (config.Health / config.MaxHealth)*HEALTH_WIDTH;
        enemyStun = (config.Stun / config.MaxStun)*STUN_WIDTH;


        currentHealth = healthBar.sizeDelta[0];
        currentDamage = damageBar.sizeDelta[0];

        // Scaling health bar
        if (currentHealth < enemyHealth) {
            currentHealth = currentHealth + healthBarSpeed*Time.deltaTime;
            if (currentHealth > enemyHealth) currentHealth = enemyHealth;
        } else if (currentHealth > enemyHealth) {
            currentHealth = currentHealth - healthBarSpeed*Time.deltaTime;
            if (currentHealth < enemyHealth) currentHealth = enemyHealth;
        }
        // scaling damage bar
        if (currentDamage > enemyHealth) {
            currentDamage = currentDamage - damageBarSpeed*Time.deltaTime;
        } if (currentDamage < enemyHealth) currentDamage = enemyHealth;

        if(enemyStun == STUN_WIDTH)
        {
            stunFull.sizeDelta = new Vector2(STUN_WIDTH, STUN_HEIGHT);
        }
        else
        {
            stunFull.sizeDelta = new Vector2(0, STUN_HEIGHT);
        }

        healthBar.sizeDelta = new Vector2(currentHealth, HEALTH_HEIGHT);
        damageBar.sizeDelta = new Vector2(currentDamage, HEALTH_HEIGHT);

        stunBar.sizeDelta = new Vector2(enemyStun, STUN_HEIGHT);

    }

    void LateUpdate(){
        transform.rotation = upright;
    }
}