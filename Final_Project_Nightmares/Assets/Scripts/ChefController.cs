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
    private Coroutine chaseCoroutine = null;

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
        transform.rotation = Quaternion.LookRotation(lookDir) ;
        
        if (Vector3.Distance(transform.position, target.transform.position) <= triggerDist)
        {
            // run towards player
            animator.SetBool("isRuning", true);
            AudioManager.SetCrossFade(true);
            if (chaseCoroutine != null) chaseCoroutine = StartCoroutine(ChasePlayer());
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
    IEnumerator ChasePlayer()
    {
        Vector3 lookDir = target.transform.position - transform.position;
        Vector3 startPos = transform.position;
        Vector3 targetPos = target.transform.position;
        float startTime = Time.time;
        float duration = 2f;

        while (Time.time - startTime < duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.Translate(Vector3.Lerp(startPos, targetPos,t));
            yield return null;
        }
        
        chaseCoroutine = null;
    }
    IEnumerator PlayDeadScene()
    {
        GameManager.ShakeTheCamera(.03f, .03f);
        playerController.SetIsAlive(false);
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        deadCoroutine = null;
    }

    // transform.Translate(lookDir * runSpeed * Time.deltaTime);
}
