//using UnityEngine;

//public class SpaceTornadoSo25 : MonoBehaviour
//{
//    [SerializeField]
//    private float pullForce = 5f;
//    [SerializeField]
//    private float rotateForce = 150f;
//    [SerializeField] 
//    private float radius = 3f;
//    [SerializeField] 
//    private float lifeTime = 5f;

//    private void Start()
//    {
//        Destroy(gameObject, lifeTime);
//    }

//    private void OnTriggerStay2D(Collider2D col)
//    {
//        Rigidbody2D rb = col.attachedRigidbody;
//        if (rb == null) return;

//        Vector2 dirToCenter = (Vector2)transform.position - rb.position;

//        // Kéo vật thể vào tâm
//        rb.AddForce(dirToCenter.normalized * pullForce);

//        // Xoay vật thể xung quanh tâm
//        Vector3 tangent = Vector3.Cross(dirToCenter, Vector3.forward).normalized;
//        rb.AddForce(tangent * rotateForce);
//    }
//}
