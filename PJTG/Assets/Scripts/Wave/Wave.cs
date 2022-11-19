using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Wave class used to describe an enemy wave
public class Wave {
    // The time stamp when the wave starts
    public int TimeStamp {get; set;}
    // Type of enemies in this wave
    public EnemyType[] Enemies {get; set;}
    // Is this a boss wave
    public bool IsBossWave {get; set;}
    // Max percentage
    public int MaxPercentage {get; set;}
    public bool HasBossSpawned {get; set;}    

    public Wave(int timeStamp, EnemyType[] enemies, bool isBossWave) {
        this.TimeStamp = timeStamp;
        this.Enemies = enemies;
        this.IsBossWave = isBossWave;
        this.HasBossSpawned = false;
        for(int i = 0; i < this.Enemies.Length; i++) {
            this.MaxPercentage += (int)this.Enemies[i];
        }
    }

    // Return an enemy type
    public int GetEnemyType(int random) {
        int currentIndex = 0;
        int currentPercentage = 0;
        for(int i = 0; i < this.Enemies.Length; i++) {
            currentPercentage += (int)this.Enemies[i];
            if(random < currentPercentage) {
                currentIndex = i;
                break;
            }
        }
        return currentIndex;
    }
}
