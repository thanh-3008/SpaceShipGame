using UnityEngine;

public class ChuyenDoi : MonoBehaviour
{
    public Animator anim;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void BienDoi()
    {
        anim.SetTrigger("biendoi");
    }
}
