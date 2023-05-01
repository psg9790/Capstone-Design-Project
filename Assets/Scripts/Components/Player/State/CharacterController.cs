using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using UnityEditor.Rendering;

namespace CharacterController
{
    
    public enum StateName
    {
        Idle,
        move,
        dash,
        attack,
    }
    public class StateMachine
    {
        public BaseState CurrentState { get; private set; }
        private Dictionary<StateName, BaseState> states = new Dictionary<StateName, BaseState>();

        public StateMachine(StateName stateName, BaseState state)
        {
            AddState(stateName, state);
            CurrentState = GetState(stateName);
        }

        public void AddState(StateName stateName, BaseState state)
        {
            if (!states.ContainsKey(stateName))
            {
                states.Add(stateName, state);
            }
        }

        public BaseState GetState(StateName stateName)
        {
            if (states.TryGetValue(stateName, out BaseState state))
            {
                return state;
            }

            return null;
        }

        public void DeleteState(StateName removeStateName)
        {
            if (states.ContainsKey(removeStateName))
            {
                states.Remove(removeStateName);
            }
        }

        public void ChangeState(StateName nextStateName)
        {
            CurrentState?.OnExitState();
            if (states.TryGetValue(nextStateName, out BaseState newState))
            {
                CurrentState = newState;
            }
            CurrentState?.OnEnterState();
        }

        public void UpdateState()
        {
            CurrentState?.OnUpdateState();
        }

        public void FixedUpdateState()
        {
            CurrentState?.OnFixedUpdateState();
        }
        
    }
    
    public abstract class BaseState
    {
        protected PlayerController Controller { get; private set; }

        public BaseState(PlayerController controller)
        {
            this.Controller = controller;
        }

        public abstract void OnEnterState();
        public abstract void OnUpdateState();
        public abstract void OnFixedUpdateState();
        public abstract void OnExitState();
    }

    public class IdleState : BaseState
    {
        public IdleState(PlayerController controller) : base(controller)
        {
            
        }
        public override void OnEnterState()
        {
            UnityEngine.Debug.Log("Idle enter");
        }

        public override void OnUpdateState()
        {
            // UnityEngine.Debug.Log("Idle");
        }

        public override void OnFixedUpdateState()
        {
            
        }

        public override void OnExitState()
        {
            
        }
    }
    
    
    

}