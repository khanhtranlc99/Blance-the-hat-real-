using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Image))]
public class LineRectScript : MonoBehaviour
{
    [Header("Config")]
    [SerializeField]
    private float width = 1f;
    public float length = 1f;

    [Header("Component")]
    [SerializeField]
    public RectTransform pointFrom = null;

    [SerializeField]
    public RectTransform pointTo = null;

    //private
    private RectTransform _rectTransform = null;

    private static float ratio;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        if (ratio == 0)
        {
            updateRatio();
        }
        updateValue();
    }

//#if UNITY_EDITOR
    private void LateUpdate()
    {
        //updateRatio();
        updateValue();
    }
//#endif

    private void updateValue()
    {
        if (pointFrom && pointTo)
        {
            Vector3 from = pointFrom.position;
            Vector3 to = pointTo.position;

            updateSize(from, to);
            updateAngle(from, to);
            updatePosition(from, to);
        }
    }

    void updateSize(Vector3 _from, Vector3 _to)
    {
        length = ratio * Vector2.Distance(_from, _to);

        Vector2 size = _rectTransform.sizeDelta;
        size.x = length;
        size.y = width;

        _rectTransform.sizeDelta = size;
    }

    void updateAngle(Vector3 _from, Vector3 _to)
    {
        float angle = Vector3.Angle(_to - _from, Vector3.right);
        if (_to.y < _from.y)
        {
            angle = 360 - angle;
        }

        Vector3 vAngle = transform.eulerAngles;
        vAngle.z = angle;
        transform.eulerAngles = vAngle;
    }

    void updatePosition(Vector3 _from, Vector3 _to)
    {
        Vector3 newPosition = (_to + _from) / 2;
        transform.position = newPosition;
    }

    public void setPoint(Vector3 _from, Vector3 _to)
    {
        updateSize(_from, _to);
        updateAngle(_from, _to);
        updatePosition(_from, _to);
    }

    private void updateRatio()
    {
        float heightCanvas = UIManager.uiScreenRect != null ? UIManager.uiScreenRect.size.y : 0;
        float heightCamera = 2 * Camera.main.orthographicSize;
        ratio = heightCanvas / heightCamera;
    }
}