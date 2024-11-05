using UnityEngine;

public class SelectionRing : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer baseRing;
    public SpriteRenderer raiseRing;

    public void Select(bool _select)
    {
        if (_select)
            animator.SetTrigger("Select");

        baseRing.enabled = _select;
    }
}
