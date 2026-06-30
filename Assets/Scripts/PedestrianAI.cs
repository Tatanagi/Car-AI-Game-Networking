using UnityEngine;

public class PedestrianAI : MonoBehaviour
{
    [Header("Crossing Points")]
    public Transform[] crossingPoints;   // ← Array: Start, Mid1, Mid2, ..., End

    [Header("Traffic Lights")]
    public StopLight[] trafficLights;

    [Header("Movement")]
    public float walkSpeed = 3.5f;
    public float waitTimeAtEnd = 3f;

    private int currentTargetIndex = 0;
    private Vector3 targetPosition;

    private enum CrossingState
    {
        WaitingToStart,
        Moving,
        WaitingAtIntermediate,
        Finished
    }

    private CrossingState currentState = CrossingState.WaitingToStart;

    private float waitTimer = 0f;
    private bool waitingForNextRed = false;

    private void Start()
    {
        if (crossingPoints != null && crossingPoints.Length > 0)
        {
            transform.position = crossingPoints[0].position;
            currentTargetIndex = 0;
        }
    }

    private void Update()
    {
        if (crossingPoints == null || crossingPoints.Length < 2)
            return;

        bool isRed = IsRedLight();

        switch (currentState)
        {
            case CrossingState.WaitingToStart:
                if (isRed)
                {
                    StartMovingToNextPoint();
                }
                break;

            case CrossingState.Moving:
                MoveToTarget();
                if (ReachedTarget())
                {
                    ArriveAtPoint();
                }
                break;

            case CrossingState.WaitingAtIntermediate:
                // At intermediate point: wait for green then next red
                if (!waitingForNextRed && !isRed) // Light turned green
                {
                    waitingForNextRed = true;
                }
                else if (waitingForNextRed && isRed)
                {
                    StartMovingToNextPoint();
                }
                break;

            case CrossingState.Finished:
                waitTimer += Time.deltaTime;
                if (waitTimer >= waitTimeAtEnd)
                {
                    // Optional: despawn, return to start, or loop
                    Debug.Log($"<color=gray>[Pedestrian] Finished waiting at end ({gameObject.name})</color>");
                }
                break;
        }
    }

    private bool IsRedLight()
    {
        if (trafficLights == null || trafficLights.Length == 0)
            return true;

        foreach (var light in trafficLights)
        {
            if (light != null && !light.isGreen)
                return true;
        }
        return false;
    }

    private void StartMovingToNextPoint()
    {
        if (currentTargetIndex >= crossingPoints.Length - 1)
            return;

        currentTargetIndex++;
        targetPosition = crossingPoints[currentTargetIndex].position;
        currentState = CrossingState.Moving;
        waitingForNextRed = false;

        Debug.Log($"<color=cyan>[Pedestrian] Moving to point {currentTargetIndex} ({gameObject.name})</color>");
    }

    private void ArriveAtPoint()
    {
        transform.position = targetPosition;

        if (currentTargetIndex >= crossingPoints.Length - 1)
        {
            // Final destination
            currentState = CrossingState.Finished;
            waitTimer = 0f;
            Debug.Log($"<color=green>[Pedestrian] Reached final destination ({gameObject.name})</color>");
        }
        else
        {
            // Intermediate point
            currentState = CrossingState.WaitingAtIntermediate;
            waitingForNextRed = false;
            Debug.Log($"<color=orange>[Pedestrian] Arrived at intermediate point {currentTargetIndex} ({gameObject.name})</color>");
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            walkSpeed * Time.deltaTime
        );
    }

    private bool ReachedTarget()
    {
        return Vector3.Distance(transform.position, targetPosition) <= 0.2f;
    }

    /// <summary>
    /// Assign multiple points and traffic lights
    /// </summary>
    public void AssignReferences(Transform[] points, StopLight[] lights)
    {
        crossingPoints = points;
        trafficLights = lights;

        if (points != null && points.Length > 0)
        {
            transform.position = points[0].position;
        }

        currentTargetIndex = 0;
        currentState = CrossingState.WaitingToStart;
        waitTimer = 0f;
        waitingForNextRed = false;

        Debug.Log($"<color=lime>[Pedestrian] Assigned {points?.Length} crossing points ({gameObject.name})</color>");
    }

    private void OnDestroy()
    {
        // Cleanup if needed
    }
}