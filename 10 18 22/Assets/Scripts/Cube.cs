using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening; //do tweening kodu tekrarlanan metod icerisine yazilmaz. Coroutine olaraq isleyir. yoxsa 1 saniyede 30 frame(30defe) isleyer.
public class Cube : MonoBehaviour
{

    [SerializeField] float AnimationDuration;
    [SerializeField] Vector3 Pos;
    [SerializeField] float xpos;
    [SerializeField] Ease ease;


    void Start()
    {
        var seq = DOTween.Sequence(); 
       seq.AppendInterval(2);
      
       seq.Append(transform.DOMoveX(5,2));
        seq.AppendInterval(3);              //WaitForSeconds Ermeni versiyasi
       seq.Append(transform.DOMoveX(5,2));


            


        



    }
}