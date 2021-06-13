using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoldOutAnimator : MonoBehaviour
{
    [SerializeField] List<Animator> foldOutAnimator;

    int[] indices =
    {
        1,
        2,
        3,
        4,
        5,
        6
    };

    public void SetIndex()
    {
        List<int> indicesRandom = new List<int>(indices);
        int index;
        int indexRand;
        foreach (Animator animator in foldOutAnimator)
        {
            indexRand = Random.Range(0, indicesRandom.Count);
            index = indicesRandom[indexRand];
            indicesRandom.RemoveAt(indexRand);

            animator.SetInteger("index", index);
        }
    }

    public void SetDirection(bool vertical = false, bool horizontal = false)
    {
        foreach (Animator animator in foldOutAnimator)
        {
            animator.SetBool("Horizontal", horizontal);
            animator.SetBool("Vertical", vertical);
        }
    }
}
