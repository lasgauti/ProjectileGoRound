using MelonLoader;
using BTD_Mod_Helper;
using ProjectileGoRound;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers;
using BTD_Mod_Helper.Extensions;
using System.Collections.Generic;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppNinjaKiwi.GUTS.Models.Endpoints.CT;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using static Il2CppSystem.DateTimeParse;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Powers;
using UnityEngine.Playables;
using static Il2CppFacepunch.Steamworks.Inventory;
using Harmony;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;

[assembly: MelonInfo(typeof(ProjectileGoRound.ProjectileGoRound), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace ProjectileGoRound;

public class ProjectileGoRound : BloonsTD6Mod
{
    public override void OnApplicationStart()
    {
        ModHelper.Msg<ProjectileGoRound>("ProjectileGoRound loaded!");
    }
    public override void OnWeaponFire(Weapon weapon)
    {
        var towerModel = weapon.attack.tower;
        var towerModel2 = towerModel.rootModel.Duplicate().Cast<TowerModel>();
        List<ProjectileModel> projectiles = new List<ProjectileModel>();
        if (towerModel2.baseId == TowerType.DartlingGunner && towerModel2.tiers[0] > 3) { return; }
        if (towerModel != null)
        {
            var towers = InGame.Bridge.GetAllTowers().ToList();
            foreach (var tower in towers)
            {
                var tm = Game.instance.model.GetTowerFromId(tower.tower.towerModel.name);
                if (tm != null)
                {
                    if (tm.baseId == TowerType.DartlingGunner && tm.tiers[0] > 3) { return; }
                    foreach (var attack in tm.GetAttackModels())
                    {
                        foreach (var weap in attack.weapons)
                        {
                            var proj = weap.projectile.Duplicate();
                            if(proj.HasBehavior<TravelStraitModel>())
                            {
                                proj.GetBehavior<TravelStraitModel>().lifespan *= 1.5f;
                                proj.AddBehavior(new ProjectileBlockerCollisionReboundModel("bounce", true, true));
                                proj.AddBehavior(new MapBorderReboundModel("bounce", true));
                            }
                            projectiles.Add(proj);
                        }
                    }
                }
            }
            foreach (var attack in towerModel2.GetAttackModels())
            {
                foreach (var weap in attack.weapons)
                {
                    Il2CppSystem.Random rnd = new Il2CppSystem.Random();
                    int randomWeapon = rnd.Next(0, projectiles.Count);
                    ProjectileModel proj2 = projectiles[randomWeapon];
                    weap.SetProjectile(proj2); 
                }
            }
            towerModel.UpdateRootModel(towerModel2);
        } 
    }
}