# Unity FPS Game

A Unity 4.x FPS project developed in 2013–2014 and revisited in 2023. This repository preserves selected gameplay source, models, and media for portfolio review.

**Status: historical source and artwork showcase. This extraction is not currently a self-contained, runnable Unity project.** Some dependencies and imported assets have intentionally been excluded for licensing reasons. It has not been verified in a modern Unity editor.

## Start reading here

| System | Files | What to examine |
| --- | --- | --- |
| Weapons | [GunData.cs](Assets/Scripts/GunData.cs), [Gun.cs](Assets/Scripts/Gun.cs) | Weapon parameters, firing cadence, recoil, projectile spawning, and animated handling |
| Equipment and character state | [Equipment.cs](Assets/Scripts/Equipment.cs), [Soldier.cs](Assets/Scripts/Soldier.cs), [Player.cs](Assets/Scripts/Player.cs) | Reload/zoom events, equipment switching, damage events, and movement |
| Projectiles and damage | [Bullet.cs](Assets/Scripts/Bullet.cs), [Damage.cs](Assets/Scripts/Damage.cs), [Grenade.cs](Assets/Scripts/Grenade.cs) | Raycasts along projectile movement and damage delivery |
| Attachments | [Optics.cs](Assets/Scripts/Optics.cs), [Sight.cs](Assets/Scripts/Sight.cs), [LaserPointer.cs](Assets/Scripts/LaserPointer.cs) | Aiming, sight feedback, and attachment behavior |
| Enemy behavior | [Robot.cs](Assets/Scripts/Robot.cs), [EnemySpawn.cs](Assets/Scripts/EnemySpawn.cs) | Steering, firing, spawning, and progression |
| Original artwork | [Models](Assets/Models), [Images](Assets/Images), [Sounds](Assets/Sounds) | Blender, image-editing, and sound-generation source files alongside game assets |

The project author created the gameplay implementation and original artwork. Imported character packs, reference photographs, and the externally derived MouseLook/DrawLine utilities have been removed from this extraction. Some character modeling source files retain MakeHuman-related paths; those models should be described with that workflow in mind. Remaining files have not received exhaustive provenance or redistribution verification. No blanket license for all contents is asserted here.

## Omitted dependencies and historical APIs

- HOTween (`Holoville.HOTween`, including `Sequence` and `TweenParms`).
- `IKLimb` and `HeadLookController` components.
- `MyMouseLook` and the `Drawing` utility, removed during source curation but still referenced by some retained scripts.
- Imported animation, model, effect, and other resources referenced by historical scenes/prefabs may also be absent.

The source uses Unity 4.x APIs, including the original networking API, GUIText/GUITexture, and component shortcuts. Restored ProjectSettings preserve historical inputs, tags, and build settings; they do not restore missing dependencies or establish modern Unity compatibility.
