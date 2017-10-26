using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour
{
    
    void Start()
    {
        
    }

    void StoreToCasino()
    {
        gameObject.GetComponent<Animator>().SetBool("st_c",true);
    }
    void CasinoToStore()
    {
        gameObject.GetComponent<Animator>().SetBool("c_st", true);
    }
}
