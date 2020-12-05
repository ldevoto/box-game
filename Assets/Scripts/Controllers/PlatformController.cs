using System;
using UnityEngine;

namespace Controllers
{
    public class PlatformController : MonoBehaviour
    {
        private Animator animator = null;
        private static readonly int VanishTrigger = Animator.StringToHash("Vanish");

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void Vanish()
        {
            animator.SetTrigger(VanishTrigger);
        }
    }
}