using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public void CreatePlayer(string name, Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
