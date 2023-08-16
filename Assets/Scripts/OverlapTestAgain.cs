using UnityEngine;

public class OverlapTestAgain : MonoBehaviour
{
    public float moveSpeed = 3f;
    public LayerMask overlapLayer;
    private BoxCollider2D coll;
    private Collider2D overlapCollCircle;
    private bool isOverlapping = false;

    private void Start()
    {
        coll = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isOverlapping)
        {
            Vector2 closestPoint = overlapCollCircle.ClosestPoint(coll.bounds.center);
            Vector2 overlapDirection = (Vector2)(transform.position - (Vector3)closestPoint);
            overlapDirection.Normalize();
            transform.Translate(overlapDirection * moveSpeed * Time.deltaTime);
            if (!coll.IsTouching(overlapCollCircle))
            {
                isOverlapping = false;
            }
        }
    }

    private void FixedUpdate()
    {
        Collider2D[] overlaps = Physics2D.OverlapBoxAll(coll.bounds.center, coll.bounds.size, 0f, overlapLayer);
        if (overlaps.Length > 0)
        {
            overlapCollCircle = overlaps[0];
            isOverlapping = true;
        }
    }
}
