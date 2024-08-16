using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delete : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeleteGameobject());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DeleteGameobject()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
