using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Animator anim;
    [SerializeField] private Text nameText;

    public float moveSpeed = 2.5f;
    public float minPauseDuration = 1f;
    public float maxPauseDuration = 3f;
    private Vector2 areaSize;

    private Vector3 targetPosition;
    private bool isMoving = false;

    void Start()
    {
        StartCoroutine(MoveRandomly());
    }

    public void CreatePlayer(string name, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        nameText.text = name;
    }

    public void GetSizeArea(Vector2 size)
    {
        areaSize = size;
    }

    IEnumerator MoveRandomly()
    {
        while (true)
        {
            targetPosition = GetRandomPosition();
            isMoving = true;
            anim.SetBool("Run", true);

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                yield return null;
            }

            isMoving = false;
            anim.SetBool("Run", false);
            float pauseDuration = Random.Range(minPauseDuration,maxPauseDuration);
            yield return new WaitForSeconds(pauseDuration);
        }
    }

    Vector3 GetRandomPosition()
    {
        float randomX = Random.Range(-areaSize.x, areaSize.x);
        float randomY = Random.Range(-areaSize.y, areaSize.y);
        return new Vector3(randomX, randomY, transform.position.z);
    }
}
