using System.Collections;
using SO;
using UnityEngine;

namespace Controllers
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private AnimationPreset platformAppearAnimation = null;
        [SerializeField] private GameObject[] platforms = null;
        [SerializeField] private GameObject winPlatform = null;
        [SerializeField] private BoxController playerPrefab = null;

        private BoxController player = null;
        private GameObject spawnPoint = null;

        private void Awake()
        {
            spawnPoint = GameObject.FindWithTag("SpawnPoint");
        }

        private void Start()
        {
            Restart();
        }

        private void Restart()
        {
            if (player)
            {
                Destroy(player.gameObject);
            }
            HidePlatforms();
            StartCoroutine(SpawnPlatforms());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }

        private void HidePlatforms()
        {
            foreach (var platform in platforms)
            {
                SetPosition(platform, platformAppearAnimation.minValue);
            }
            SetPosition(winPlatform, platformAppearAnimation.minValue);
        }

        private IEnumerator SpawnPlatforms()
        {
            foreach (var platform in platforms)
            {
                StartCoroutine(platformAppearAnimation.Animate(value => SetPosition(platform, value)));
                yield return new WaitForSeconds(0.1f);
            }
            StartCoroutine(platformAppearAnimation.Animate(value => SetPosition(winPlatform, value)));
            yield return new WaitForSeconds(0.1f);
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            if (player)
            {
                Destroy(player.gameObject);
            }
            player = Instantiate(playerPrefab, spawnPoint.transform);
            player.OnDead += SpawnPlayer;
        }
        
        private static void SetPosition(GameObject platform, float yValue)
        {
            var position = platform.transform.localPosition;
            position = new Vector3(position.x, yValue, position.z);
            platform.transform.localPosition = position;
        }
    }
}