using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles scene transitions
public class SceneTransition : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void LoadScreen(int levelIndex)
    {
        StartCoroutine(Load(levelIndex));
    }

    IEnumerator Load(int levelIndex)
    {
        anim.SetTrigger("start");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(levelIndex);
    }
}
