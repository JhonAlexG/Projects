using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    private Animator animator;
    public DoorEndController doorEndController;
    public int sceneIndex;

    [SerializeField] private AnimationClip transitionAnimation;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doorEndController.isInsideDoor)
        {
            StartCoroutine(Transition());
        }
    }

    IEnumerator Transition()
    {
        animator.SetTrigger("Start");

        yield return new WaitForSeconds(transitionAnimation.length);

        SceneManager.LoadScene(sceneIndex);
    }
}
