using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class TreeShadowScript : MonoBehaviour
{
    UnityEngine.Rendering.Universal.Light2D spriteLight;
    SpriteRenderer sR;

    
    private FieldInfo _LightCookieSprite =  typeof( UnityEngine.Rendering.Universal.Light2D ).GetField( "m_LightCookieSprite", BindingFlags.NonPublic | BindingFlags.Instance );
 

    // Start is called before the first frame update
    void Start()
    {
        spriteLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        sR = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCookieSprite(sR.sprite);
    }

    void UpdateCookieSprite(Sprite sprite)
    {
        _LightCookieSprite.SetValue(spriteLight, sprite);
    }
}
