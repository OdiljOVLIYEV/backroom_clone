using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public string targetTag = "Table"; // Stol obyektiga qo‘yilgan tag

    void Start()
    {
        GameObject targetObj = GameObject.FindWithTag(targetTag);
        if (targetObj == null)
        {
            Debug.LogWarning("Target with tag '" + targetTag + "' not found!");
            return;
        }

        Vector3 direction = targetObj.transform.position - transform.position;
        direction.y = 0; // Faqat gorizontal yo'nalishda qarash

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}