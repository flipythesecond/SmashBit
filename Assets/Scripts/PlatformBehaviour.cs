using UnityEngine;

public class PlatformBehaviour : MonoBehaviour
{
    public float moveSpeed = 2f;   // how fast it moves
    public float waitMin = 1f;     // min wait time
    public float waitMax = 3f;     // max wait time

    public float yMin = -1f;       // min y position
    public float yMax = 3f;        // max y position
    public float xMin = -5f;       // min x position
    public float xMax = 5f;        // max x position

    private float targetY;
    private float targetX;

    private bool isMoving = false;
    private float waitTimer;

    void Start()
    {
        // start waiting before first move
        SetNextWait();
    }

    void Update()
    {
        if (isMoving)
        {
            // move toward target x and y
            Vector3 pos = transform.position;

            pos.x = Mathf.MoveTowards(pos.x, targetX, moveSpeed * Time.deltaTime);
            pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);

            transform.position = pos;

            // check if reached BOTH x and y targets
            if (Mathf.Abs(pos.x - targetX) < 0.01f &&
                Mathf.Abs(pos.y - targetY) < 0.01f)
            {
                isMoving = false;
                SetNextWait();
            }
        }
        else
        {
            // countdown wait
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0f)
            {
                ChooseNewTarget();
                isMoving = true;
            }
        }
    }

    void ChooseNewTarget()
    {
        // pick random x and y within range
        targetX = Random.Range(xMin, xMax);
        targetY = Random.Range(yMin, yMax);
    }

    void SetNextWait()
    {
        // random delay before next move
        waitTimer = Random.Range(waitMin, waitMax);
    }
}