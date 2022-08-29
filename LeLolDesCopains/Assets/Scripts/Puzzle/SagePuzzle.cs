using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SagePuzzle : MonoBehaviour, Iinteractable
{
    [SerializeField] private List<GameObject> soclesBase;
    [SerializeField] private List<GameObject> platesBase;

    [SerializeField] private Door doorToOpen;

    [SerializeField] private Queue<GameObject> soclesQueue;
    [SerializeField] private Queue<GameObject> platesQueue;

    private bool doorIsOpen = false;

    private void Start()
    {
        ResetPuzzle();
    }

    public GameObject Interact(GameObject sender)
    {
        if (doorIsOpen) return null;

        if (sender.Equals(platesQueue.Peek()))
        {
            soclesQueue.Dequeue().GetComponent<MovableSocle>().MoveToTarget(false);
            platesQueue.Dequeue();

            if (soclesQueue.Count <= 0)
            {
                doorToOpen.Interact(this.gameObject);
                doorIsOpen = true;
            }
        }
        else ResetPuzzle();

        return this.gameObject;
    }

    private void ResetPuzzle()
    {
        RePopulateQueue(soclesBase, ref soclesQueue);
        RePopulateQueue(platesBase, ref platesQueue);

        foreach (GameObject socle in soclesBase)
        {
            socle.GetComponent<MovableSocle>().MoveToTarget(true);
        }
    }

    private void RePopulateQueue(List<GameObject> baseList, ref Queue<GameObject> queue)
    {
        queue = new Queue<GameObject>();
        foreach (GameObject item in baseList)
        {
            queue.Enqueue(item);
        }
    }

    public void SetOutlineActive(bool active)
    {
        throw new System.NotImplementedException();
    }
}
