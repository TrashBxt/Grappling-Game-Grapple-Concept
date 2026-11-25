using UnityEngine;

public class Kunai : MonoBehaviour
{
    [SerializeField] private GameObject kunaiPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private float throwSpeed = 15f;
    [SerializeField] private float cooldown = 3f;

    private float nextThrowTime = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) &&
            Time.time >= nextThrowTime)
        {
            ThrowKunai();
            nextThrowTime = Time.time + cooldown;
        }
    }

    private void ThrowKunai()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f;

        GameObject kunai = Instantiate(
            kunaiPrefab, throwPoint.position, Quaternion.identity);

        Rigidbody2D rb = kunai.GetComponent<Rigidbody2D>();

        rb.linearVelocity = new Vector2(direction * throwSpeed, 0f);

        if (direction < 0)
        {
            kunai.transform.Rotate(0f, 0f, 180f);
        }

        Destroy(kunai, 5f);
    }
}