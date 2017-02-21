using UnityEngine;
using System.Collections;

public class GeneralNPCanim : MonoBehaviour {
    Animator animator;
    bool idle;
    bool walk;
    bool attack;
    bool dead;
    bool dmged;
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
        dmged = this.GetComponent<AggChase>().damaged;
        // Based on current state set this objects animation 
        if (dmged)
        {
            
            animator.speed = defaultspeed;
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDamaged", true);
        }
        else if (walk)
        {
            animator.speed = defaultspeed;
            animator.SetBool("isDamaged", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isWalking", true);
        

        }
        else if (attack)
        {
            float multiplier = GetComponent<Properties>().aspeed_m; //Attack speed multiplier.
            animator.speed = defaultspeed * multiplier;//Attack speed increases rate of animation
            animator.SetBool("isDamaged", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", false);
            animator.SetBool("isAttacking", true);
           

        }else if (dead)
        {
            animator.speed = defaultspeed;
            animator.SetBool("isDamaged", false);
            animator.SetBool("isIdle", false);
            animator.SetBool("isAttacking", false);
            animator.SetBool("isWalking", false);
            animator.SetBool("isDead", true);

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
	void Update () {
        animcontrol();
	}
}
