using System;
using System.Collections.Generic;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Extensions;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using ProjectileGoRound;

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
        var tower = weapon.attack.tower;
        if (tower == null) return;

        var towerModel = tower.rootModel.Cast<TowerModel>();
        if (towerModel.baseId == TowerType.DartlingGunner && towerModel.tiers[0] > 3 ||
            towerModel.baseId == TowerType.Alchemist ||
            towerModel.baseId == TowerType.SentryParagonGreen ||
            towerModel.baseId == TowerType.BeastHandler)
        {
            return;
        }

        var projectiles = new List<ProjectileModel>();
        foreach (var placedTower in InGame.Bridge.GetAllTowers().ToList())
        {
            var model = Game.instance.model.GetTowerFromId(placedTower.tower.towerModel.name);
            if (model == null) continue;

            foreach (var attack in model.GetAttackModels())
            {
                if (attack.name.Contains("AttackAirUnitModel_Anti-MoabMissile_")) continue;

                foreach (var weaponModel in attack.weapons)
                {
                    var projectile = weaponModel.projectile.Duplicate();
                    if (projectile.HasBehavior<TravelStraitModel>())
                    {
                        projectile.GetBehavior<TravelStraitModel>().lifespan *= 1.5f;
                        projectile.AddBehavior(new ProjectileBlockerCollisionReboundModel("bounce", true, true));
                        projectile.AddBehavior(new MapBorderReboundModel("bounce", true, true));
                    }
                    projectiles.Add(projectile);
                }
            }
        }

        if (projectiles.Count == 0) return;

        var updatedModel = towerModel.Duplicate();
        foreach (var attack in updatedModel.GetAttackModels())
        {
            foreach (var weaponModel in attack.weapons)
            {
                weaponModel.SetProjectile(projectiles[Random.Shared.Next(projectiles.Count)]);
            }
        }
        tower.UpdateRootModel(updatedModel);
    }
}
