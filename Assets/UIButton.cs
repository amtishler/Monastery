using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{

    private void OnMouseOver()
    {
        Debug.Log("DING DING DING");
    }

    public void PlayMouseOver()
    {
        UISFXManager.Instance.PlayMenuSFX(UISFX.hoverOver);
    }

    public void PlaySelect()
    {
        UISFXManager.Instance.PlayMenuSFX(UISFX.selectOption);
    }

}
