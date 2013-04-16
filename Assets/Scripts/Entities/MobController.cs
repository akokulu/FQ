using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GridMotor))]
public class MobController : MonoBehaviour 
{
    public Transform enemyWeapon;
    public Transform enemyShield;

    public Vector3 lastKnownPos;     // Holds the last known position of the player
    public Vector2 lastKnownDir;     // Holds the last known direction of the player

	[System.Serializable]
	public class AttackConfig
	{
		public float AttackDuration = 1.0f;
		public float Attack2Duration = 2.0f;
	}
	public AttackConfig attackConfig = new AttackConfig();
	
	[System.Serializable]
	public class RecoilConfig
	{
		public float RecoilDuration = 1.0f;
	}
	public RecoilConfig recoilConfig = new RecoilConfig();

	public enum MobState
	{
		Start, Idle, Wandering, Waiting, Chasing, Searching, Attacking, Recoiling, Dying
	}

	StateMachine<MobState> sm = new StateMachine<MobState>();
	public MobState state { get { return sm.currentState; } }

	void Start () 
	{
		sm.RegisterState(MobState.Idle, new IdleState(gameObject));
		sm.RegisterState(MobState.Wandering, new WanderingState(gameObject));
		sm.RegisterState(MobState.Chasing, new ChasingState(gameObject));
        sm.RegisterState(MobState.Searching, new SearchingState(gameObject));
		sm.RegisterState(MobState.Attacking, new AttackingState(gameObject));
		sm.RegisterState(MobState.Recoiling, new RecoilingState(gameObject));
        sm.RegisterState(MobState.Dying, new DyingState(gameObject));

		sm.ChangeState(MobState.Start, MobState.Wandering);
	}
	
	void Update () 
	{
		sm.Update();
	}

	class BaseMobState : IState<MobState>
	{
		protected StateChanger<MobState> changeState;
		public StateChanger<MobState> ChangeState 
		{ 
			get { return changeState; }
			set { changeState = value; }
		}

		protected GameObject self;
		protected PlayerController  controller;
		protected GridMotor motor;
        protected GameObject player;

		public BaseMobState(GameObject self)
		{
			this.self = self;
			controller = self.GetComponent<PlayerController>();
			motor = self.GetComponent<GridMotor>();
            player = GameObject.Find("Player");
		}

		public virtual void OnEnter(MobState priorState) { }

        public virtual void Update()
        {
            if (self.GetComponent<EnemyProps>().currentHealth <= 0)
                ChangeState(self.GetComponent<MobController>().state, MobState.Dying);
        }

        public bool CanSeePlayer()
        {
            RaycastHit sight;
            Vector3 enemyHead = new Vector3(self.transform.position.x, self.transform.position.y + self.transform.localScale.y, self.transform.position.z);

            Vector3 rayDirection = player.transform.position - self.transform.position;

            if (PlayerIsBehind())
                return true;

            if ((Vector3.Angle(rayDirection, self.transform.forward)) < EnemyProps.fieldOfViewRange) // Detect if player is within the field of view
            {
                if (Physics.Raycast(enemyHead, rayDirection, out sight, EnemyProps.viewDistance))
                {
                    if (sight.transform.tag == "Player")
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

        private bool PlayerIsBehind()
        {
            RaycastHit sight;
            Vector3 enemyHead = new Vector3(self.transform.position.x, self.transform.position.y + self.transform.localScale.y, self.transform.position.z);

            Vector3 rayDirection = player.transform.position - self.transform.position;

            float distanceToPlayer = Vector3.Distance(self.transform.position, player.transform.position);

            if (Physics.Raycast(enemyHead, rayDirection, out sight)) // If the player is very close behind the enemy and not in view the enemy will detect the player
            {
                Debug.DrawRay(enemyHead, rayDirection, Color.magenta);

                if (sight.transform.tag != "Player")
                    return false;
                else if ((sight.transform.tag == "Player") && (distanceToPlayer <= EnemyProps.minDetectDistance))
                    return true;
            }

            return false;
        }

        public bool CanHitPlayer()
        {
            RaycastHit sight;
            Vector3 rayDirection = player.transform.position - self.transform.position;

            if ((Vector3.Angle(rayDirection, self.transform.forward)) < EnemyProps.fieldOfViewAttack) // Detect if player is within the field of view
            {
                if (Physics.Raycast(self.transform.position, rayDirection, out sight, EnemyProps.attackDistance))
                {
                    if (sight.transform.tag == "Player")
                        return true;
                    else
                        return false;
                }
            }

            return false;
        }

		public virtual void OnExit(MobState nextState) { }
		
		public virtual void SendEvent(string name) { }
	}
	

	//Should never get to an idle state.
	class IdleState : BaseMobState
	{
		public IdleState(GameObject self) : base(self) { }

		public override void OnEnter(MobState priorState)
		{
			motor.enabled = true;
		}

		public override void OnExit(MobState nextState)
		{
			motor.enabled = false;
		}
	}


	class WanderingState : BaseMobState
	{
		public WanderingState(GameObject self) : base(self) {}

		public override void OnEnter(MobState priorState)
		{
			motor.enabled = true;
		}

		public override void OnExit(MobState nextState)
		{
			motor.enabled = false;
		}

		public override void Update() 
		{
			if (CanHitPlayer())
                ChangeState(MobState.Wandering, MobState.Attacking);
            else if (CanSeePlayer())
                ChangeState(MobState.Wandering, MobState.Chasing);

			motor.input.direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
			
			if (!self.animation.IsPlaying("run"))
				self.animation.CrossFade("run");

            base.Update();      // Check if dead
		}
	}


	class ChasingState : BaseMobState
	{
		public ChasingState(GameObject self) : base(self) {}

		public override void OnEnter(MobState priorState)
		{
            //print("Chasing");
			motor.enabled = true;  //<-------- CHANGE THIS BACK WHEN ENEMY IS ABLE TO FIND THE PLAYER positon
            motor.input.isRunning = true;
		}

		public override void OnExit(MobState nextState)
		{
            //print("Stop");
			motor.enabled = false;
            motor.input.isRunning = false;
		}

		public override void Update() 
		{
            if (!self.animation.IsPlaying("run"))
                self.animation.CrossFade("run");

			// Moves the enemy towards the player
            Vector3 rayDirection = player.transform.position - self.transform.position;
            motor.input.direction = new Vector2(rayDirection.x, rayDirection.z);

            if (!CanSeePlayer())
                ChangeState(MobState.Chasing, MobState.Searching);
            else if (CanHitPlayer() && Vector3.Distance(player.transform.position, self.transform.position) <= EnemyProps.attackDistance)
                ChangeState(MobState.Chasing, MobState.Attacking);

            base.Update();      // Check if dead
            
            self.GetComponent<MobController>().lastKnownPos = player.transform.position;   // Keep track of the last known position of the player
            self.GetComponent<MobController>().lastKnownDir = player.GetComponent<GridMotor>().input.direction;   // Keep track of the last known position of the player
		}

		public override void SendEvent(string name) { }
	}


    class SearchingState : BaseMobState
    {
        float searchingTime;
        bool reachedPlayerPos;

        float delay = .1f;
        Vector3 lastPos;

        public SearchingState(GameObject self) : base(self) { }

		public override void OnEnter(MobState priorState)
		{
            lastPos = self.transform.position;
            searchingTime = Time.time + EnemyProps.searchTime;

            //print("Searching");
			motor.enabled = true;
            motor.input.isRunning = true;
		}

		public override void OnExit(MobState nextState)
		{
			motor.enabled = false;
            motor.input.isRunning = false;
		}

        public override void Update()
        {
            // Animate movement
            if (!self.animation.IsPlaying("run"))
                self.animation.CrossFade("run");

            if (CanSeePlayer())
                ChangeState(MobState.Searching, MobState.Chasing);
            else if (Time.time >= searchingTime)        // Currently not really being used because enemy cannot navigate through a maze
                ChangeState(MobState.Searching, MobState.Wandering);

            if (CanHitPlayer() && Vector3.Distance(player.transform.position, self.transform.position) <= EnemyProps.attackDistance)
                ChangeState(MobState.Chasing, MobState.Attacking);

            // Move enemy to player's last known position
            if (!reachedPlayerPos)
            {
                Vector3 rayDirection = self.GetComponent<MobController>().lastKnownPos - self.transform.position;
                motor.input.direction = new Vector2(rayDirection.x, rayDirection.z);
            }

            // when enemy reaches player's last known position, move in player's last known direction
            if (Vector3.Distance(self.transform.position, self.GetComponent<MobController>().lastKnownPos) < .5)
            {
                reachedPlayerPos = true;
                motor.input.direction = self.GetComponent<MobController>().lastKnownDir;
            }

            // Checks if player is moving
            delay -= Time.deltaTime;
            if (delay < 0)
            {
                delay = .1f;
                if (self.transform.position == lastPos)
                    // enemy is stuck on a tree looking for player
                    ChangeState(MobState.Chasing, MobState.Wandering);

                lastPos = self.transform.position;
            }

            base.Update();      // Check if dead
        }

		public override void SendEvent(string name) { }
    }


	class AttackingState : BaseMobState
	{
		float endTime;

		public AttackingState(GameObject self) : base(self) { }

		public override void OnEnter(MobState priorState) 
		{
			//print("Attacking");
            motor.enabled = false;

			//rand call to decide between attacks needs to go here
			
			//If attack 1
				//endTime = Time.time + self.attackConfig.AttackDuration;
				//Player.Health --;
			
			//else if attack 2
				//endTime = Time.time + controller.attackConfig.Attack2Duration;
				//Player.health -5;
			
			//Temp code...
			endTime = Time.time + 2.0f;
		}

		public override void Update() 
		{
            if (!self.animation.IsPlaying("attack"))
            {
                self.animation.CrossFade("attack");
                self.GetComponent<MobController>().enemyWeapon.GetComponent<BoxCollider>().enabled = true;
            }

            self.transform.LookAt(player.transform);

			if (Time.time > endTime)
			{
				ChangeState(MobState.Attacking, MobState.Recoiling);
			}

            if (!CanHitPlayer())
                ChangeState(MobState.Attacking, MobState.Searching);
            else if (Vector3.Distance(player.transform.position, self.transform.position) > EnemyProps.attackDistance)
                ChangeState(MobState.Attacking, MobState.Chasing);

            base.Update();      // Check if dead

            self.GetComponent<MobController>().lastKnownPos = player.transform.position;   // Keep track of the last known position of the player
		}
	}

	
	class RecoilingState : BaseMobState
	{
		float endTime;
		
		public RecoilingState(GameObject self) : base(self) { }
		
		public override void OnEnter(MobState priorState)
		{
			//print("Recoil!");

			//if self.health > 0
			//endTime = Time.time + controller.recoilConfig.RecoilDuration;
			//Enemy health - damage done will go here.
			
			//else 
			//Destroy(gameObject);
			
			endTime = Time.time + 2.0f;
		}

		public override void Update()
		{
            if (!self.animation.IsPlaying("gethit"))
            {
                self.animation.CrossFade("gethit");
                self.GetComponent<MobController>().enemyShield.GetComponent<BoxCollider>().enabled = true;
            }

			if (Time.time > endTime)
			{
				ChangeState(MobState.Recoiling, MobState.Attacking);
			}

            if (!CanHitPlayer())
                ChangeState(MobState.Recoiling, MobState.Wandering);
            else if (Vector3.Distance(player.transform.position, self.transform.position) > EnemyProps.attackDistance)
                ChangeState(MobState.Recoiling, MobState.Chasing);

            base.Update();      // Check if dead
		}
	}
	
	//Does not use this state yet.
	class DelayState : BaseMobState
	{
		float endTime;
		float duration;

		public DelayState(float duration, GameObject self) : base(self) 
		{
			this.duration = duration; 
		}

		public override void OnEnter(MobState priorState) 
		{

		}

		public override void Update() 
		{ 
			if (Time.time > endTime)
			{
				ChangeState(MobState.Attacking, MobState.Idle);
			}

            base.Update();      // Check if dead
		}

	}

    class DyingState : BaseMobState
    {
        public DyingState(GameObject self) : base(self) { }

        public override void OnEnter(MobState priorState)
        {
            self.animation.CrossFade("die");    // Play enemy death animation
        }

        public override void Update()
        {
            if (!self.animation.IsPlaying("die"))
            {
                self.gameObject.GetComponent<EnemyProps>().OnEnemyDeath();  // Deal with enemy death before destroying prefab

                Destroy(self.gameObject);
            }
        }
    }
}
