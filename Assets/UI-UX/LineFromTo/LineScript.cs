using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class LineScript : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float width = 1f;

    [SerializeField]
    private Color color = Color.white;

    [Header("Component")]
    [SerializeField]
    private Transform pointFrom = null;

    [SerializeField]
    private Transform pointTo = null;

    //private 
    private SpriteRenderer sprite = null;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        updateValue();
    }

#if UNITY_EDITOR
    private void LateUpdate()
    {
        updateValue();
    }
#endif

    private void updateValue()
    {
        if (pointFrom && pointTo)
        {
            Vector3 from = pointFrom.position;
            Vector3 to = pointTo.position;

            updateSize(from, to);
            updateAngle(from, to);
            updatePosition(from, to);
            setColor(color);
        }
    }

    void updateSize(Vector3 _from, Vector3 _to)
    {
        float length = Vector2.Distance(_from, _to);

        Vector2 size = sprite.size;
        size.x = length;
        size.y = width;

        sprite.size = size;
    }

    void updateAngle(Vector3 _from, Vector3 _to)
    {
        float angle = Vector3.Angle(_to - _from, Vector3.right);
        if (_to.y < _from.y)
        {
            angle = 360 - angle;
        }

        Vector3 vAngle = sprite.transform.eulerAngles;
        vAngle.z = angle;
        sprite.transform.eulerAngles = vAngle;
    }

    void updatePosition(Vector3 _from, Vector3 _to)
    {
        Vector3 newPosition = (_to + _from) / 2;
        transform.position = newPosition;
    }

    public void setColor(Color _color)
    {
        sprite.color = _color;
    }

    public void setPoint(Vector3 _from, Vector3 _to)
    {
        updateSize(_from, _to);
        updateAngle(_from, _to);
        updatePosition(_from, _to);
    }
}