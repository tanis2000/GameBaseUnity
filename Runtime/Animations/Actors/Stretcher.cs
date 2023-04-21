using System;
using GameBase.Utils;
using UnityEngine;

namespace GameBase.Animations.Actors
{
    public class Hopper : MonoBehaviour
    {
        public float HopTimeInSeconds = 0.2f;
        public float HopHeight = 10.0f;
        public AnimationCurve Curve;

        private Vector3 originalPosition;
        private bool isRunning = false;
        private float timeCounterInSeconds = 0.0f;

        private void Awake()
        {
            originalPosition = transform.position;
        }

        public void Execute()
        {
            isRunning = true;
            timeCounterInSeconds = 0;
        }

        private void Update()
        {
            if (!isRunning)
            {
                return;
            }

            timeCounterInSeconds += Time.deltaTime;
            if (timeCounterInSeconds > HopTimeInSeconds)
            {
                timeCounterInSeconds = HopTimeInSeconds;
                isRunning = false;
            }

            var ratio = timeCounterInSeconds / HopTimeInSeconds;
            var step = Curve.Evaluate(ratio) * HopHeight;
            transform.localPosition = transform.localPosition.WhereY(originalPosition.y + step);
        }
    }
}