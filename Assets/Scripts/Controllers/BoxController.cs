using System;
using System.Collections;
using SO;
using UnityEngine;
using UnityTemplateProjects.Base;

namespace Controllers
{
    public class BoxController : MonoBehaviour
    {
        [SerializeField] private AnimationPreset animationRoll = null;
        [SerializeField] private AnimationPreset animationFailedRoll = null;
        [SerializeField] private AnimationPreset animationAppear = null;
        [SerializeField] private AnimationPreset animationFall = null;
        [SerializeField] private Animator animator;

        public Action OnExitLevel;
        public Action OnDead;
        public Action OnMove;
        public Action OnCaptureStar;

        private bool controlsCaptured = false;
        private Movement movement = Movement.NONE;
        private static readonly int ExitLevelTrigger = Animator.StringToHash("ExitLevel");
        private static readonly int FallTrigger = Animator.StringToHash("Fall");

        const int indicatorsLayer = 1 << 8;

        private void Start()
        {
            EnterLevel();
        }
        
        private void Initialize()
        {
            controlsCaptured = true;
            movement = Movement.NONE;
        }

        private void Update()
        {
            CheckMovement();
            if (controlsCaptured) return;
            if (movement == Movement.NONE) return;
            
            controlsCaptured = true;
            DetermineMovementBehaviour(MovementUtils.DirectionByMovement[movement]);
            movement = Movement.NONE;
        }

        private void CheckMovement()
        {
            if (Input.GetButtonDown("Forward"))
            {
                movement = Movement.FORWARD;
            }
            else if (Input.GetButtonDown("Backward"))
            {
                movement = Movement.BACKWARD;
            }
            else if (Input.GetButtonDown("Right"))
            {
                movement = Movement.RIGHT;
            }
            else if (Input.GetButtonDown("Left"))
            {
                movement = Movement.LEFT;
            }
        }

        private void DetermineMovementBehaviour(Direction direction)
        {
            OnMove?.Invoke();
            var indicatorController = GetIndicatorController(direction);
            var indication = GetIndication(indicatorController);
            switch (indication)
            {
                case IndicationType.EMPTY:
                    StartCoroutine(animationFailedRoll.Animate(MoveTowards(direction), StopMoving));
                    break;
                case IndicationType.PLATFORM:
                    StartCoroutine(animationRoll.Animate(MoveTowards(direction), StopMoving));
                    break;
                case IndicationType.STAR_PLATFORM:
                    StartCoroutine(animationRoll.Animate(MoveTowards(direction), CaptureStar(indicatorController), StopMoving));
                    break;
                case IndicationType.EXIT:
                    StartCoroutine(animationRoll.Animate(MoveTowards(direction), ExitLevel));
                    break;
                case IndicationType.FAKE_PLATFORM:
                    indicatorController.GetPlatformController().Vanish();
                    StartCoroutine(animationFall.Animate(MoveTowards(direction), () => Fall(direction)));
                    break;
                case IndicationType.JUMP:
                case IndicationType.BUTTON:
                case IndicationType.LASER:
                case IndicationType.TELEPORT:
                case IndicationType.CLIFF:
                    StopMoving();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Action CaptureStar(IndicatorController indicatorController)
        {
            var starPlatformController = indicatorController.GetMainController<StarPlatformController>();
            return () =>
            {
                if (!starPlatformController.HasStar()) return;
                starPlatformController.PickUpStar();
                OnCaptureStar?.Invoke();
            };
        }

        private IndicatorController GetIndicatorController(Direction direction)
        {
            if (Physics.Raycast(transform.position, DirectionUtils.normalByDirection[direction], out var hit, WorldConstants.unit, indicatorsLayer))
            {
                Debug.DrawRay(transform.position, DirectionUtils.normalByDirection[direction] * WorldConstants.unit, Color.green, 2);
                var indicatorController = hit.collider.gameObject.GetComponent<IndicatorController>();
                return indicatorController;
            }
            Debug.DrawRay(transform.position, DirectionUtils.normalByDirection[direction] * WorldConstants.unit, Color.red, 2);
            return null;
        }

        private static IndicationType GetIndication(IndicatorController indicatorController)
        {
            return !indicatorController ? IndicationType.EMPTY : indicatorController.GetIndication();
        }

        private Action<float> MoveTowards(Direction direction)
        {
            var pivot = DirectionUtils.GetPivotForMovement(direction, transform.position);
            return value => RotateAroundDelta(pivot, direction, value);
        }

        public void EnterLevel()
        {
            Initialize();
            StartCoroutine(animationAppear.Animate(value => SetPosition(this.gameObject, value), StopMoving));
        }

        private void ExitLevel()
        {
            SnapToGrid();
            animator.SetTrigger(ExitLevelTrigger);
            OnExitLevel?.Invoke();
        }
        
        private void Fall(Direction direction)
        {
            transform.rotation = Quaternion.LookRotation(DirectionUtils.normalByDirection[direction]);
            animator.SetTrigger(FallTrigger);
            StartCoroutine(KillCoroutine(1));
        }

        private IEnumerator KillCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            Kill();
        }
        
        private void Kill()
        {
            OnDead?.Invoke();
        }

        private void RotateAroundDelta(Vector3 pivot, Direction direction, float value)
        {
            transform.RotateAround(pivot, DirectionUtils.RotationAxisByDirection[direction], value);
        }

        private void StopMoving()
        {
            controlsCaptured = false;
            SnapToGrid();
        }

        private void SnapToGrid()
        {
            var transform1 = transform;
            var position = transform1.localPosition;
            transform1.localPosition = new Vector3(Mathf.Round(position.x * 10f) / 10f, Mathf.Round(position.y * 10f) / 10f, Mathf.Round(position.z * 10f) / 10f);
            transform1.rotation = Quaternion.identity;
        }
        
        private static void SetPosition(GameObject platform, float yValue)
        {
            var position = platform.transform.localPosition;
            position = new Vector3(position.x, yValue, position.z);
            platform.transform.localPosition = position;
        }
    }
}