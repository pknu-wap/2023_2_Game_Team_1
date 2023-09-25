﻿using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    // [MBTNode(name = "Tasks/Wait")]
    public class Wait : Leaf
    {
        [Tooltip("Wait time in seconds")] public FloatReference time = new(1f);

        public float randomDeviation;
        public bool continueOnRestart;
        private float timer;

        private void OnValidate()
        {
            randomDeviation = Mathf.Clamp(randomDeviation, 0f, time.isConstant ? time.GetConstant() : 600f);
        }

        public override void OnEnter()
        {
            if (!continueOnRestart)
                timer = randomDeviation == 0f ? 0f : Random.Range(-randomDeviation, randomDeviation);
        }

        public override NodeResult Execute()
        {
            if (timer >= time.Value)
            {
                // Reset timer in case continueOnRestart option is active
                if (continueOnRestart)
                    timer = randomDeviation == 0f ? 0f : Random.Range(-randomDeviation, randomDeviation);
                return NodeResult.success;
            }

            timer += DeltaTime;
            return NodeResult.running;
        }
    }
}