using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;
using Debug = System.Diagnostics.Debug;

namespace Script
{
    public sealed class InternalPlayerController : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int AttackTrigger = Animator.StringToHash("attack");
        private bool _canJump = true;
        private static readonly int JumpTrigger = Animator.StringToHash("jump");
        private SpriteRenderer _spriteRenderer;
        private static readonly int RunBool = Animator.StringToHash("run");
        private bool _prevAttackAnimationEnding;

        [SerializeField] private GameObject projectile;
        [SerializeField] private Transform projectileLocation;
        [SerializeField] private float projectileVelocity = 0f;
        [SerializeField] private float attackTimePoint = 0.2f;

        // Start is called before the first frame update
        private void Start()
        {
            _animator = this.GetComponent<Animator>();
            Debug.Assert(_animator != null, nameof(_animator) + " != null");
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            Debug.Assert(_spriteRenderer != null, nameof(_spriteRenderer) + " != null");
            
            Assert.IsNotNull(projectile);
            Assert.IsNotNull(projectileLocation);
        }
        
        private void Awake()
        {
            _animator = this.GetComponent<Animator>();
            Debug.Assert(_animator != null, nameof(_animator) + " != null");
            _spriteRenderer = this.GetComponent<SpriteRenderer>();
            Debug.Assert(_spriteRenderer != null, nameof(_spriteRenderer) + " != null");
            
            Assert.IsNotNull(projectile);
            Assert.IsNotNull(projectileLocation);
        }
        
        private void Update()
        {
            var animationState = _animator.GetCurrentAnimatorStateInfo(0);

            var attackAnimationEnding = animationState.IsName("atk") && (animationState.normalizedTime > attackTimePoint);
            var attackJustConnected = !_prevAttackAnimationEnding && attackAnimationEnding;
            
            if (attackJustConnected)
            {
                var projectileInstance = Instantiate(projectile, projectileLocation.position, Quaternion.identity);
                Debug.Assert(projectileInstance, nameof(projectileInstance) + " != null");
                var projectileScript = projectileInstance.GetComponent<ProjectileScript>();
                Debug.Assert(projectileScript, nameof(projectileScript) + " != null");
                var speed = Vector2.right * projectileVelocity;
                var velocity = (_spriteRenderer.flipX ? -1 : 1) * speed;
                projectileScript.SetVelocity(velocity);
            }

            _prevAttackAnimationEnding = attackAnimationEnding;
        }

        internal void SetRunning()
        {
            _animator.SetBool(RunBool, true);
        }

        internal void ResetRunning()
        {
            Debug.Assert(_animator, nameof(_animator) + " != null");
            // print(nameof(RunBool) +" "+RunBool);
            _animator.SetBool(RunBool, false);
        }

        internal void Attack()
        {
            _animator.SetTrigger(AttackTrigger);
        }

        internal bool Jump()
        {
            if (!_canJump)
            {
                return false;
            }
            
            _canJump = false;
            _animator.SetTrigger(JumpTrigger);

            return true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            _canJump = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _canJump = true;
        }

        internal void FaceRight()
        {
            _spriteRenderer.flipX = false;
            // projectileLocation.localPosition.x = 0 - projectileLocation.localPosition.x;
            var localPosition = projectileLocation.localPosition;
            var vector3 = localPosition;
            vector3.x = 0 - localPosition.x;
            localPosition = vector3;
            projectileLocation.localPosition = localPosition;
        }

        internal void FaceLeft()
        {
            _spriteRenderer.flipX = true;
            // projectileLocation.localPosition.x = 0 - projectileLocation.localPosition.x;
            var localPosition = projectileLocation.localPosition;
            var vector3 = localPosition;
            vector3.x = 0 - localPosition.x;
            localPosition = vector3;
            projectileLocation.localPosition = localPosition;
        }
    }
}
