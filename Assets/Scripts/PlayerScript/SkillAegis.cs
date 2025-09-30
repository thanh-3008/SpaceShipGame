using UnityEngine;

public class SkillAegis : MonoBehaviour
{
    public void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController controller = GetComponent<PlayerController>();
        if (collision.CompareTag("TauMe"))
        {
            if (controller.thanhmauhientai < controller.thanhmauToiDa)
            {
                controller.thanhmauhientai += 3f * Time.deltaTime;
                controller.thanhmau.capnhatthanhmau(controller.thanhmauhientai, controller.thanhmauToiDa);
            }
        }
    }
}
