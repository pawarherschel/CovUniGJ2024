using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Script
{
    public sealed class InternalBossController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private static readonly int RunningBool = Animator.StringToHash("chase");

        internal bool Attack()
        {
            var animationState = _animator.GetCurrentAnimatorStateInfo(0);

            if (!animationState.IsName("idle")) return false;
            
            _animator.SetTrigger(AttackTrigger);
            return true;
        }
        
        internal bool Chase()
        {
            var animationState = _animator.GetCurrentAnimatorStateInfo(0);

            if (!animationState.IsName("idle")) return false;
            
            _animator.SetBool(RunningBool, true);
            return true;

        }

        internal void StopChase()
        {
            _animator.SetBool(RunningBool, false);
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            _animator = this.GetComponent<Animator>();
            Debug.Assert(_animator != null, nameof(_animator) + " != null");
        }

        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            Debug.Assert(_animator != null, nameof(_animator) + " != null");
        }

        // Update is called once per frame
        private void Update()
        {
            
        }
    }
}
