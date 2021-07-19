using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RimWorld;
using UnityEngine;

using Verse;

namespace VanillaSushiExpanded
{
    public class Plant_NeedsWater: Plant
    {

        const int radius = 6;
        int numberOfWater = 0;


        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            int num = GenRadial.NumCellsInRadius(radius);
            for (int i = 0; i < num; i++)
            {
                IntVec3 c = this.Position + GenRadial.RadialPattern[i];
                if (c.InBounds(map)) {
                    TerrainDef terrain = c.GetTerrain(map);

                    if (terrain != null && terrain.IsWater)
                    {
                        numberOfWater++;
                    }

                }
                


              
            }

        }

        public override float GrowthRate
        {
            get
            {
                if (this.Blighted)
                {
                    return 0f;
                }
                if (base.Spawned && !PlantUtility.GrowthSeasonNow(base.Position, base.Map, false))
                {
                    return 0f;
                }
                return this.GrowthRateFactor_Fertility * this.GrowthRateFactor_Temperature * this.GrowthRateFactor_Light * this.GrowthRateFactor_Water;
            }
        }

        public float GrowthRateFactor_Water
        {
            get
            {
               if (numberOfWater <= 20) {
                    return 1f - ((20 - numberOfWater) * 0.05f);

               }
               else {
                    return 1f;
               }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            
            Scribe_Values.Look<int>(ref this.numberOfWater, "numberOfWater", 0, false);
       
        }


    }
}
