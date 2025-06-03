using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public Vector3 cameraDir = Vector3.zero;

    private Camera m_Camera;
    private Vector3 moveDir;
    private CharacterController controller;
    private Animator animator;
    private void Start()
    {
        m_Camera = Camera.main;
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        Animate();
        CameraMove();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        moveDir = new Vector3(h, 0, v).normalized;
       
        controller.SimpleMove(moveDir * moveSpeed);
    }

    void Rotate()
    {
        if (Player.instance.target != null)
        {
            Vector3 dirToMonster = Player.instance.Direction();
            dirToMonster.y = 0f;

            RotateToQuaternion(dirToMonster);
        }
        // magnitude (상대적으로 느림), sqrMagnitude (빠름)
        else if(moveDir.sqrMagnitude > 0.01f)
        {
            RotateToQuaternion(moveDir);
        }
    }

    void RotateToQuaternion(Vector3 direction)
    {
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 10f * Time.deltaTime);
    }


    void Animate()
    {
        animator.SetFloat("SPEED", moveDir.magnitude);
    }

    void CameraMove()
    {
        m_Camera.transform.position = Vector3.Lerp(
            m_Camera.transform.position,
            transform.position + cameraDir,
            2.0f * Time.deltaTime);
    }
}
