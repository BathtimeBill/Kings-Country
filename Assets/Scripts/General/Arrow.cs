using UnityEngine;

public class Arrow : GameBehaviour
{
    public float speed;
    private Transform target;

    public void Setup(Transform _target)
    {
        target = _target;
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
            transform.position = Vector3.MoveTowards(transform.position, target.position + new Vector3(0, 2, 0), speed);
            transform.LookAt(target.position);
        }
    }
}
