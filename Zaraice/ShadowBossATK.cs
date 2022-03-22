using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowBossATK : MonoBehaviour
{
    public GameObject Target,sword;
    public bool isatk;
    public Animator am;
    // Start is called before the first frame update

    void Awake()
    {
        Target = GameObject.Find("player"); 
    }

    // Update is called once per frame
    void Update()
    {       
        float offest = Vector3.Distance(transform.position , Target.transform.position);
        Vector3 x = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 a = new Vector3(Target.transform.position.x, 0f, Target.transform.position.z);
        Quaternion targetrot = Quaternion.LookRotation(a-x);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrot, 10f * Time.deltaTime);
       
        if(offest >3 && isatk == false)
        {
         transform.Translate(Vector3.forward *2f* Time.deltaTime);
        }
        else
        {
            sword.SetActive(true);
            isatk = true;
            am.SetTrigger("Atk");
        }         
    }
    public void DestoryShadow()
    {
        Destroy(this.gameObject);
    }
}
