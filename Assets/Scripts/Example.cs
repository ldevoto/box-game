using UnityEngine;

namespace UnityTemplateProjects
{
    public class Example : MonoBehaviour
    {
        //Assign a GameObject in the Inspector to rotate around
        public GameObject target;
        public AnimationCurve behabiour;
        public float speed = 1;
        private float totalTime = 0;
        private float lastAngle = 0;

        void Update()
        {
            totalTime += Time.deltaTime;
            //transform.RotateAround(target.transform.position, Vector3.right, );
            var angle = - behabiour.Evaluate(totalTime / (1 / speed)) * 360;
            transform.RotateAround(target.transform.position, Vector3.right, angle - lastAngle);
            lastAngle = angle;
        }
    }
}