using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public struct WaveInfo
    {
        public int WaveIndex { get; private set; }
        public int EntityCount { get;private set; }
        public GameObject[] Entities { get; private set; }

        //public WaveInfo(int waveIndex, int entityCount, GameObject entity)
        //{
        //    WaveIndex = waveIndex;
        //    EntityCount = entityCount;
        //    Entity = entity;
        //}

        public WaveInfo(int waveIndex, GameObject[] entities)
        {
            WaveIndex = waveIndex;
            EntityCount = entities.Length;
            Entities = entities;
        }
    }
}
