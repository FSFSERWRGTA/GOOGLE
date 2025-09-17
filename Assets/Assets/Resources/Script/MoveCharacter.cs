using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MoveCharacter : MonoBehaviour
{
    public float speed = 2f;
    public LayerMask wallLayer;
    
    public GameManager manager;

    private Rigidbody2D rigidBody2D;
    private Animator animator;

    private Vector2 moveInput;
    private Vector2 lastDirection;

    private Vector3 dirVec;
    GameObject scanObject;

    private Animator anim;
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        lastDirection = Vector2.down;
    }

    void Update()
    {
        moveInput = Vector2.zero;

        if (manager.isAction ? false : Input.GetKey(KeyCode.UpArrow)) //상
        {
            moveInput.y += 1;
            dirVec = Vector3.up;
        }
        if (manager.isAction ? false : Input.GetKey(KeyCode.DownArrow)) //하
        {
            moveInput.y -= 1;
            dirVec = Vector3.down;
        }
        if (manager.isAction ? false : Input.GetKey(KeyCode.LeftArrow)) //좌
        {
            moveInput.x -= 1;
            dirVec = Vector3.left;
        }
        if (manager.isAction ? false : Input.GetKey(KeyCode.RightArrow)) //우
        {
            moveInput.x += 1;
            dirVec = Vector3.right;
        }

        if (moveInput != Vector2.zero)
        {
            lastDirection = moveInput.normalized;
        }

        animator.SetFloat("DirX", lastDirection.x);
        animator.SetFloat("DirY", lastDirection.y);
        
        //Scan Object
        if (Input.GetButtonDown("Jump") && scanObject is not null)
        {
            manager.Action(scanObject);
        }
        

    }

    void FixedUpdate()
    {
        Vector2 moveDelta = moveInput.normalized * (speed * Time.fixedDeltaTime);
        Vector2 newPos = rigidBody2D.position + moveDelta;

        RaycastHit2D hit = Physics2D.Linecast(rigidBody2D.position, newPos, wallLayer);

        if (hit.collider == null)
        {
            rigidBody2D.MovePosition(newPos);
            animator.SetBool("Walking", moveInput != Vector2.zero);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
        
        //ray
        Debug.DrawRay(rigid.position, dirVec*0.7f, new Color(0,1,0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, dirVec, 0.7f, LayerMask.GetMask("Object"));
        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }
}