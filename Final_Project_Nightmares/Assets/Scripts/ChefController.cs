using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChefController : MonoBehaviour
{
    public GameObject target;
    public float runSpeed;
    public Animator animator;
    public float triggerDist;
    public float deadDist;
    public PlayerController playerController;
    private Coroutine deadCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //follow six
        FollowTarget();

        //Chef got six
        GotTarget();

        // shake camera

    }

    private void FollowTarget()
    {
        // rotation of chef
        Vector3 lookDir = target.transform.position - transform.position;
        transform.rotation = new Quaternion(lookDir.x,lookDir.y, lookDir.z, 0f) ;

        if (Vector3.Distance(transform.position, target.transform.position) <= triggerDist)
        {
            // run towards player
            animator.SetBool("isRuning", true);
            transform.Translate(lookDir * runSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("isRuning", false);
        }
    }

    private void GotTarget()
    {
        if (Vector3.Distance(transform.position, target.transform.position) <= deadDist)
        {
            if (deadCoroutine == null)
            {
                deadCoroutine = StartCoroutine(PlayDeadScene());
            }
            
        }

    }

    IEnumerator PlayDeadScene()
    {
        GameManager.ShakeTheCamera(.03f, .03f);
        playerController.SetIsAlive(false);
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
