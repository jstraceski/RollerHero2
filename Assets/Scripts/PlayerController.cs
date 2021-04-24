using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController cont;
    Vector3 input, mvDirection;

    float speed = 5;
    private float height = 10, gravity = 9.8f, airControl = 2;

    // Start is called before the first frame update
    void Start()
    {
        cont = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float mx = Input.GetAxis("Horizontal");
        float my = Input.GetAxis("Veritcal");

        input = (transform.right * mx + transform.forward * my).normalized;
        input *= speed;
        if (cont.isGrounded)
        {
            mvDirection = input;
            if (Input.GetButton("Jump"))
            {
                mvDirection.y = Mathf.Sqrt(2 * height * gravity);
            }
            else{
                mvDirection.y = 0;
            }
        }
        else
        {
            input.y = mvDirection.y;
            mvDirection = Vector3.Lerp(mvDirection, input, airControl * Time.deltaTime);
        }

        mvDirection.y -= gravity * Time.deltaTime;
        cont.Move(mvDirection * Time.deltaTime );
    }
}
