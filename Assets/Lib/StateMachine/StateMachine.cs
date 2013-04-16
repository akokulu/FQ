using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


public delegate void StateChanger<T>(T priorState, T nextState);

public interface IState<T>
{
	StateChanger<T> ChangeState { get; set; }
	void OnEnter(T priorState);
	void Update();
	void OnExit(T nextState);
	void SendEvent(string name);
}

public delegate void StateChangedEventHandler(object sender, EventArgs e);

public class StateChangedEventArgs<T> : EventArgs
{
	public T priorState, nextState;
	public StateChangedEventArgs(T priorState, T nextState)
	{
		this.priorState = priorState;
		this.nextState = nextState;
	}
}

public class StateMachine<T>
{
	Dictionary<T, IState<T>> states = new Dictionary<T, IState<T>>();

	T _currentState;
	public T currentState
	{
		get { return _currentState; }
	}

	public event StateChangedEventHandler OnStateChanged;

	public void ChangeState(T priorState, T nextState)
	{
		if (states.ContainsKey(priorState))
		{
			states[priorState].OnExit(nextState);
		}
		_currentState = nextState;
		if (states.ContainsKey(_currentState))
		{
			states[_currentState].OnEnter(priorState);
		}
		
		if (null != OnStateChanged)
		{
			OnStateChanged(this, new StateChangedEventArgs<T>(priorState, nextState));
		}
	}

	public void RegisterState(T stateName, IState<T> state)
	{
		states[stateName] = state;
		state.ChangeState = ChangeState;
	}

	public void Update()
	{
		states[_currentState].Update();
	}

}