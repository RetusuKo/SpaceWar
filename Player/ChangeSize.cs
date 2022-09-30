using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSize : MonoBehaviour
{
    private Vector2 _rollVectoroffset = new Vector2(0, 0.75f);
    private Vector2 _rollVectorsize = new Vector2(1.415656f, 1.22f);

    private Vector2 _standartVectoroffset = new Vector2(0, 1.384723f);
    private Vector2 _standartVectorsize = new Vector2(1.415656f, 2.47633f);

    public void RollChange(BoxCollider2D collider)
    {
        collider.offset = _rollVectoroffset;
        collider.size = _rollVectorsize;
        PlayerInfo.DoNotTakeDamage = true;
    }
    public void StandartSize(BoxCollider2D collider)
    {
        collider.offset = _standartVectoroffset;
        collider.size = _standartVectorsize;
    }
}
