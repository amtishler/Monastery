using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthManager : MonoBehaviour {

    const float HEALTH_WIDTH = 5f;
    const float HEALTH_HEIGHT = .6f;

    const float STUN_WIDTH = 5f;
    const float STUN_HEIGHT = .9f;

    const float FULL_STUN_WIDTH = 5.3f;

    [Header("Variables")]
    [SerializeField] float healthBarSpeed;
    [SerializeField] float damageBarSpeed;

    public Sprite normalimg;
    public Sprite stunnedimg;

    protected CharacterConfig config;
    protected RectTransform healthBar;
    protected RectTransform damageBar;
    protected RectTransform stunBar;
    protected RectTransform stunFull;
    protected float currentHealth;
    protected float currentDamage;
    protected float enemyHealth;

    protected Image stunBarImage;

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

        stunBarImage = this.transform.Find("StunBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {

        if(config.stunned){
            stunBarImage.sprite = stunnedimg;
        }else{
            stunBarImage.sprite = normalimg;
        }

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
            stunFull.sizeDelta = new Vector2(FULL_STUN_WIDTH, STUN_HEIGHT);
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