using UnityEngine;

public class Background : MonoBehaviour
{
    public float Speed;

    private void FixedUpdate()
    {
        if (transform.position.y < -20)
            transform.position = Vector2.zero;
        transform.Translate(Speed * Time.fixedDeltaTime * Vector2.down);
    }
}