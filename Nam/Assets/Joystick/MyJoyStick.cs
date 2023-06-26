using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class MyJoyStick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    // ** 움직일 대상
    [SerializeField] private Transform Target;

    // ** 실제 움직일 조이스틱
    [SerializeField] private RectTransform Stick;

    // ** 조이스틱뒷 배경 (스틱이 움직일 수 있는 최대반경으로 사용)
    [SerializeField] private RectTransform BackBoard;

    [Header("Look at")]
    [Tooltip("Look at the direction of the joystick")]
    // ** Target이 조이스틱이 가르키는 방향을 바라볼지 결정함.
    [SerializeField] private bool Look = true;

    [Header("2D and 3D")]
    [Tooltip("Horizontal & Vertical")]
    // ** 평면에서 움직이는 상태인지, 수직으로 움직이는 상태인지 결정.
    [SerializeField] private bool HorizontalSpace = true;

    // ** 반지름(조이스틱이 움직일 수 있는 최대 반경을 구하기 위함.)
    private float Radius;

    // ** TouchCheck 입력 확인.
    private bool TouchCheck = false;

    // ** 이동속도.
    private float Speed = 5.0f;

    // ** 방향값
    private Vector2 Direction;

    // ** Direction * Speed * Time.deltaTime;
    // ** 얼마 만큼 움직여야할지에 대한 값.
    private Vector3 Movement;


    void Awake()
    {
        /*
            BackBoard 생성 -> [OutLineCircle.png]
            BackBoard 하위에 Stick 생성 -> [FilledCircle.png]
         */

        RectTransform rectTransPos = transform.gameObject.AddComponent<RectTransform>();

        BackBoard = transform.Find("BackBoard").GetComponent<RectTransform>();
        Stick = transform.Find("BackBoard/Stick").GetComponent<RectTransform>();

        Target = GameObject.Find("Player").transform;
    }


    void Start()
    {
        // ** BackBoard의 반지름을 구함.
        Radius = BackBoard.rect.width / 2;
    }

    void Update()
    {
        if (TouchCheck)
            Target.position += Movement;
    }

    void OnTouch(Vector2 vecTouch)
    {
        Stick.localPosition = new Vector2(vecTouch.x - BackBoard.position.x, vecTouch.y - BackBoard.position.y);

        // ** ClampMagnitude 함수는 Vector의 길이를 제한한다.
        // ** Stick.localPosition의 값이 Radius보다 커지지 않도록 하게함.
        Stick.localPosition = Vector2.ClampMagnitude(Stick.localPosition, Radius);

        // ** 스틱이 배경의 중심으로 부터 얼마 만큼의 비율로 움직였는지 확인.
        // ** 스틱이 배경에서 움직인 비율.
        // ** Ratio = 스틱이 움직인 비율.
        float Ratio = (BackBoard.position - Stick.position).sqrMagnitude / (Radius * Radius);

        // ** 스틱이 가르키는 방향을 구함.
        Direction = Stick.localPosition.normalized;

        // ** Target를 움직일 방향과 속도를 구함.
        if(HorizontalSpace)
        {
            // ** Horizontal Space
            Movement = new Vector3(
             Direction.x * Speed * Time.deltaTime * Ratio,
             0f,
             Direction.y * Speed * Time.deltaTime * Ratio);

            // ** 타겟이 Direction과 같은 방향으을 바라보게함.
            if (Look)
                Target.eulerAngles = new Vector3(0f, Mathf.Atan2(Direction.x, Direction.y) * Mathf.Rad2Deg, 0f);
        }
        else
        {
            // ** Vertical Space
            Movement = new Vector3(
            Direction.x * Speed * Time.deltaTime * Ratio,
            Direction.y * Speed * Time.deltaTime * Ratio,
            0f);
        }
    }




    /****************************************************
     * 클릭 또는 터치
     * nDrag      : 버튼이 눌려지고 있는 동안 계속 실행됨
     * OnPointerUp      : 버튼이 눌리는 순간 실행됨
     * OnPointerDown    : 버튼을 떼는 순간 실행됨
     * 마라메터 : eventData = 위치를 나타냄.
     * 설명
     * 현재 스크립트가 놓인 위치에서만 작동하며, 현재 
     * 스크립트는 조이스틱을 작동시키기 위해 만들어진 
     * 스크립트이다. 조이스틱은 Canvas하위에 만들어질 것이며,
     * 현재 스크립트를 Canvas에 붙혀놓으면 Canvas하위에 놓인
     * UI에서만 동작하게 된다.
    ****************************************************/
    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.ToString());
        OnTouch(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(eventData.ToString());
        OnTouch(eventData.position);
        TouchCheck = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(eventData.ToString());
        Stick.localPosition = Vector2.zero;
        TouchCheck = false;
    }
}
