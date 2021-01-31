using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

    
    public GameObject Object;
    public GameObject UIMessage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIMessage.SetActive(true);
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIMessage.SetActive(false);
            Invoke("DestroyTrigger", 5);
        }
        
    }

    void DestroyTrigger()
    {
        UIMessage.SetActive(false);
        Destroy(Object);
    }

  
  
}
