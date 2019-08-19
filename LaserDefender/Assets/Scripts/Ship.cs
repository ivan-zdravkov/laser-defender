using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class Ship : MonoBehaviour
    {
        [SerializeField] protected AlignmentEnum alignment = AlignmentEnum.Enemy;

        [SerializeField] protected float health = 100f;
        [SerializeField] protected float projectileSpeed = 10f;

        [SerializeField] protected GameObject laserPrefab;

        [SerializeField] protected GameObject hitVFX;
        [SerializeField] protected GameObject deathVFX;

        [SerializeField] protected AudioClip hitSFX;
        [SerializeField] protected AudioClip deathSFX;
        [SerializeField] protected AudioClip shootSFX;

        [SerializeField] [Range(0, 1)] protected float shootVolume;
        [SerializeField] [Range(0, 1)] protected float dieVolume;

        protected void OnTriggerEnter2D(Collider2D other)
        {
            DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();

            if (damageDealer.Alignment != this.alignment)
                this.ProcessHit(damageDealer);
        }

        protected void ProcessHit(DamageDealer damageDealer)
        {
            damageDealer.DestroyOnHit();

            this.health -= damageDealer.Damage;

            if (this.health <= 0)
                this.Die();
            else
                this.Hit();
        }

        protected void Hit()
        {
            this.PlayVFX(this.hitVFX);
            this.PlaySFX(this.hitSFX);
        }

        protected virtual void Die()
        {
            this.PlayVFX(this.deathVFX);
            this.PlaySFX(this.deathSFX);

            Destroy(this.gameObject);
        }

        protected void PlaySFX(AudioClip clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, this.dieVolume);
        }

        protected void PlayVFX(GameObject vfx)
        {
            GameObject explosion = Instantiate(vfx, this.transform.position, Quaternion.identity);

            Destroy(explosion, 1f);
        }
    }
}
