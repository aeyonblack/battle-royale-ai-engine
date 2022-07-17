using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public enum UpdateModes { Update, LateUpdate, FixedUpdate }

    [Header("Rotation")]
    public bool Rotating = true;
    public Space RotationSpace = Space.Self;
    public UpdateModes UpdateMode = UpdateModes.Update;
    public Vector3 RotationSpeed = new Vector3(100f, 0, 0);

    private void Update()
    {
        if (UpdateMode == UpdateModes.Update)
        {
            Rotate();
        }
    }

    private void FixedUpdate()
    {
        if (UpdateMode == UpdateModes.FixedUpdate)
        {
            Rotate();
        }
    }

    private void LateUpdate()
    {
        if (UpdateMode == UpdateModes.LateUpdate)
        {
            Rotate();
        }
    }

    public virtual void Rotate(bool rotate)
    {
        Rotating = rotate;
    }

    protected virtual void Rotate()
    {
        if (Rotating)
        {
            transform.Rotate(RotationSpeed * Time.deltaTime, RotationSpace);
        }
    }
}
