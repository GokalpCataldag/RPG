using UnityEngine;

public class PlayerOnClick : MonoBehaviour
{
    public float maxSpeed = 5f;
    public float turnSpeed = 15f;
    public float attackRange = 2f;

    private Animator anim;
    private CharacterController Controller;
    private CollisionFlags collisionFlags = CollisionFlags.None;

    private Vector3 playerMove= Vector3.zero;
    private Vector3 targetMovePoint = Vector3.zero;
    private Vector3 targetAttackPoint = Vector3.zero;

    private float currentSpeed;
    private float playerTopPointDistance; //tıkladığımız yerle olan mesafe
    private float gravity = 9.8f;
    private float height;

    private bool canAttackMove;
    private bool canMove; // hareket edip edemeyeceğimize 
    private bool finishedMovement = true;
    private Vector3 NewMovepoint;
    private Vector3 NewAttackPoint;

    private GameObject enemy;
    private void Awake()
    {
        anim= GetComponent<Animator>();
        Controller = GetComponent<CharacterController>();
        currentSpeed = maxSpeed;
    }
    

    void Update()
    {
        CalculateHeight();
        CheckIfFinishedMovement();
        AttackMove();
    }
    bool IsGrounded()
    {
        return collisionFlags == CollisionFlags.CollidedBelow ? true : false;
    }
    void AttackMove()
    {
        if (canAttackMove)
        {
            targetAttackPoint = enemy.gameObject.transform.position;
            NewAttackPoint = new Vector3(targetAttackPoint.x,transform.position.y,targetAttackPoint.z);
        }
        if(!anim.IsInTransition(0)&& anim.GetCurrentAnimatorStateInfo(0).IsName("Basic Attack"))
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NewAttackPoint - transform.position), turnSpeed *2* Time.deltaTime);
        }
    }
    void CalculateHeight()
    {
        if (IsGrounded())
        {
            height = 0f;
        }
        else
        {
            height -= gravity*Time.deltaTime;
        }
    }

    void CheckIfFinishedMovement()
    {
        if (!finishedMovement)
        {
            if (!anim.IsInTransition(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Great Sword Idle") && anim.GetCurrentAnimatorStateInfo(0).normalizedTime>=0.8f)
            {
                finishedMovement = true;
            }
        }
        else
        {
            MovePlayer();
            playerMove.y = height*Time.deltaTime;
            collisionFlags = Controller.Move(playerMove);
        }
    }
    void MovePlayer()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray= Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray,out hit))
            {
                playerTopPointDistance= Vector3.Distance(transform.position,hit.point);
                if (hit.collider.gameObject.layer==LayerMask.NameToLayer("Ground"))
                {
                    if(playerTopPointDistance >= 1.0f)
                    {
                        canMove = true;
                        canAttackMove = false;
                        targetMovePoint = hit.point;
                    }
                }
                else if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
                {
                    if (playerTopPointDistance >= 1.0f)
                    {
                        enemy = hit.collider.gameObject.GetComponentInParent<EnemyWayPointTracker>().gameObject;
                        canMove = true;
                        canAttackMove = true;
                        targetMovePoint = hit.point;
                    }
                }
            }
        }
        if (canMove)
        {
            anim.SetFloat("Speed", 1.0f);
            
            if (!canAttackMove)
            {
                NewMovepoint = new Vector3(targetMovePoint.x, transform.position.y, targetMovePoint.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NewMovepoint - transform.position), turnSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(NewAttackPoint - transform.position), turnSpeed * Time.deltaTime);
            }
            
            playerMove=transform.forward*currentSpeed*Time.deltaTime;

            if (Vector3.Distance(transform.position,NewMovepoint) <= 0.6f && !canAttackMove)
            {
                canMove = false;
                canAttackMove = false;
            }
            else if (canAttackMove)
            {
                if (Vector3.Distance(transform.position,NewAttackPoint) <= attackRange)
                {
                    playerMove.Set(0f, 0f, 0f);
                    anim.SetFloat("Speed", 0f);
                    targetAttackPoint = Vector3.zero;
                    anim.SetTrigger("AttackMove");
                    canAttackMove= false;
                    canMove = false;
                }
            }
        }
        else
        {
            playerMove.Set(0f, 0f, 0f);
            anim.SetFloat("Speed", 0f);
        }
    }

    public bool FinishedMovement 
    {
        get
        {
            return finishedMovement;
        }

        set
        {
            finishedMovement = value;
        }
    }

    public bool CanMove
    {
        get { return canMove; }
        set { CanMove = value; }
    }


    public Vector3 TargetPosition
    {
        get
        {
            return targetMovePoint;
        }
        set 
        {
            targetMovePoint = value; 
        }
    }
}
