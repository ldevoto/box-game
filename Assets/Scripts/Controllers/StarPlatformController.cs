using UnityEngine;

namespace Controllers
{
    public class StarPlatformController : MonoBehaviour
    {
        [SerializeField] private GameObject star = null;
        [SerializeField] private ParticleSystem pickUp = null;

        private bool taken = false;

        public void PickUpStar()
        {
            if (taken) return;

            taken = true;
            star.SetActive(false);
            pickUp.gameObject.SetActive(true);
            //var main = pickUp.main;
            //main.simulationSpeed = 5f;
            pickUp.Play();
        }

        public bool HasStar()
        {
            return !taken;
        }
    }
}