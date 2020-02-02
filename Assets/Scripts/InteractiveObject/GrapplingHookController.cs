﻿using System;
using System.Collections;
using Helpers;
using UnityEngine;

namespace InteractiveObject
{
    public class GrapplingHookController : MonoBehaviour
    {
        public SpriteRenderer ChainSprite;
        public Transform HookTransform;
        public Collider2D HookCollider;
        public Player.PlayerControl Control;

        public float ChainMaxLength = 20f;
        public float ChainMinLength = 1f;
        public float AnimationSpeed = 0.5f;
    
        public float ChainLength
        {
            set
            {
                ChainSprite.size = new Vector2(value, ChainSprite.size.y);
                HookTransform.localPosition = new Vector3(0, -value, 0);
            }
            get => ChainSprite.size.x;
        }

        private bool dropped = true;
        private Coroutine resetCoroutine;

        private void Awake()
        {
            ChainLength = dropped ? ChainMaxLength : ChainMinLength;
        }

        public IEnumerator ResetLengthCoroutine(float time)
        {
            var startTime = Time.time;
            var finishTime = startTime + time;
            var startLength = ChainLength;
            var targetLength = dropped ? ChainMaxLength : ChainMinLength;

            while (Time.time < finishTime)
            {
                ChainLength = EasingFunction.EaseOutBounce(startLength, targetLength, 1f / time * (Time.time - startTime));
                yield return null;
            }

            ChainLength = targetLength;
            resetCoroutine = null;
        }

        private void OnEnable()
        {
            Control.OnHoldCancel += HoldCancelHandler;
            Control.OnHoldStart += HoldStartHandler;
            Control.OnHoldProgress += HoldProgressHandler;
            Control.OnHoldFinish += HoldFinishHandler;
        }
        
        void OnDisable()
        {
            Control.OnHoldCancel -= HoldCancelHandler;
            Control.OnHoldStart -= HoldStartHandler;
            Control.OnHoldProgress -= HoldProgressHandler;
            Control.OnHoldFinish -= HoldFinishHandler;
        }
    
        private void HoldCancelHandler()
        {
            resetCoroutine = StartCoroutine(ResetLengthCoroutine(AnimationSpeed));
        }

        private void HoldStartHandler(bool value)
        {
            if (resetCoroutine != null) StopCoroutine(resetCoroutine);
            resetCoroutine = null;
        }

        private void HoldFinishHandler(bool value)
        {
            dropped = !value;
        }

        private void HoldProgressHandler(bool up, float progress)
        {
            var startLength = dropped ? ChainMaxLength : ChainMinLength;
            var targetLength = up ? ChainMinLength : ChainMaxLength;
            ChainLength = Mathf.Lerp(startLength, targetLength, progress);
        }

    }
}