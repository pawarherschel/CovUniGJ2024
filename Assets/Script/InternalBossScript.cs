using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Script
{
    public sealed class InternalBossScript : MonoBehaviour
    {
        [SerializeField] private Transform attackPosition;
        [SerializeField] private Transform chasePosition;
        [SerializeField] private float hSpeed;

        private Animator _animator;
        private GameObject _player;

        internal void Attack()
        {
            throw new NotImplementedException();
        }
        
        // Start is called before the first frame update
        private void Start()
        {
            Debug.Assert(attackPosition != null, nameof(attackPosition) + " != null");
            Debug.Assert(chasePosition != null, nameof(chasePosition) + " != null");
            Debug.Assert(hSpeed != 0);
            
            _animator = this.GetComponent<Animator>();
            Debug.Assert(_animator != null, nameof(_animator) + " != null");
        }

        // Update is called once per frame
        private void Update()
        {
        
        }
    }
}
