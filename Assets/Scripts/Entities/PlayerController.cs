using UnityEngine;
using System.Collections;

[RequireComponent(typeof(GridMotor))]
public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class InputConfig
    {
        public string HorizontalAxis = "Horizontal";
        public string VerticalAxis = "Vertical";
        public string AttackButton = "Fire1";
        public string BlockButton = "Fire2";
    }
    public InputConfig inputConfig = new InputConfig();

    [System.Serializable]
    public class AttackConfig
    {
        public float AttackDuration = 1.5f;
        public float BlockDuration = 1.0f;
    }
    public AttackConfig attackConfig = new AttackConfig();

    [System.Serializable]
    public class RecoilConfig
    {
        public float RecoilDuration = 1.0f;
    }
    public RecoilConfig recoilConfig = new RecoilConfig();

    GridMotor motor;

    public enum PlayerState
    {
        Start, Idle, Moving, Attacking, Blocking, Waiting, Recoiling, Dying
    }

    StateMachine<PlayerState> sm = new StateMachine<PlayerState>();
    public PlayerState state { get { return sm.currentState; } }
    // public event StateChangedEventHandler OnStateChanged
    // {
    // 	get { return sm.OnStateChanged; }
    // 	set { sm.OnStateChanged = value; }
    // }

    void Start()
    {
        motor = GetComponent<GridMotor>();
        sm.RegisterState(PlayerState.Idle, new IdleState(gameObject));
        sm.RegisterState(PlayerState.Moving, new MovingState(gameObject));
        sm.RegisterState(PlayerState.Attacking, new AttackingState(gameObject));
        sm.RegisterState(PlayerState.Blocking, new BlockingState(gameObject));
        sm.RegisterState(PlayerState.Dying, new DyingState(gameObject));

        sm.ChangeState(PlayerState.Start, PlayerState.Idle);
    }

    void Update()
    {
        if (MenuButtons.gamestate == MenuButtons.GameState.Play)
        {
            motor.input.direction = new Vector2(Input.GetAxis(inputConfig.HorizontalAxis), Input.GetAxis(inputConfig.VerticalAxis));
            sm.Update();
        }
    }


    class BasePlayerState : IState<PlayerState>
    {
        protected StateChanger<PlayerState> changeState;
        public StateChanger<PlayerState> ChangeState
        {
            get { return changeState; }
            set { changeState = value; }
        }

        protected GameObject self;
        protected PlayerController controller;
        protected GridMotor motor;

        public BasePlayerState(GameObject self)
        {
            this.self = self;
            controller = self.GetComponent<PlayerController>();
            motor = self.GetComponent<GridMotor>();
        }

        public virtual void OnEnter(PlayerState priorState) { }

        public virtual void Update()
        {
            if (PlayerProps.currentHealth <= 0)
                ChangeState(self.GetComponent<PlayerController>().state, PlayerState.Dying);
        }

        public virtual void OnExit(PlayerState nextState) { }

        public virtual void SendEvent(string name) { }
    }


    class IdleState : BasePlayerState
    {
        public IdleState(GameObject self) : base(self) { }

        public override void Update()
        {
            if (MenuButtons.gamestate == MenuButtons.GameState.Play)
            {
                if (Input.GetButtonDown(controller.inputConfig.HorizontalAxis)
                    || Input.GetButtonDown(controller.inputConfig.VerticalAxis))
                {
                    ChangeState(PlayerState.Idle, PlayerState.Moving);
                }
                else if (Input.GetButtonDown(controller.inputConfig.AttackButton))
                {
                    ChangeState(PlayerState.Idle, PlayerState.Attacking);
                }
                else if (Input.GetButtonDown(controller.inputConfig.BlockButton))
                {
                    ChangeState(PlayerState.Idle, PlayerState.Blocking);
                }

                base.Update();
            }
        }
    }


    class MovingState : BasePlayerState
    {
        public MovingState(GameObject self) : base(self) { }

        public override void OnEnter(PlayerState priorState)
        {
            motor.enabled = true;
        }

        public override void OnExit(PlayerState nextState)
        {
            motor.enabled = false;
        }

        public override void Update()
        {
            if (MenuButtons.gamestate == MenuButtons.GameState.Play)
            {
                if (!motor.isMoving && Input.GetButtonDown(controller.inputConfig.AttackButton))
                {
                    ChangeState(PlayerState.Moving, PlayerState.Attacking);
                }
                else if (!motor.isMoving && Input.GetButtonDown(controller.inputConfig.BlockButton))
                {
                    ChangeState(PlayerState.Moving, PlayerState.Blocking);
                }
                
                if (!motor.isMoving && !Input.GetButton(controller.inputConfig.HorizontalAxis)
                     && !Input.GetButton(controller.inputConfig.VerticalAxis))
                {
                    ChangeState(PlayerState.Moving, PlayerState.Idle);
                }

                base.Update();
            }
        }

        public override void SendEvent(string name) { }
    }


    class BlockingState : BasePlayerState
    {
        float endTime;

        public BlockingState(GameObject self) : base(self) { }

        public override void OnEnter(PlayerState priorState)
        {
            if (Input.GetButtonDown(controller.inputConfig.BlockButton))
            {
                PlayerProps.isBlocking = true;
                endTime = Time.time + controller.attackConfig.BlockDuration;
            }
        }

        public override void Update()
        {
            if (Time.time > endTime)
            {
                ChangeState(PlayerState.Blocking, PlayerState.Idle);
            }

            base.Update();
        }

        public override void OnExit(PlayerState nextState)
        {
            PlayerProps.isBlocking = false;
        }
    }


    class AttackingState : BasePlayerState
    {
        float endTime;

        public AttackingState(GameObject self) : base(self) { }

        public override void OnEnter(PlayerState priorState)
        {
            if (Input.GetButtonDown(controller.inputConfig.AttackButton))
            {
                self.gameObject.GetComponent<PlayerSpriteRenderer>().playerHand.GetComponent<BoxCollider>().enabled = true;
                endTime = Time.time + controller.attackConfig.AttackDuration;
            }
        }

        public override void Update()
        {
            if (Time.time > endTime)
            {
                ChangeState(PlayerState.Attacking, PlayerState.Idle);
            }

            base.Update();
        }
    }


    class DyingState : BasePlayerState
    {
        public DyingState(GameObject self) : base(self) { }

        public override void Update()
        {
            if (!self.gameObject.GetComponent<PlayerSpriteRenderer>().playerModel.animation.IsPlaying("diehard"))
            {
                Application.LoadLevel("Main Menu");
            }
        }
    }
}