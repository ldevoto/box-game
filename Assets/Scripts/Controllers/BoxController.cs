using System;
using SO;
using UnityEngine;
using UnityTemplateProjects.Base;

namespace Controllers
{
    public class BoxController : MonoBehaviour
    {
        public AnimationPreset animationRoll = null;
        public AnimationPreset animationFailedRoll = null;
        public Animator animator;

        private bool controlsCaptured = false;
        private Movement movement = Movement.NONE;
        private static readonly int ExitLevelTrigger = Animator.StringToHash("ExitLevel");

        const int indicatorsLayer = 1 << 8;
        
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
            var indication = GetMovementIndication(direction);
            switch (indication)
            {
                case IndicationType.CLIFF:
                    StartCoroutine(animationFailedRoll.Animate(MoveTowards(direction), StopMoving));
                    break;
                case IndicationType.EMPTY:
                    StartCoroutine(animationRoll.Animate(MoveTowards(direction), StopMoving));
                    break;
                case IndicationType.EXIT:
                    StartCoroutine(animationRoll.Animate(MoveTowards(direction), ExitLevel));
                    break;
                case IndicationType.JUMP:
                case IndicationType.BUTTON:
                case IndicationType.LASER:
                case IndicationType.TELEPORT:
                    StopMoving();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private IndicationType GetMovementIndication(Direction direction)
        {
            if (Physics.Raycast(transform.position, DirectionUtils.normalByDirection[direction], out var hit, WorldConstants.unit, indicatorsLayer))
            {
                Debug.DrawRay(transform.position, DirectionUtils.normalByDirection[direction] * WorldConstants.unit, Color.red, 2);
                var indicatorController = hit.collider.gameObject.GetComponent<IndicatorController>();
                return indicatorController.GetIndication();
            }
            Debug.DrawRay(transform.position, DirectionUtils.normalByDirection[direction] * WorldConstants.unit, Color.green, 2);
            return IndicationType.EMPTY;
        }

        private Action<float> MoveTowards(Direction direction)
        {
            var pivot = DirectionUtils.GetPivotForMovement(direction, transform.position);
            return value => RotateAroundDelta(pivot, direction, value);
        }

        private void ExitLevel()
        {
            SnapToGrid();
            animator.SetTrigger(ExitLevelTrigger);
            Debug.Log("Win!");
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
    }
}