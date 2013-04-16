using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
public class PlayerSpriteRenderer : MonoBehaviour 
{
	OTAnimatingSprite sprite;
	PlayerController controller;
	PlayerController.PlayerState priorState;
	
	[System.Serializable]
	public class AnimationMappings
	{
		public string IdleNW 	= "NWIdle";
		public string IdleN 	= "NIdle";
		public string IdleNE 	= "NEIdle";
		public string IdleW 	= "WIdle";
		public string IdleE 	= "EIdle";
		public string IdleSW 	= "SWIdle";
		public string IdleS 	= "SIdle";
		public string IdleSE	= "SEIdle";

		public string MovingNW 	= "NWWalk";
		public string MovingN 	= "NWalk";
		public string MovingNE 	= "NEWalk";
		public string MovingW 	= "WWalk";
		public string MovingE 	= "EWalk";
		public string MovingSW 	= "SWWalk";
		public string MovingS 	= "SWalk";
		public string MovingSE	= "SEWalk";
		// public string Attack	= "Attack";
		// public string Attack2	= "Attack2";
	}
	public AnimationMappings animationMappings;
	
	public Transform playerModel;
    public Transform playerHand;
    private bool runOnce = true;
	
	// Use this for initialization
	void Start () 
	{
		//sprite = GetComponentInChildren<OTAnimatingSprite>();
		/*if (null == sprite)
		{
			Debug.LogError("PlayerSpriteRenderer could not find OTAnimatingSprite");
		}*/
		controller = GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// Debug.Log(controller.state);	
		//int angle = (int) (Mathf.Sign(transform.forward.x) 
				//* Mathf.Round(Vector3.Angle(Vector3.forward, transform.forward) / 45f));

        if (PlayerController.PlayerState.Dying == controller.state)
        {
            if (runOnce)
            {
                playerModel.animation.CrossFade("diehard");
                runOnce = false;
            }
        }
        else if (PlayerController.PlayerState.Moving == controller.state)
        {
            playerModel.animation.CrossFade("walk");
        }
        else if (PlayerController.PlayerState.Attacking == controller.state)
        {
            if (!playerModel.animation.IsPlaying("attack"))
                playerModel.animation.CrossFade("attack");
        }
        else if (PlayerController.PlayerState.Blocking == controller.state)
        {
            if (!playerModel.animation.IsPlaying("resist"))
                playerModel.animation.CrossFade("resist");
        }
        else if (PlayerController.PlayerState.Idle == controller.state)
        {
            if (!playerModel.animation.IsPlaying("idle"))
                playerModel.animation.CrossFade("idle");
        }
	}
}
