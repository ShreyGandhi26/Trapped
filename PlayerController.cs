using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    float xInput;
    float zInput;

    [SerializeField]
    GameObject deadPanel;

    public float moveSpeed;

    public float health;

    public GameObject cam;

    public GameObject playerObj;

    public GameObject bulletSpawnPoint;
    public float waitTime;
    public float SceneLoadTime;
    public GameObject Bullet;
    float deadTime = 0f;

    private bool CanShoot = false;
    public float Timer = 0;

    private Transform BulletSpawned;

    public static bool Interacted = false;

    private Animator anim;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Interacted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0)
        {
            Timer += Time.deltaTime;
            if (Timer >= waitTime)
            {
                CanShoot = true;
            }

            Plane playerplane = new Plane(Vector3.up, transform.position);
            Ray ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitDist = 0.0f;

            if (playerplane.Raycast(ray, out hitDist))
            {
                Vector3 targetPoint = ray.GetPoint(hitDist);
                Quaternion targetRot = Quaternion.LookRotation(targetPoint - transform.position);
                targetRot.x = 0;
                targetRot.z = 0;
                playerObj.transform.rotation = Quaternion.Slerp(playerObj.transform.rotation, targetRot, 7f * Time.deltaTime);
            }

            Move();

            if (Input.GetMouseButtonDown(0) && CanShoot == true)
            {
                Shoot();
            }

            if (Generator.canInteract == true)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    anim.SetBool("Interact", Generator.canInteract);
                    Interacted = true;
                }
            }
            if (Generator.canInteract == false)
            {
                anim.SetBool("Interact", Generator.canInteract);
            }


        }
        else
        {
            deadTime += Time.deltaTime;
            anim.SetBool("IsDead", true);
            Destroy(rb);
            Destroy(GetComponent<Collider>());
            deadPanel.SetActive(true);
            if (SceneLoadTime <= deadTime)
            {
                deadPanel.SetActive(false);
                SceneManager.LoadScene("Level 1");
            }
        }
    }

    private void Move()
    {
        xInput = Input.GetAxis("Horizontal");
        zInput = Input.GetAxis("Vertical");

        rb.velocity = new Vector3(xInput, rb.velocity.y, zInput) * moveSpeed;
        if (xInput == 0 && zInput == 0)
        {
            anim.SetBool("Running", false);
        }
        else
        {
            anim.SetBool("Running", true);
        }
    }

    private void Shoot()
    {
        BulletSpawned = Instantiate(Bullet.transform, bulletSpawnPoint.transform.position, Quaternion.identity);
        BulletSpawned.rotation = bulletSpawnPoint.transform.rotation;
        CanShoot = false;
        Timer = 0;
    }
}
