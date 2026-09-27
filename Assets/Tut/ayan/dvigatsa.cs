using UnityEngine;
using UnityEngine.InputSystem;

public class dvigatsa : MonoBehaviour
{
    PlayerInput ayadv;

    InputAction moveAction;

    [SerializeField] float speed = 5f;

    // ДОБАВЛЕНО: ссылка на CharacterController
    // Он нужен вместо прямого transform.position, потому что даёт метод Move(),
    // который учитывает столкновения, и свойство isGrounded (стоит ли игрок на земле)
    CharacterController controller;

    // ДОБАВЛЕНО: сила прыжка (насколько высоко подпрыгивает игрок)
    [SerializeField] float jumpForce = 5f;

    // ДОБАВЛЕНО: сила гравитации, которую будем вручную применять каждый кадр
    [SerializeField] float gravity = -9.81f;

    // ДОБАВЛЕНО: текущая вертикальная скорость (падение/взлёт)
    // Храним отдельно, потому что transform.position += двигает только по X/Z,
    // а по Y нам нужно накапливать скорость (гравитация тянет вниз, прыжок толкает вверх)
    float verticalVelocity;

    // ДОБАВЛЕНО: флаг, чтобы прыжок срабатывал один раз за нажатие, а не каждый кадр пока зажат Space
    // (input.y у тебя = 1 всё время, пока держишь Space, поэтому без этого флага
    // игрок будет "прыгать" непрерывно и фактически взлетит)
    bool jumpPressedLastFrame;

    void Start()
    {
        ayadv = GetComponent<PlayerInput>();

        moveAction = ayadv.actions.FindAction("moce");

        // ДОБАВЛЕНО: получаем CharacterController с этого же объекта
        // (его нужно будет добавить в инспекторе: Add Component -> Character Controller)
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
        Jump(); // ДОБАВЛЕНО: отдельный метод для прыжка/гравитации, вызывается каждый кадр
    }

    void Move()
    {
        Vector3 input = moveAction.ReadValue<Vector3>(); // x = A/D, y = Space/Shift, z = W/S

        Vector3 move = transform.right * input.x + transform.forward * input.z;

        // ИЗМЕНЕНО: было "transform.position += move * speed * Time.deltaTime;"
        // Теперь двигаем через CharacterController.Move(), а не напрямую position,
        // чтобы одновременно применять и горизонтальное движение, и вертикальную скорость (гравитацию/прыжок)
        // move * speed — это движение по земле, Vector3.up * verticalVelocity — это прыжок/падение
        Vector3 finalMove = move * speed + Vector3.up * verticalVelocity;
        controller.Move(finalMove * Time.deltaTime);
    }

    // ДОБАВЛЕНО: логика прыжка и гравитации
    void Jump()
    {
        Vector3 input = moveAction.ReadValue<Vector3>();
        bool jumpHeldNow = input.y > 0f; // Space зажат

        if (controller.isGrounded)
        {
            // ДОБАВЛЕНО: если игрок на земле — сбрасываем вертикальную скорость
            // (небольшое отрицательное значение, а не 0, чтобы isGrounded корректно определялся каждый кадр)
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            // ДОБАВЛЕНО: прыжок срабатывает только в момент нажатия (не было нажато -> стало нажато)
            if (jumpHeldNow && !jumpPressedLastFrame)
            {
                verticalVelocity = jumpForce;
            }
        }
        else
        {
            // ДОБАВЛЕНО: если в воздухе — применяем гравитацию, ускоряя падение со временем
            verticalVelocity += gravity * Time.deltaTime;
        }

        jumpPressedLastFrame = jumpHeldNow;
    }
}
