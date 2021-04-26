using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public Vector3[] CreateBoatPositions()
    {
        Vector3[] positions = new Vector3[10];
        int count = 0;
        int[,] matrix = new int[10, 10];
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                matrix[i, j] = 0;
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                bool failed = true;
                while (failed)
                {
                    int a = (int) Mathf.Clamp(Random.value * 10, 0f, 9f);
                    int b = (int) Mathf.Clamp(Random.value * 10, 0f, 9f);
                    int r = (int) Random.value * 4;

                    failed = false;
                    int aCheck = a + (3 - i) * (int) Math.Sin(Mathf.Deg2Rad * 90 * r);
                    int bCheck = b + (3 - i) * (int) Math.Cos(Mathf.Deg2Rad * 90 * r);
                    if (aCheck < 0 || aCheck > 9 || bCheck < 0 || bCheck > 9)
                    {
                        failed = true;
                    }
                    else
                    {
                        for (int k = 0; k < 4 - i; k++)
                        {
                            if (matrix[a + k * (int)Math.Sin(Mathf.Deg2Rad*90*r), b + k * (int)Math.Cos(Mathf.Deg2Rad*90*r)] == 1)
                            {
                                failed = true;
                                break;
                            }
                        }

                        if (!failed)
                        {
                            positions[count++] = new Vector3(a + 0.5f, r, b + 0.5f);
                            for (int k = 0; k < 4 - i; k++)
                            {
                                matrix[a + k * (int) Math.Sin(Mathf.Deg2Rad * 90 * r),
                                    b + k * (int) Math.Cos(Mathf.Deg2Rad * 90 * r)] = 1;
                            }
                        }
                    }
                }
            }
        }

        return positions;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
