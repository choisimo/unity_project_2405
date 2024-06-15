using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//짐칸 랜덤 생성 스크립트
public class WagonGenerator: MonoBehaviour
{
    public GameObject[] Wagons;
    public GameObject settingPoint;


    
    void Start()
    {
        SpawnRandomWagon();
    }


    void SpawnRandomWagon()
    {
        // 랜덤 인덱스 선택
        int randomIndex = 1;//Random.Range(0, Wagons.Length);

        // 선택된 프리팹 생성
        GameObject NewWagon = Instantiate(Wagons[1/*randomIndex*/], transform.position, transform.rotation);

        Wagon wagonComponent = NewWagon.GetComponent<Wagon>();

        SetWagon setWagonScript = settingPoint.GetComponent<SetWagon>();
        if (setWagonScript != null && wagonComponent != null)
        {
            setWagonScript.Initialize(wagonComponent);
        }

        StaticVal.Instance.SetRandomIndex(randomIndex);

    }

}
