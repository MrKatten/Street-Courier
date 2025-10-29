using UnityEngine;

public class Anim : MonoBehaviour
{
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim.Play("wave");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
