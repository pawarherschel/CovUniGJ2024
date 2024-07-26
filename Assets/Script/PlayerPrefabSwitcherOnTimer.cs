using System;
using UnityEngine;

namespace Script
{
    public sealed class PlayerPrefabSwitcherOnTimer : MonoBehaviour
    {
        
        [SerializeField] private GameObject[] prefabs;

        private int SpriteIndex
        {
            get => _spriteIndex;
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException(nameof(value));
                _spriteIndex = value % prefabs.Length;
                CurrentSprite = prefabs[_spriteIndex];
            }
        }

        private GameObject _currentSprite;
        private int _spriteIndex;

        public GameObject CurrentSprite
        {
            get => _currentSprite;
            set
            {
                Destroy(_currentSprite);
                System.Diagnostics.Debug.Assert(value, nameof(value) + " != null");
                _currentSprite = Instantiate(value, this.transform);
                CurrentInternalController = _currentSprite.GetComponent<InternalPlayerController>();
                // currentAnimator = _currentSprite.GetComponent<Animator>();
            }
        }

        internal InternalPlayerController CurrentInternalController;
        // public Animator currentAnimator;

        private float _changeTimer;

        [SerializeField] private float changeCooldown = 12f;
        
        // Start is called before the first frame update
        private void Start()
        {
            System.Diagnostics.Debug.Assert(prefabs.Length > 1, "prefabs.Length > 1");
            print("prefabs.Length " +prefabs.Length);

            SpriteIndex = 0;
        }

        // Update is called once per frame
        private void Update()
        {
            // Update Start
            _changeTimer += Time.deltaTime;
            
            // Timer Check
            if (_changeTimer >= changeCooldown)
            {
                TimerJustCompleted();
            }
        }

        private void TimerJustCompleted()
        {
            _changeTimer = 0f;

            print("Switching Player");
            SpriteIndex++;
        }
    }
}