using System;
using MBT;
using UnityEngine;

namespace BT
{
    [AddComponentMenu("")]
    [MBTNode(name = "Hero/Roll")]
    public class Roll : Leaf
    {
        private float moveSpeed = 7f;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rigidbody2D;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public override NodeResult Execute()
        {
            var inputX = Input.GetAxis("Horizontal");
            _rigidbody2D.velocity = new Vector2( inputX * moveSpeed, _rigidbody2D.velocity.y);
            
            if(inputX == 0)
                switch (_spriteRenderer.flipX)
                {
                    case true:
                        _rigidbody2D.velocity = new Vector2(-moveSpeed, _rigidbody2D.velocity.y);
                        break;
                    case false:
                        _rigidbody2D.velocity = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
                        break;
                }
                //_rigidbody2D.velocity = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
            
            _animator.Play("Roll");
            return NodeResult.success;
        }
    }
}