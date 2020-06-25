using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeMap : MonoBehaviour
{
    public int[,] map = {
        {0,0,0,0,0,0,0,0,0 }, 
        {0,0,0,0,0,0,0,0,0 }, 
        {1,1,1,1,0,0,0,0,1 }, 
        {0,0,0,0,0,0,0,1,1 }, 
        {1,0,0,0,0,0,1,0,1 },
        {1,0,0,1,1,1,0,0,1 },
        {1,0,0,0,0,0,0,0,1 },
        {1,0,0,0,0,0,0,0,1 },
        {1,1,1,1,1,1,1,1,1 }
    };

    public bool isDebug;

    public Dictionary<int, string> codeToBlock = new Dictionary<int, string>() 
    {
        {1, "Ground"}
    }
    ;
    
    // Start is called before the first frame update
    void Start()
    {
        isDebug = GameObject.Find("Player").GetComponent<PlayerMove>().isDebug;
        int midX = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(map.GetLength(0) / 2)));
        int midY = Convert.ToInt32(Math.Ceiling(Convert.ToDouble((map.Length / map.GetLength(0)) / 2)));
        if (isDebug)
        {
            Debug.Log(midX.ToString() + midY.ToString());
        }
        for (int x=0; x<map.GetLength(0); x++)
        {
            for (int y = 0; y < map.Length / map.GetLength(0); y++)
            {
                int nowCode = map[y, x];
                if (nowCode != 0)
                {
                    GameObject map = GameObject.Find(codeToBlock[nowCode]);
                    Instantiate(map, new Vector3((x - midX)*1.28F, (y - midY) * -1.28F, 0), new Quaternion(0,0,0,0), GameObject.Find("Maps").GetComponent<Transform>());
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
