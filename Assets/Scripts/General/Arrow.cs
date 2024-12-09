using UnityEngine;

public class Arrow : GameBehaviour
{
    public float speed;
    private Transform target;
    private Vector3 colliderCentre;

    public void Setup(Transform _target)
    {
        target = _target;
        colliderCentre = _target.GetComponent<Collider>().bounds.center;
        GetComponent<AudioSource>().Play();
    }
    private void FixedUpdate()
    {
        if(!target)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, colliderCentre, speed);
            transform.LookAt(colliderCentre);
        }
    }
}
