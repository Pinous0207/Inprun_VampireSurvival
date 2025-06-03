using System.Collections;
using UnityEngine;

public class MANAGER : MonoBehaviour
{
    public static MANAGER instance = null;
    public static Pool_Mng POOL;
    public static Database_Mng DB;
    public static Session_Mng SESSION;
    public static Skill_Mng SKILL;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        POOL = GetComponentInChildren<Pool_Mng>();
        DB = GetComponentInChildren<Database_Mng>();
        SESSION = GetComponentInChildren<Session_Mng>();
        SKILL = GetComponentInChildren<Skill_Mng>();
    }

    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
}
