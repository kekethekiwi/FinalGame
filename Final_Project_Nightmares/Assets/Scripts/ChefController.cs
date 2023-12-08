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
    private Coroutine deadCoroutine = null;
    private bool crossFadedAlready = false;

    // Update is called once per frame
    void Update()
    {
        //follow six
        FollowTarget();

        //Chef got six
        GotTarget();


    }

    private void FollowTarget()
    {
        // rotation of chef
        Vector3 lookDir = target.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(lookDir) ;
        
        if (Vector3.Distance(transform.position, target.transform.position) <= triggerDist)
        {
            // run towards player and cross fade music
            animator.SetBool("isRuning", true);
            ManageCrossFade();
            ChasePlayer();
        }
        else
        {
            animator.SetBool("isRuning", false);
        }
    }

    private void ManageCrossFade()
    {
        if (!crossFadedAlready)
        {
            AudioManager.SetCrossFade(true); 
            crossFadedAlready = true;
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

    private void ChasePlayer()
    {
        Vector3 lookDir = (target.transform.position - transform.position);
        lookDir.Normalize();
        transform.Translate(lookDir * runSpeed * Time.deltaTime);
    }
    
    IEnumerator PlayDeadScene()
    {
        GameManager.ShakeTheCamera(.06f, .06f);
        PlayerController.SetIsAlive(false);
        yield return new WaitForSeconds(2.5f);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        deadCoroutine = null;
    }


    // IGNORE 
    // transform.Translate(lookDir * runSpeed * Time.deltaTime);
    //transform.Translate(Vector3.Lerp(startPos, targetPos,t));
    //transform.position = Vector3.MoveTowards(startPos, targetPos, t* runSpeed * Time.deltaTime);
    //IEnumerator ChasePlayerCoroutine()
    //{
    //    Vector3 lookDir = target.transform.position - transform.position;
    //    Vector3 startPos = transform.position;
    //    Vector3 targetPos = target.transform.position;
    //    float startTime = Time.time;
    //    float duration = .8f;

    //    while (Time.time - startTime < duration)
    //    {
    //        float t = (Time.time - startTime) / duration;
    //        transform.Translate(Vector3.Lerp(startPos, targetPos, t));
    //        yield return null;
    //    }

    //    transform.position = targetPos;

    //    chaseCoroutine = null;
    //}
    //if (chaseCoroutine == null) chaseCoroutine = StartCoroutine(ChasePlayerCoroutine());

}
