using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobSpwaner : MonoBehaviour
{
    // Public - Inspector
    public GameObject Mob;
    public List<Transform> Spawn;
    public UnityEngine.Experimental.Rendering.LWRP.Light2D Light;
    public int numberMobTotal = 0;
    public int numberMaxAlive = 0;
    public float area = 1;


    private Transform transform;

    private List<UnityEngine.Experimental.Rendering.LWRP.Light2D> newLights = new List<UnityEngine.Experimental.Rendering.LWRP.Light2D>();
    private List<NPC> listeMob = new List<NPC>();
    private int numberMobSpawn = 0;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        numberMobTotal = Mathf.Clamp(numberMobTotal, 0, 100);
        numberMaxAlive = Mathf.Clamp(numberMaxAlive, 0, numberMobTotal);
        //StartSpawn();
        //CreateLight();
    }

    
    // Update is called once per frame
    void Update()
    {
        RemoveDeadMob();
        if (numberMobSpawn < numberMobTotal && listeMob.Count < numberMaxAlive)
        {
            CreateMob();
            numberMobSpawn++;
        }

        /*
        foreach (UnityEngine.Experimental.Rendering.LWRP.Light2D light in newLights)
        {
            AnimationLight(light);
        }

        foreach (NPC mob in listeMob)
        {
            AnimationSpawn(mob);
        }*/
        
    }


    void StartSpawn()
    {
        for(int i=0; i<numberMobTotal; i++)
        {
            CreateMob();
        }
    }

    void CreateMob()
    {
        GameObject newMob = Instantiate(Mob, RandomPosition(), Quaternion.identity);
        NPC npc = newMob.GetComponentInChildren<NPC>();
        npc.StartFading();
        listeMob.Add(npc);

        /*SpriteRenderer spriteR = newMob.GetComponentInChildren<SpriteRenderer>();
        Color co = spriteR.material.color;
        co.a = 0f;
        spriteR.material.color = co;*/
        //listeMob.Add(newMob);

    }

    Vector3 RandomPosition()
    {
        int i = Random.Range(0, Spawn.Count);
        Transform m_transform = Spawn[i].GetComponent<Transform>();
        float x = Random.Range(m_transform.position.x - area, m_transform.position.x + area);
        float y = Random.Range(m_transform.position.y - area, m_transform.position.y + area);
        return new Vector3(x, y, 0);
    }

    void RemoveDeadMob()
    {
        foreach (NPC el in listeMob)
        {
            if (!el.IsAlive())
            {
                listeMob.Remove(el);
                return;
            }
        }
    }

    /*
    void CreateLight()
    {
        UnityEngine.Experimental.Rendering.LWRP.Light2D newLight = Instantiate(Light, transform.position, transform.rotation);
        newLight.transform.RotateAround(Vector3.zero, Vector3.right, 53);
        newLight.intensity = 0f;
        newLight.radius = 0.2f;
        newLight.color = Color.red;
        newLight.volumeOpacity = 0.13f;
        newLights.Add(newLight);
    }

    void AnimationLight(UnityEngine.Experimental.Rendering.LWRP.Light2D light)
    {
        if (light.intensity <= 0.35f) light.intensity += 0.02f;
        else if (light.radius <= 0.7) light.radius += 0.01f;
        else AnimationSpawn(listeMob[newLights.IndexOf(light)]);
    }

    void AnimationSpawn(NPC mob)
    {
        SpriteRenderer spriteRenderer = mob.GetComponentInChildren<SpriteRenderer>();
        Color MobColor = spriteRenderer.material.color;
        if (MobColor.a <= 1f)
        {
            Color c = spriteRenderer.material.color;
            c.a = MobColor.a + 0.1f;
            spriteRenderer.material.color = c;
        }
    }*/



    
}
