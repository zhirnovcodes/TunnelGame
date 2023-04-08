using UnityEngine;

public class MoveInTunnelState : MonoBehaviour
{
    public TunnelSplineModel Spline;
    public SplinePositionModel Position;

    public float SpeedForward = 1f;
    public float TimeSideMove = 0.5f;
    public float OffsetSidePosition = 0.5f;
    public bool ShouldUseInput;

    private float CurrentSidePosition;
    private float TargetSidePosition;

    private IState CurrentState;
    private IState FMoveForwardState;
    private IState FMoveSideState;
    private IState FMoveSideVectorState;

    private void Awake()
    {
        FMoveForwardState = new MoveForwardState(this);
        FMoveSideState = new MoveSideState(this);
        FMoveSideVectorState = new MoveSideVectorState(this);

        SetCurrentState(FMoveForwardState);
    }

    private void Update()
    {
        CurrentState.Update();
    }

    private void SetCurrentState(IState newState)
    {
        if (CurrentState != null)
        {
            CurrentState.OnEnd();
        }

        CurrentState = newState;

        CurrentState.OnStart();

        //Debug.Log("New state " + CurrentState.GetType().Name);
    }

    private void MovePosition(Vector3 speed)
    {
        Position.Data = new SplinePositionData { Position = speed + Position.Data.Position };//Spline.MovePosition(Position.Data, speed * Time.deltaTime);
        Debug.Log(Position.Data.Position);
    }

    private void ApplyWorldPosition(Quaternion? localRotation = null)
    {
        localRotation = localRotation ?? Quaternion.identity;

        //var worldPosition = Spline.GetWorldPositionRotation(Position.Data);
        var worldPosition = Spline.ToWorldSpace(Position.Data.Position);

        transform.position = worldPosition.Position;
        transform.rotation = worldPosition.Rotation * localRotation.Value;
    }

    private Vector3 GetInputDirection()
    {
        if (ShouldUseInput)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                return Vector3.left;
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                return Vector3.right;
            }
        }

        return Vector3.zero;
    }

    private bool ApplyLeftRightInput()
    {

        var direction = GetInputDirection();

        if (direction.sqrMagnitude > 0)
        {
            var newPositionX = TargetSidePosition + (direction * OffsetSidePosition).x;

            if (Mathf.Abs(newPositionX) <= OffsetSidePosition)
            {
                CurrentSidePosition = Position.Data.Position.x;
                TargetSidePosition = newPositionX;

                return true;
            }
        }

        return false;
    }

    private interface IState
    {
        void Update();
        void OnStart();
        void OnEnd();
    }

    private class MoveForwardState : IState
    {
        private MoveInTunnelState Machine { get; }

        public MoveForwardState(MoveInTunnelState machine)
        {
            Machine = machine;
        }

        public void OnStart()
        {
        }

        public void OnEnd()
        {
        }

        public void Update()
        {
            var speed = new Vector3(0, 0, Machine.SpeedForward) * Time.deltaTime;

            Machine.MovePosition(speed);
            Machine.ApplyWorldPosition();

            if (Machine.ApplyLeftRightInput())
            {
                return;
                Machine.SetCurrentState(Machine.FMoveSideState);
            }
        }
    }

    private class MoveSideState : IState
    {
        private MoveInTunnelState Machine { get; }

        private float StartTime;
        private float TimeMove;

        public MoveSideState(MoveInTunnelState machine)
        {
            Machine = machine;
        }

        public void OnStart()
        {
            StartTime = Time.time;
            var distance = Mathf.Abs( Machine.CurrentSidePosition - Machine.TargetSidePosition );
            TimeMove = (distance * Machine.TimeSideMove) / Machine.OffsetSidePosition;
        }

        public void Update()
        {
            var speed = new Vector3(0, 0, Machine.SpeedForward) * Time.deltaTime;

            Machine.MovePosition(speed);

            var position = Machine.Position.Data.Position;
            var t = Mathf.Clamp01( (Time.time - StartTime) / TimeMove);

            position.x = XFunction(t);
            Machine.Position.Data.Position = position;

            Machine.ApplyWorldPosition();

            if (t >= 1)
            {
                Machine.SetCurrentState(Machine.FMoveForwardState);
            }

            if (Machine.ApplyLeftRightInput())
            {
                Machine.SetCurrentState(Machine.FMoveSideState);
            }
        }

        private float XFunction(float t)
        {
            var direction = Machine.CurrentSidePosition - Machine.TargetSidePosition;
            return (Mathf.Cos(Mathf.PI * t) - 1) / 2f * direction + Machine.CurrentSidePosition;

        }

        public void OnEnd()
        {
        }
    }

    private class MoveSideVectorState : IState
    {
        private MoveInTunnelState Machine { get; }

        private Vector3 CurrentSpeed;
        private float StartPosition;
        private float TargetPosition;

        private float CurrentPositionX => Machine.Position.Data.Position.x;

        public MoveSideVectorState(MoveInTunnelState machine)
        {
            Machine = machine;
        }

        public void OnStart()
        {
            CurrentSpeed = Vector3.forward * Machine.SpeedForward;
            StartPosition = 0;
            TargetPosition = 0;
        }

        public void Update()
        {
            var distanceToTarget = TargetPosition - CurrentPositionX;
            var distanceFromStartToTarget = Mathf.Abs(TargetPosition - StartPosition);
            var force = XFunction(distanceToTarget, distanceFromStartToTarget, distanceFromStartToTarget);
            var forceVector = new Vector3(force, 0, 0);

            var speed = (forceVector + CurrentSpeed).normalized * Machine.SpeedForward;
            var localRotation = Quaternion.FromToRotation(Vector3.forward, speed);

            Machine.MovePosition(speed);
            Machine.ApplyWorldPosition(localRotation);

            var direction = Machine.GetInputDirection();

            if (direction.sqrMagnitude != 0)
            {
                var newPositionX = TargetPosition + (direction * Machine.OffsetSidePosition).x;

                if (Mathf.Abs(newPositionX) <= Machine.OffsetSidePosition)
                {
                    StartPosition = CurrentPositionX;
                    TargetPosition = newPositionX;
                }
            }
        }

        private float XFunction(float x, float distance, float maxLength)
        {
            if (distance == 0)
            {
                return 0;
            }
            return Mathf.Sin(x * Mathf.PI / (distance + 0.0001f)) * maxLength;

        }

        public void OnEnd()
        {
        }
    }
}
