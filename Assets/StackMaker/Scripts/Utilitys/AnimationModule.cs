using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AnimationModule : MonoBehaviour
{
    public event Action<string> UpdateEventAnimationState;
    public readonly string PLAYER_ANIM_STATE = "State";
    [SerializeField]
    private Animator Anim;
    public void Activate(string AnimBoolName)
    {
        if (AnimBoolName != null)
            Anim.SetBool(AnimBoolName, true);
    }

    public void Deactivate(string AnimBoolName)
    {
        if (AnimBoolName != null)
            Anim.SetBool(AnimBoolName, false);
    }
    public void ExitAnimator()
    {
        Anim.Rebind();
        Anim.Update(0f);
    }
    public void SetState(string name, int state)
    {
        ExitAnimator();
        Anim.SetInteger(name, state);      
    }

    public void SetActive(bool p)
    {
        Anim.enabled = p;
    }

    
    public void CallEvent(string code)
    {
        UpdateEventAnimationState.Invoke(code);
    }
}
