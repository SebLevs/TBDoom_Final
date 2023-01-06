using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSeed
{
    private int seed;
    private System.Random random;

    public int Seed { get => seed; set => seed = value; }
    public System.Random Random { get => random; set => random = value; }

    public RandomSeed()
    {
        seed = UnityEngine.Random.Range(0, 1000000);
        random = new System.Random(seed);
    }
}
