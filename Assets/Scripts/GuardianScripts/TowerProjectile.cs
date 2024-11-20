using UnityEngine;

public class TowerProjectile : GameBehaviour
{
    public GameObject particleEffect;
    public GameObject spitDecal;
    public bool isSpitTower;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != "Tree" || collision.collider.tag != "Unit" || collision.collider.tag != "Wildlife")
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
            if(isSpitTower)
            {
                Vector3 offset = new Vector3(0, 0.5f, 0);
                Vector3 rotationOffset = new Vector3(90, 0, 0);
                Instantiate(spitDecal, transform.position + offset, Quaternion.Euler(rotationOffset));
            }

            gameObject.SetActive(false);
        }
    }
}
