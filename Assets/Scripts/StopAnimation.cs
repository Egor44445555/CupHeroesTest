using UnityEngine;

public class StopAnimation : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void StopAnimate(string name)
    {
        anim.SetBool(name, false);
    }
}
