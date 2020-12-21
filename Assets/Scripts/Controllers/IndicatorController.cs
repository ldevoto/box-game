using UnityEngine;
using UnityTemplateProjects.Base;

namespace Controllers
{
    public class IndicatorController : MonoBehaviour
    {
        [SerializeField] private IndicationType indication = IndicationType.EMPTY;
        [SerializeField] private GameObject mainController = null;

        private void Awake()
        {
            gameObject.layer = 8;
            if (GetComponent<Collider>()) return;
            
            var boxCollider = gameObject.AddComponent<BoxCollider>();
            boxCollider.center = new Vector3(0, WorldConstants.halfUnit, 0);
            boxCollider.size = new Vector3(WorldConstants.halfUnit, WorldConstants.unit, WorldConstants.halfUnit);
        }

        public IndicationType GetIndication()
        {
            return indication;
        }

        public PlatformController GetPlatformController()
        {
            return gameObject.transform.parent.GetComponent<PlatformController>();
        }

        public T GetMainController<T>()
        {
            return mainController.GetComponent<T>();
        }
    }

    public enum IndicationType
    {
        CLIFF, JUMP, BUTTON, LASER, TELEPORT, EXIT, EMPTY, PLATFORM, FAKE_PLATFORM, STAR_PLATFORM
    }
}