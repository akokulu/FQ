using UnityEngine;
using System.Collections;

public class GridMotor : MonoBehaviour
{
    // Use these to control the motor
    public class MotorInput
    {
        public Vector2 direction = Vector2.zero; // this must be normalized

        public bool isRunning = false;
    }

    public MotorInput input = new MotorInput();

    public float walkSpeed = 1.0f;
    public float runSpeed = 2.0f;
    public float gridSize = 1.0f;
    public bool allowDiagonal = true;

    public EaseType easing = EaseType.Linear;

    public bool isMoving = false;

    void Update()
    {
        if (!isMoving && !Mathf.Approximately(input.direction.magnitude, 0f))
        {
            Vector3 moveDirection = new Vector3(input.direction.x, 0, input.direction.y);
            if (allowDiagonal)
            {
                float angle = Mathf.Sign(moveDirection.x) * Vector3.Angle(Vector3.forward, moveDirection);
                float snappedAngle = 45f * Mathf.Round(angle / 45f);

                Quaternion q = Quaternion.Euler(0f, snappedAngle, 0f);
                moveDirection = q * Vector3.forward * input.direction.magnitude;
            }
            else
            {
                if (Mathf.Abs(input.direction.x) > Mathf.Abs(input.direction.y))
                    moveDirection.y = 0;
                else
                    moveDirection.x = 0;
            }

            Vector3 moveOffset = new Vector3(0.5f > Mathf.Abs(moveDirection.x) ? 0.0f : Mathf.Sign(moveDirection.x) * gridSize,
                                             0,
                                             0.5f > Mathf.Abs(moveDirection.z) ? 0.0f : Mathf.Sign(moveDirection.z) * gridSize);

            if (0.5f < moveOffset.magnitude && !Physics.Raycast(transform.position + 0.5f * Vector3.up, moveOffset, gridSize))
            {
                isMoving = true;
                iTween.MoveBy(gameObject,
                    iTween.Hash(
                        "amount", moveOffset,
                        "space", "world",
                        "orienttopath", true,
                        "speed", input.isRunning ? runSpeed : walkSpeed,
                        "oncomplete", "OnMoveComplete",
                        "oncompletetarget", gameObject,
                        "easetype", iTweenX.Ease(easing)
                        ));
            }
            else if (gameObject.name == "Player")
            {
                // Allows rotation when stuck
                gameObject.transform.LookAt(transform.position + moveDirection);
            }
        }
    }

    void OnMoveComplete()
    {
        isMoving = false;
    }

    public GUIText dialog;

    void OnTriggerEnter(Collider c)
    {
        if (c.transform.name == "Dungeon")
        {
            Application.LoadLevel("test-scene");
        }
        else if (c.transform.name == "Exit")
        {
            Application.LoadLevel("level switch");
        }
    }


    private IEnumerator RemoveDialog(int delay)
    {
        yield return new WaitForSeconds(delay);
        dialog.text = "";
    }
}
