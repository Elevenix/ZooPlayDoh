using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooManager : MonoBehaviour
{
    [SerializeField] private Player prefabPlayer;
    [SerializeField] private Vector2 sizeRandomSpawn;

    private List<Player> _players = new List<Player>();

    public void AddPlayer(string name,Sprite sprite, float size, bool randomSpawn = true)
    {
        Vector2 pos = Vector2.zero;

        if (randomSpawn)
        {
            float posX = Random.Range(-sizeRandomSpawn.x, sizeRandomSpawn.x);
            float posY = Random.Range(-sizeRandomSpawn.y, sizeRandomSpawn.y);
            pos = new Vector2(posX, posY);
        }

        Player newPlayer = Instantiate(prefabPlayer, pos, Quaternion.identity);
        newPlayer.CreatePlayer(name, sprite);
        newPlayer.gameObject.transform.localScale = Vector3.one * size;
        newPlayer.GetSizeArea(sizeRandomSpawn);
        _players.Add(newPlayer);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, sizeRandomSpawn*2);
    }
}
