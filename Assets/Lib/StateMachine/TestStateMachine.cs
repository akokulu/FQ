using UnityEngine;
using System;
using System.Collections;

public class TestStateMachine : MonoBehaviour 
{
	public enum StateOption
	{
		Start,
		Running,
		End
	}

	StateMachine<StateOption> sm = new StateMachine<StateOption>();


	void Start () 
	{
		sm.OnStateChanged += this.OnStateChangedEvent;
		sm.RegisterState(StateOption.Start, new StartState(gameObject));
		sm.RegisterState(StateOption.Running, new RunningState(gameObject));

		sm.ChangeState(StateOption.Start, StateOption.Start);
	}
	
	void Update () 
	{
		sm.Update();
	}

	void OnStateChangedEvent(object sender, EventArgs args)
	{
		Debug.Log("State changed");
	}


	abstract class BaseState : IState<StateOption>
	{
		protected StateChanger<StateOption> changeState;
		public StateChanger<StateOption> ChangeState 
		{ 
			get { return changeState; }
			set { changeState = value; }
		}

		GameObject self;

		public BaseState(GameObject self)
		{
			this.self = self;
		}

		public virtual void OnEnter(StateOption priorState) { }

		public virtual void Update() { }

		public virtual void OnExit(StateOption nextState) { }

		public virtual void SendEvent(string name) { }

	}

	class StartState : BaseState
	{
		public StartState(GameObject self) : base(self)
		{

		}

		float endTime;

		public override void OnEnter(StateOption priorState)
		{
			endTime = Time.time + 5f;
		}

		public override void Update()
		{
			if (Time.time > endTime)
			{
				changeState(StateOption.Start, StateOption.Running);
			}
		}

	}

	class RunningState : BaseState
	{
		public RunningState(GameObject self) : base(self)
		{

		}

		float endTime;

		public override void OnEnter(StateOption priorState)
		{
			endTime = Time.time + 5f;
		}

		public override void Update()
		{
			if (Time.time > endTime)
			{
				changeState(StateOption.Start, StateOption.Running);
			}
		}

	}
}
