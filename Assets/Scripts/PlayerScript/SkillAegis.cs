using UnityEngine;

public class SkillAegis : MonoBehaviour
{
    public float timer;
    float mautang = 0f;
    public void OnTriggerStay2D(Collider2D collision)
    {
        PlayerController controller = GetComponent<PlayerController>();
        if (collision.CompareTag("TauMe"))
        {
            if (controller.thanhmauhientai < controller.thanhmauToiDa)
            {
                timer += Time.deltaTime;
                mautang += 3f * Time.deltaTime;
                if (timer >= 1)
                {
                    DamePopUpGenerator.Instance.CreatePopUpHeal(transform.position, mautang);
                    timer = 0f;
                }

                controller.thanhmauhientai += 3f * Time.deltaTime;
                controller.thanhmau.capnhatthanhmau(controller.thanhmauhientai, controller.thanhmauToiDa);            
            }
        }
    }
}
