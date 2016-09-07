using UnityEngine;
using System.Collections;

public class Otto_Controller : MonoBehaviour
{ 
	private Animator anim;
	private bool Extra_Idle_Bool = false;

	void Start () 
	{
		anim = GetComponent <Animator>();
	}

	void Update () 
	{
		#region Courintine (Bum Scratch)

		if(anim.GetCurrentAnimatorStateInfo(0).IsName("Idle") && !Extra_Idle_Bool)
		{
			ExtraIdleMethod ();
		}

		/*
		if(anim.GetCurrentAnimatorStateInfo(1).IsName("Idle") && anim.GetCurrentAnimatorStateInfo(1).IsName("Bum") )
		{
			StopCoroutine(ExtraIdle());
			Extra_Idle_Bool = false;
		}
		*/
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			anim.SetBool ("Jump", true);
			anim.SetBool ("Idle", false);
			anim.SetBool ("Bum", false);
		}
		
		if (Input.GetKeyUp(KeyCode.Space))
		{
			anim.SetBool ("Jump", false);
		}

		if (Input.GetKeyUp (KeyCode.Space)) {
			anim.SetBool ("Jump", false);
		}
		
		if (Input.GetKey (KeyCode.D)) {
			anim.SetBool ("MoveRight", true);
			anim.SetBool ("Bum", false);
		}
		if (Input.GetKeyUp (KeyCode.D)) {
			anim.SetBool ("MoveRight", false);
		}
		if (Input.GetKey (KeyCode.A)) {
			anim.SetBool ("MoveRight", true);
			anim.SetBool ("Bum", false);
		}
		if (Input.GetKeyUp (KeyCode.A)) {
			anim.SetBool ("MoveRight", false);

			if (!Extra_Idle_Bool) {
				anim.SetBool ("Idle", false);
				anim.SetBool ("Turn", true);
				StartCoroutine(ExtraIdle());
				
			} else if (!Extra_Idle_Bool) {
				Extra_Idle_Bool = false;
				StartCoroutine(ExtraIdle());
			}

		if (Input.GetKeyUp (KeyCode.A)) {
			anim.SetBool ("Turn", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("Idle", true);
			}
		}
	}
	
	IEnumerator ExtraIdle()
	{
		yield return new WaitForSeconds (10);
		
		anim.SetBool ("Bum", true);
		anim.SetBool ("Idle", false);
		
		yield return new WaitForSeconds (1.6f);

		Extra_Idle_Bool = false;

		anim.SetBool ("Bum", false);
		anim.SetBool ("Idle", true);
	}
	
	void ExtraIdleMethod()
	{
		Extra_Idle_Bool = true;
		anim.SetBool ("Idle", false);
		StartCoroutine (ExtraIdle());
	}
        #endregion

        #region (Animation Triggers)

    void OnTriggerEnter(Collider col)
    { 
		if (col.gameObject.tag == "Vine Slide")
		{
			anim.SetBool ("Swing", true);
			anim.SetBool ("Jump", false);
			anim.SetBool ("MoveRight", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("Turn", false);
			anim.SetBool ("Idle", false);
			anim.SetBool ("Bum", false);
			anim.SetBool ("Fall", false);
			anim.SetBool ("Slide", false);
			anim.SetBool ("Land", false);

			Debug.Log("Col1 Works");
		}

		else if (col.gameObject.tag == "Vine End Point")
		{
			anim.SetBool ("Swing", false);
			anim.SetBool ("Jump", false);
			anim.SetBool ("MoveRight", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("Turn", false);
			anim.SetBool ("Idle", true);
			anim.SetBool ("Bum", false);
			anim.SetBool ("Fall", false);
			anim.SetBool ("Slide", false);
			anim.SetBool ("Land", false);

			Debug.Log("Col2 Works");
		}

		if (col.gameObject.tag == "Slope Start")
		{
			anim.SetBool ("Swing", false);
			anim.SetBool ("Jump", false);
			anim.SetBool ("MoveRight", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("Turn", false);
			anim.SetBool ("Idle", false);
			anim.SetBool ("Bum", false);
			anim.SetBool ("Fall", false);
			anim.SetBool ("Slide", true);
			anim.SetBool ("Land", false);
			Debug.Log("Col3 Works");
		}

		else if (col.gameObject.tag == "Slope Stop")
		{
			anim.SetBool ("Swing", false);
			anim.SetBool ("Jump", false);
			anim.SetBool ("MoveRight", false);
			anim.SetBool ("MoveLeft", false);
			anim.SetBool ("Turn", false);
			anim.SetBool ("Idle", true);
			anim.SetBool ("Bum", false);
			anim.SetBool ("Fall", false);
			anim.SetBool ("Slide", false);
			anim.SetBool ("Land", false);
			Debug.Log("Col4 Works");
		}
	#endregion
	}
}
