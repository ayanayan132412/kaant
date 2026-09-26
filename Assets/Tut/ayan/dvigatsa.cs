using UnityEngine;
using UnityEngine.InputSystem;

public class dvigatsa : MonoBehaviour
{
    PlayerInput ayadv;

    InputAction moveAction;

    [SerializeField] float speed = 5f;

    void Start()
    {
        ayadv = GetComponent<PlayerInput>();

        moveAction = ayadv.actions.FindAction("moce");

    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 input = moveAction.ReadValue<Vector3>(); // x = A/D, y = W/S

        Vector3 move = transform.right * input.x + transform.forward * input.z;
        transform.position += move * speed * Time.deltaTime;
        //Vector3 input = moveAction.ReadValue<Vector3>();
        //Vector3 movement = input * speed * Time.deltaTime;
        //transform.position = transform.position + movement;
    }
}
