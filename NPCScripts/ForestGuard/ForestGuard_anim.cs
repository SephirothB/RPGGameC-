using UnityEngine;
using System.Collections;

public class ForestGuard_anim : MonoBehaviour {
    Animator animator;
    bool idle;
    bool walk;
    bool attack;
    bool dead;
    float defaultspeed; //Animation default speed.
    // Use this for initialization
    void Awake()
    {
        defaultspeed = this.GetComponent<Properties>().animspeed;//Default animation speed initializer.
        animator = this.GetComponent<Animator>();//Set animator to this objects animator.

    }
    void animcontrol()
    {
        //Update current state
        idle = this.GetComponent<AggChase>().animidle;
        walk = this.GetComponent<AggChase>().animwalking;
        attack = this.GetComponent<AggChase>().animattacking;
        dead = this.GetComponent<Properties>().dead;
        // Based on current state set this objects animation
        if (dead)
        {
            animator.speed = defaultspeed;
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", true);
        }
        else if (walk)
        {
            animator.speed = defaultspeed;
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isWalking", true);


        }
        else if (attack)
        {
            float multiplier = GetComponent<Properties>().aspeed_m; //Attack speed multiplier.
            animator.speed = defaultspeed * multiplier;//Attack speed increases rate of animation
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isAttacking", true);


        }
        else//set Idle
        {
            animator.speed = defaultspeed;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isIdle", true);

        }
    }
    // Update is called once per frame
    void Update()
    {
        animcontrol();
    }
}
