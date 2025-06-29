using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBarAnimator : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    private const string IS_FLASHING = "IsFlashing";
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
        animator.SetBool(IS_FLASHING, false);
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burnShowProgressAmount = 0.5f;
        bool show = e.progressNormalized >= burnShowProgressAmount && stoveCounter.IsFried();
        if (show)
        {
            animator.SetBool(IS_FLASHING, true);
        }
        else
        {
            animator.SetBool(IS_FLASHING, false);
        }
    }
}
