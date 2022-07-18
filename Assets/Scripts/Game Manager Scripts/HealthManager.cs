using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour {

    private PlayerConfig config;
    [SerializeField] GameObject health;

    private Vector2 defaultHealthDim;

    // Start is called before the first frame update
    void Start() {
        config = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConfig>();
        defaultHealthDim = health.GetComponent<RectTransform>().sizeDelta;
    }

    // Update is called once per frame
    void Update() {
        UpdateHealth();
    }

    private void UpdateHealth() {
        RectTransform bar = health.GetComponent<RectTransform>();
        float barScale = config.Health / config.MaxHealth;
        bar.sizeDelta = new Vector2(defaultHealthDim.x*barScale, defaultHealthDim.y);
    }
}
