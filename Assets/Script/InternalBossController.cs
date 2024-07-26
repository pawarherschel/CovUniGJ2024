using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;
using Object = UnityEngine.Object;

namespace Script
{
    public sealed class InternalBossController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private static readonly int RunningBool = Animator.StringToHash("chase");
        private bool _attackedLastFrame = false;
        [SerializeField] private float attackTriggerTime = 0.5f;
        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform projectileLocation;

        internal bool Attack()
        {
            var animationState = _animator.GetCurrentAnimatorStateInfo(0);

            if (!animationState.IsName("idle"))
            {
                _attackedLastFrame = false;
                return false;
            }
            
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

            Debug.Assert(projectile != null, nameof(projectile) + " != null");
            Debug.Assert(projectileLocation != null, nameof(projectileLocation) + " != null");
        }

        private void Awake()
        {
            Start();
        }

        // Update is called once per frame
        private void Update()
        {
            var animationState = _animator.GetCurrentAnimatorStateInfo(0);

            var isAttacking = animationState.IsName("atk");

            if (!isAttacking) return;

            var isAttackAlmostDone = animationState.normalizedTime > attackTriggerTime;
            var shouldAttack = isAttackAlmostDone && !_attackedLastFrame;
            var bossProjectilesAlreadyExist = GameObject.FindGameObjectWithTag("Enemy Projectile");
            
            if (shouldAttack && !bossProjectilesAlreadyExist)
            {
                SpawnProjectile();
            }
        }

        private void SpawnProjectile()
        {
            var instance = Instantiate(this.projectile, this.projectileLocation.position, Quaternion.identity);
            Debug.Assert(instance != null, nameof(instance) + " != null");
            var projectileScript = instance.GetComponent<ProjectileScript>();
            Debug.Assert(projectileScript != null, nameof(projectileScript) + " != null");
            projectileScript.SetVelocity(Vector2.zero);
        }
    }
}
