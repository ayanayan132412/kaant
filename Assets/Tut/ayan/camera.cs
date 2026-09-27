using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonLook : MonoBehaviour
{
    public Transform playerBody;

    // ИЗМЕНЕНО: было "public float sensitivity", стало "private" + [SerializeField]
    // [SerializeField] говорит Unity: "это приватное поле, но всё равно покажи его в инспекторе"
    // Так поле остаётся видимым и редактируемым в редакторе, но недоступным напрямую из других скриптов
    [SerializeField] private float sensitivity = 0.1f; // с новым Input System чувствительность обычно в разы меньше

    float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * sensitivity;
        float mouseY = mouseDelta.y * sensitivity;

        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }

    // ДОБАВЛЕНО: метод для изменения чувствительности из кода (из другого скрипта, UI-слайдера и т.д.)
    // Так как поле теперь private, это единственный способ поменять sensitivity извне
    public void SetSensitivity(float value)
    {
        sensitivity = value;
    }
}
