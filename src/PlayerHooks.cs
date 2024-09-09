using BepInEx;
using JetBrains.Annotations;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Botanist
{
    internal class PlayerHooks
    {

        public void OnEnable()
        {
            try
            {
                On.Player.SwallowObject += Player_SwallowObject;
                On.Creature.Violence += Creature_Violence;
                On.Player.LungUpdate += Player_LungUpdate;
            }
            catch (Exception ex)
            {
                Plugin.s_logger.LogError(ex);
            }
        }

        private void Player_LungUpdate(On.Player.orig_LungUpdate orig, Player self)
        {
            if (self.GetCustomData().activePlant == BotEnums.BotActivePlant.BubbleGrass)
            {
                self.lungsExhausted = false;
                self.airInLungs = 1;
            }
            else
            {
                orig(self);
            }
        }

        private void Creature_Violence(On.Creature.orig_Violence orig, Creature self, BodyChunk source, Vector2? directionAndMomentum, BodyChunk hitChunk, PhysicalObject.Appendage.Pos hitAppendage, Creature.DamageType type, float damage, float stunBonus)
        {
            orig(self, source, directionAndMomentum, hitChunk, hitAppendage, type, damage, stunBonus);
            if (self is Player player && player != null && player.SlugCatClass.value == ("Botanist"))
            {
                if (player.GetCustomData().activePlant == BotEnums.BotActivePlant.FirecrackerPlant)
                {
                    if (type == Creature.DamageType.Explosion)
                    {
                        damage *= 0f;
                    }
                }
            }
        }

        private void Player_SwallowObject(On.Player.orig_SwallowObject orig, Player self, int grasp)
        {
            orig(self, grasp);
            if (self != null && self.SlugCatClass.value == ("Botanist"))
            {
                if (self.objectInStomach.type == AbstractPhysicalObject.AbstractObjectType.BubbleGrass)
                {
                    self.GetCustomData().activePlant = BotEnums.BotActivePlant.BubbleGrass;
                    self.objectInStomach.Destroy();
                }
                if (self.objectInStomach.type == AbstractPhysicalObject.AbstractObjectType.FirecrackerPlant)
                {
                    self.GetCustomData().activePlant = BotEnums.BotActivePlant.FirecrackerPlant;
                    self.objectInStomach.Destroy();
                }
                if (self.objectInStomach.type == AbstractPhysicalObject.AbstractObjectType.FlareBomb)
                {
                    self.GetCustomData().activePlant = BotEnums.BotActivePlant.FlareBomb;
                    self.objectInStomach.Destroy();
                }
                if (self.objectInStomach.type == AbstractPhysicalObject.AbstractObjectType.FlyLure)
                {
                    self.GetCustomData().activePlant = BotEnums.BotActivePlant.FlyLure;
                    self.objectInStomach.Destroy();
                }
                if (self.objectInStomach.type == AbstractPhysicalObject.AbstractObjectType.PuffBall)
                {
                    self.GetCustomData().activePlant = BotEnums.BotActivePlant.PuffBall;
                    self.objectInStomach.Destroy();
                }
                if (self.objectInStomach.type == AbstractPhysicalObject.AbstractObjectType.SporePlant)
                {
                    self.GetCustomData().activePlant = BotEnums.BotActivePlant.SporePlant;
                    self.objectInStomach.Destroy();
                }
            }
        }
    }

    public static class BotanistCWT
    {
        static ConditionalWeakTable<Player, Data> table = new ConditionalWeakTable<Player, Data> ();
        public static Data GetCustomData(this Player self) => table.GetValue(self, x => new Data(x));

        public class Data
        {
            internal BotEnums.BotActivePlant activePlant = BotEnums.BotActivePlant.None;
            public Data(Player self) { }
        }
    }
}
