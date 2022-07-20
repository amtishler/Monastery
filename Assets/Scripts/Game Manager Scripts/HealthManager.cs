using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    const float WIDTH = 11.125f;
    const float HEIGHT = 0.75f;
    const float SHINE_WIDTH = 11.5f;
    const float SHINE_HEIGHT = 1f;

    [Header("Objects")]
    [SerializeField] GameObject health;
    [SerializeField] Sprite green;
    [SerializeField] Sprite yellow;
    [SerializeField] Sprite orange;
    [SerializeField] Sprite red;
    [Header("Variables")]
    [SerializeField] float healthBarSpeed;
    [SerializeField] float damageBarSpeed;
    [SerializeField] float yellowBreakpoint;
    [SerializeField] float orangeBreakpoint;
    [SerializeField] float redBreakpoint;
    private PlayerConfig config;
    private RectTransform healthBar;
    private RectTransform shineBar;
    private RectTransform damageBar;
    private Image color;
    private float currentHealth;
    private float currentDamage;
    private float playerHealth;

    // Start is called before the first frame update
    void Start() {
        //Debug.Log(transform==null);
        config = GameObject.FindObjectOfType<PlayerConfig>();
        Transform bar = transform.Find("Health");
        healthBar = bar.Find("healthBar").GetComponent<RectTransform>();
        shineBar  = bar.Find("shine").GetComponent<RectTransform>();
        damageBar = bar.Find("damageBar").GetComponent<RectTransform>();
        color = bar.Find("healthBar").GetComponent<Image>();
    }

    // Update is called once per frame
    void Update() {
        Debug.Log(config.Health.ToString());
        playerHealth = (config.Health / config.MaxHealth)*WIDTH;
        currentHealth = healthBar.sizeDelta[0];
        currentDamage = damageBar.sizeDelta[0];

        // Scaling health bar
        if (currentHealth < playerHealth) {
            currentHealth = currentHealth + healthBarSpeed*Time.deltaTime;
            Debug.Log(currentHealth);
            if (currentHealth > playerHealth) currentHealth = playerHealth;
        } else if (currentHealth > playerHealth) {
            currentHealth = currentHealth - healthBarSpeed*Time.deltaTime;
            if (currentHealth < playerHealth) currentHealth = playerHealth;
        }
        // scaling damage bar
        if (currentDamage > playerHealth) {
            currentDamage = currentDamage - damageBarSpeed*Time.deltaTime;
        } if (currentDamage < playerHealth) currentDamage = playerHealth;

        healthBar.sizeDelta = new Vector2(currentHealth, HEIGHT);
        shineBar.sizeDelta  = new Vector2(currentHealth*SHINE_WIDTH/WIDTH, SHINE_HEIGHT);
        damageBar.sizeDelta = new Vector2(currentDamage, HEIGHT);

        

        // Changing colors
        if (playerHealth < WIDTH*redBreakpoint) {
            color.sprite = red; return;
        }
        if (playerHealth < WIDTH*orangeBreakpoint) {
            color.sprite = orange; return;
        }
        if (playerHealth < WIDTH*yellowBreakpoint) {
            color.sprite = yellow; return;
        } else {
            color.sprite = green;
        }
        //Debug.Log(playerHealth);
    }
}