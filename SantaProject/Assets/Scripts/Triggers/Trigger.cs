using UnityEngine;


public class Trigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Triggered();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        UnTriggered();
    }

    protected virtual void Triggered()
    {
        Debug.Log("Triggered!");
    }

    protected virtual void UnTriggered()
    {
        Debug.Log("UnTriggered!");
    }

}
