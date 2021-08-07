using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Confetti : MonoBehaviour
{
    //velocidade de queda
    private float speed = 900f;

    private void Update()
    {
        //movimentação dos confetes
        transform.position += Vector3.down * speed * Time.deltaTime;
    }

    public void OnEnable()
    {
        //destruição do objeto
        Destroy(this.gameObject, 3f);
    }
}
