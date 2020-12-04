using System;
using System.Collections;
using UnityEngine;

namespace SO
{
    [CreateAssetMenu(fileName = "AnimationPreset", menuName = "SO/AnimationPreset", order = 0)]
    public class AnimationPreset : ScriptableObject
    {
        public AnimationCurve curve;
        public float duration;
        public float minValue;
        public float maxValue;
        public bool deltaAnimate;

        public IEnumerator Animate(Action<float> onFrame, Action onFinished = null)
        {
            var lastValue = 0f;
            var elapsedTime = 0f;
            while (elapsedTime <= 1f)
            {
                var value = GetValue(elapsedTime);
                onFrame?.Invoke(deltaAnimate ? value - lastValue : value);

                lastValue = value;
                yield return null;
                elapsedTime += Time.deltaTime / duration;
            }

            var finalValue = GetValue(curve[curve.length - 1].time);
            onFrame?.Invoke(deltaAnimate ? finalValue - lastValue : finalValue);
            onFinished?.Invoke();
        }

        private float GetValue(float time)
        {
            return curve.Evaluate(time) * (maxValue - minValue) + minValue;
        }
    }
}