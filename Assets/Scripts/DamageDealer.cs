using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] int damage = 100;
    [SerializeField] AlignmentEnum alignment = AlignmentEnum.Enemy;

    public int Damage
    {
        get
        {
            return this.damage;
        }
    }

    public void DestroyOnHit()
    {
        Destroy(this.gameObject);
    }

    public AlignmentEnum Alignment
    {
        get
        {
            return this.alignment;
        }
    }
}
