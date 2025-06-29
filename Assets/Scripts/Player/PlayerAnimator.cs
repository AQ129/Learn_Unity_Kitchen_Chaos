using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool(Is_Walking, player.IsWalking());
    }

    private const string Is_Walking = "IsWalking";
    [SerializeField] private NewBehaviourScript player;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
}
