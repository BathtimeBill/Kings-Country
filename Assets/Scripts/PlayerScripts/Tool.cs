using UnityEngine;

public class Tool : GameBehaviour
{
    public ToolID toolID;
    public GameObject selectObjects;
    public AudioSource useAudioSource;

    public virtual void Start()
    {
        selectObjects.SetActive(false);
    }
    public virtual void Select()
    {
        selectObjects.SetActive(true);
    }

    public virtual void Deselect()
    {
        selectObjects.SetActive(false);
    }

    public virtual void Use()
    {
        useAudioSource.Play();
    }
}
