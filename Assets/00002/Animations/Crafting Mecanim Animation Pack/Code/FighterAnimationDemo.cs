using UnityEngine;
using System.Collections;

public class FighterAnimationDemo : MonoBehaviour {

	private bool crouchBool = false;
	private bool blockBool = false;
	private bool dead = false;
	private bool InAir = false;
	public Animator animator;

	private Transform defaultCamTransform;
	private Vector3 resetPos;
	private Quaternion resetRot;
	private GameObject cam;
	private GameObject fighter;

	void Start()
	{
		cam = GameObject.FindWithTag("MainCamera");
		defaultCamTransform = cam.transform;
		resetPos = defaultCamTransform.position;
		resetRot = defaultCamTransform.rotation;
		fighter = GameObject.FindWithTag("Player");
		fighter.transform.position = new Vector3(0,0,0);
	}

	IEnumerator COInAir(float toAnimWindow)
	{
		yield return new WaitForSeconds(toAnimWindow);
		InAir = true;
		animator.SetBool("InAir", true);
		yield return new WaitForSeconds(0.5f);
		InAir = false;
		animator.SetBool("InAir", false);
	}

	void OnGUI () 
	{
		if (GUI.RepeatButton (new Rect (815, 535, 100, 30), "Reset Scene")) 
		{
			defaultCamTransform.position = resetPos;
			defaultCamTransform.rotation = resetRot;
			fighter.transform.position = new Vector3(0,0,0);
			animator.Play("Idle");
		}

		if(!dead)
		{
			if(!blockBool && !crouchBool)
			{
				if (GUI.RepeatButton (new Rect (565, 20, 100, 30), "Run")) 
				{
					animator.SetBool("Run", true);
				}
				else
				{
					animator.SetBool("Run", false);
				}

				if (GUI.RepeatButton(new Rect (25, 20, 100, 30), "Walk Forward")) 
					animator.SetBool("Walk Forward", true);
				else
					animator.SetBool("Walk Forward", false);

				if (GUI.Button(new Rect (245, 20, 100, 30), "Dash Forward")) 
					animator.SetTrigger("DashForwardTrigger");

				if (GUI.Button(new Rect (245, 90, 100, 30), "Dash Left")) 
					animator.SetTrigger("DashLeftTrigger");

				if (GUI.Button(new Rect (245, 120, 100, 30), "Dash Right")) 
					animator.SetTrigger("DashRightTrigger");

				if (GUI.RepeatButton(new Rect (135, 20, 100, 30), "Walk Left")) 
					animator.SetBool("WalkLeft", true);
				else
					animator.SetBool("WalkLeft", false);

				if (GUI.RepeatButton(new Rect (135, 50, 100, 30), "Walk Right")) 
					animator.SetBool("WalkRight", true);
				else
					animator.SetBool("WalkRight", false);

				if (GUI.RepeatButton(new Rect (135, 90, 100, 30), "Walk Slow")) 
					animator.SetBool("WalkSlow", true);
				else
					animator.SetBool("WalkSlow", false);

				if (GUI.Button (new Rect (355, 20, 100, 30), "Intro1")) 
				{
					animator.SetTrigger("Intro1Trigger");
				}

				if (GUI.Button (new Rect (355, 50, 100, 30), "Intro2")) 
				{
					animator.SetTrigger("Intro2Trigger");
				}

				if (GUI.Button (new Rect (455, 20, 100, 30), "Victory1")) 
				{
					animator.SetTrigger("Victory1Trigger");
				}

				if (GUI.RepeatButton (new Rect (25, 50, 100, 30), "Walk Backward")) 
				{
					animator.SetBool("Walk Backward", true);
				}
				else
				{
					animator.SetBool("Walk Backward", false);
				}

				if (GUI.Button (new Rect (245, 50, 100, 30), "Dash Backward")) 
				{
					animator.SetTrigger("DashBackwardTrigger");
				}

				if (GUI.Button (new Rect (355, 90, 100, 30), "Roll Forward")) 
				{
					animator.SetTrigger("RollForwardTrigger");
				}

				if (GUI.Button (new Rect (355, 120, 100, 30), "Roll Backard")) 
				{
					animator.SetTrigger("RollBackwardTrigger");
				}

				if (GUI.Button (new Rect (455, 50, 100, 30), "Victory2")) 
				{
					animator.SetTrigger("Victory2Trigger");
				}

				if (GUI.Button (new Rect (25, 90, 100, 30), "Uppercut")) 
				{
					animator.SetTrigger("UppercutTrigger");
				}

				if (GUI.Button (new Rect (25, 120, 100, 30), "Punch")) 
				{
					animator.SetTrigger("PunchTrigger");
				}
        
		        if (GUI.Button (new Rect (135, 120, 100, 30), "Heavy Smash")) 
		        {
		          animator.SetTrigger("HeavySmashTrigger");
		        }

				if (GUI.Button (new Rect (135, 150, 100, 30), "Smash Combo")) 
				{
					animator.SetTrigger("SmashComboTrigger");
				}

				if (GUI.Button (new Rect (245, 150, 100, 30), "Combo1")) 
				{
					animator.SetTrigger("Combo1Trigger");
				}

				if (GUI.Button (new Rect (355, 150, 100, 30), "Forward Smash")) 
				{
					animator.SetTrigger("ForwardSmashTrigger");
				}

				if (GUI.Button (new Rect (25, 150, 100, 30), "Jab")) 
				{
					animator.SetTrigger("JabTrigger");
				}

				if (GUI.Button (new Rect (25, 180, 100, 30), "Kick")) 
				{
					animator.SetTrigger("KickTrigger");
				}

				if (GUI.Button (new Rect (135, 180, 100, 30), "AxeKick")) 
				{
					animator.SetTrigger("AxeKickTrigger");
				}
			}

			blockBool = GUI.Toggle (new Rect (25, 215, 100, 30), blockBool, "Block");
			
			if (blockBool)
				animator.SetBool("Block", true);
			else
				animator.SetBool("Block", false);

			crouchBool = GUI.Toggle (new Rect (140, 215, 100, 30), crouchBool, "Crouch");

			if (crouchBool)
				animator.SetBool("Crouch", true);
			else
				animator.SetBool("Crouch", false);


			if(blockBool)
			{
				if(!crouchBool)
				{
					if (GUI.Button (new Rect (30, 240, 100, 30), "BlockHitReact")) 
					{
						animator.SetTrigger("BlockHitReactTrigger");
					}
          if (GUI.Button (new Rect (30, 270, 100, 30), "BlockBreak")) 
          {
            animator.SetTrigger("BlockBreakTrigger");
          }
				}
				else
				{
					if (GUI.Button (new Rect (30, 240, 100, 30), "BlockHitReact")) 
					{
						animator.SetTrigger("CrouchBlockHitReactTrigger");
					}
				}
			}
			else
			{
				if (GUI.Button (new Rect (30, 240, 100, 30), "Hit React")) 
				{
					animator.SetTrigger("LightHitTrigger");
				}
			}

			if(!blockBool)
			{
				if (GUI.Button (new Rect (30, 270, 100, 30), "Knockdown")) 
				{
					animator.SetTrigger("KnockdownTrigger");
				}

				if (GUI.Button (new Rect (30, 300, 100, 30), "Choke")) 
				{
					animator.SetTrigger("Choke");
				}
			}

			if (crouchBool)
			{
				if (GUI.Button (new Rect (135, 300, 100, 30), "Low Kick") && crouchBool) 
				{
					animator.SetTrigger("LowKickTrigger");
				}

				if (GUI.Button (new Rect (135, 240, 100, 30), "Sweep") && crouchBool) 
				{
					animator.SetTrigger("SweepTrigger");
				}

				if (GUI.Button (new Rect (245, 240, 100, 30), "DownSmash") && crouchBool) 
				{
					animator.SetTrigger("DownSmashTrigger");
				}

				if (GUI.Button (new Rect (135, 270, 100, 30), "Low Punch")) 
				{
					animator.SetTrigger("LowPunchTrigger");
				}
			}

			if(!blockBool && !crouchBool)
			{
				if (GUI.Button (new Rect (30, 340, 100, 30), "Jump")) 
				{
					animator.SetTrigger("JumpTrigger");
					StartCoroutine (COInAir(0.25f));
				}

				if (GUI.Button (new Rect (30, 370, 100, 30), "Jump Forward")) 
				{
					animator.SetTrigger("JumpForwardTrigger");
					StartCoroutine (COInAir(0.25f));
				}

				if (InAir)
				{
					if (GUI.Button (new Rect (135, 370, 100, 30), "Jump Punch") && InAir) 
					{
						animator.SetTrigger("HighPunchTrigger");
					}

					if (GUI.Button (new Rect (245, 370, 100, 30), "Jump Smash") && InAir) 
					{
						animator.SetTrigger("HighSmashTrigger");
					}

					if (GUI.Button (new Rect (135, 340, 100, 30), "Jump Hit React") && InAir) 
					{
						animator.SetTrigger("JumpHitReactTrigger");
					}

					if (GUI.Button (new Rect (135, 400, 100, 30), "Jump Kick") && InAir) 
					{
						animator.SetTrigger("HighKickTrigger");
					}
				}

				if (GUI.Button (new Rect (30, 400, 100, 30), "Jump Backward")) 
				{
					animator.SetTrigger("JumpBackwardTrigger");
					StartCoroutine (COInAir(0.25f));
				}

				if (GUI.Button (new Rect (30, 440, 100, 30), "RangeAttack1")) 
				{
					animator.SetTrigger("RangeAttack1Trigger");
				}

				if (GUI.Button (new Rect (135, 440, 100, 30), "RangeAttack2")) 
				{
					animator.SetTrigger("RangeAttack2Trigger");
				}

				if (GUI.Button (new Rect (30, 470, 100, 30), "MoveAttack1")) 
				{
					animator.SetTrigger("MoveAttack1Trigger");
				}

				if (GUI.Button (new Rect (135, 470, 100, 30), "MoveAttack2")) 
				{
					animator.SetTrigger("MoveAttack2Trigger");
				}

				if (GUI.Button (new Rect (30, 500, 100, 30), "SpecialAttack1")) 
				{
					animator.SetTrigger("SpecialAttack1Trigger");
				}

				if (GUI.Button (new Rect (135, 500, 100, 30), "SpecialAttack2")) 
				{
					animator.SetTrigger("SpecialAttack2Trigger");
				}

				if (GUI.Button (new Rect (30, 540, 100, 30), "Death")) 
				{
					animator.SetTrigger("DeathTrigger");
					dead = true;
				}
			}
		}
		if (dead)
		{
			if (GUI.Button (new Rect (135, 540, 100, 30), "Revive")) 
			{
				animator.SetTrigger("ReviveTrigger");
				dead = false;
			}
		}
	}
}