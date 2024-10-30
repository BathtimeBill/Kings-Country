using System.Collections;
using UnityEngine;

public class MaegenSprite : GameBehaviour
{
    public GameObject homeTree;
    public float speed;
    public float floatSpeed;
    public bool isMovingTowardsTree;
    public Vector3 pointAboveTree;


    public GameObject plusParticle;
    public GameObject collectMaegenParticle;

    void Start()
    {
        homeTree = GameObject.FindGameObjectWithTag("Home Tree");
        pointAboveTree = transform.position + new Vector3(0, 200, 0);
        StartCoroutine(WaitForChangeDirection());
    }

    // Update is called once per frame
    void Update()
    {
        if(isMovingTowardsTree)
        {
            transform.position = Vector3.MoveTowards(transform.position, homeTree.transform.position, speed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pointAboveTree, floatSpeed);
        }
    }

    IEnumerator WaitForChangeDirection()
    {
        yield return new WaitForSeconds(Random.Range(0.8f, 1.2f));
        isMovingTowardsTree = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Home Tree"))
        {
            Instantiate(plusParticle, transform.position, transform.rotation);
            Instantiate(collectMaegenParticle, transform.position, transform.rotation);
            _GM.IncreaseMaegen(1);
            GameEvents.ReportOnWispDestroy();
            Destroy(gameObject);
        }
    }
}
