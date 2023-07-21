# inktober
An attempt to catalog example scripts and unity-related tutorials in one repository for the Inktober 2018 trend.

IMPORTANT: This example project is using Unity 2018.2.10f1

**Day 1**: Simple GameObject animation system. I found this useful for animating my Voxel characters in Mobster Lobsters and Thug Slugs since I made each frame a new obj that I exported from MagicaVoxel into Unity. It made it easy to set up all the frames under a parent object then apply the animation script to control the speed of the animations and if they are looping/ping-ponging.

Spin script is useful for making objects spin over time and having control both locally and globally around which axis the object spins and which direction/speed it will spin.

SetRandomColor script is useful for debugging things mostly. I use it to distinguish multiple objects at the start of the app in editor. There are probably gizmos or other ways to do this, but I find this was a super simple way to prototype ideas without getting confused by similar objects with the same color.

**Day 2**: Converted the GameObject animation system and Spin script to utilize Coroutines. Also, cleaned up a bit and removed bad code.

![day2](https://github.com/thunderawesome/inktober/assets/4368017/0fa929d3-b20a-4efe-904f-ac8ce4a48391)

**Day 3**: Made a voxel character and used all the previous day examples to showcase the animation framework and other scripts.

![day3](https://github.com/thunderawesome/inktober/assets/4368017/b97bfdc4-d5d0-4f89-a69b-7a02654539d3)

**Day 4**: Made gizmo spheres for easy locating and clicking of objects in the scene.

![day4](https://github.com/thunderawesome/inktober/assets/4368017/16216b71-8b01-4993-a75d-09a96e3108df)

**Day 5**: Pause/Resume using coroutines to affect TimeScale. Needs refinement, but the idea is there!

![day5](https://github.com/thunderawesome/inktober/assets/4368017/7615919e-b495-4422-99e0-a039ec8929cc)

**Day 6**: Utility scripts to help find child objects. Much like GetComponentsInChildren; however, these examples are useful for excluding the parent and/or excluding/including descendants of children.

![day6](https://github.com/thunderawesome/inktober/assets/4368017/50d56fde-2eb9-41bb-a4f0-01ea5e61baed)

**Day 7**: Wave system for survival-type games. Manages waves, sub-waves, and enabling/disabling of enemies in the sub-waves.

![day7](https://github.com/thunderawesome/inktober/assets/4368017/55ce3256-cdb0-47df-86f0-acee8ad6ee9e)

**Day 8**: BulletHell basic mechanics. Building off previous examples and scripts. Lots of new scripts added and refactoring done to the previous examples.

![day8](https://github.com/thunderawesome/inktober/assets/4368017/195e0f15-c880-4857-933a-682607a405ec)

**Day 9**: Enemy AI and explosion particles.

![day9](https://github.com/thunderawesome/inktober/assets/4368017/4066de7a-2384-4049-b1ae-0348e3690fae)

**Day 10**: Typewriter text. Sound fx and Wave Complete text triggered on sub-wave completion

![day10](https://github.com/thunderawesome/inktober/assets/4368017/430ab2af-ee26-4c31-a9e7-1e522f2860ae)

**Day 11**: Respawning. Health/Lives. Game Over. All Waves Clear. Probably should have split to future days. Moving onto a new game type for the following days! 

![day11](https://github.com/thunderawesome/inktober/assets/4368017/400982ee-f538-4735-85a7-edf667ff55ab)

**Day 12**: Rudimentary multiplayer. Needs refinement/clean-up. But it works!

![day12](https://github.com/thunderawesome/inktober/assets/4368017/136a29fb-3851-4037-85a9-17d230258083)

**Day 13**: Simple platformer with double jump ability.

![day13](https://github.com/thunderawesome/inktober/assets/4368017/77a143a2-7596-430f-b8c3-4eae001b3980)

**Day 14**: Simple waypoint AI and co-op platforming.

![day14](https://github.com/thunderawesome/inktober/assets/4368017/729bc004-8479-4198-a7bb-94bccd066249)

**Day 15**: Head stomps. Also, fixed some issues in the bullet-hell example(s).

![day15](https://github.com/thunderawesome/inktober/assets/4368017/d6cfe08c-0619-489a-9bc2-3060c8cbda1c)

**Day 16**: Screen Shake added to Day 16 scene for bullet-hell game.

![day16](https://github.com/thunderawesome/inktober/assets/4368017/e6738ca5-805c-4fbf-90e6-c18b3066e623)

**Day 17**: ScriptableAsset example

![day17](https://github.com/thunderawesome/inktober/assets/4368017/e18a6a7b-6967-423b-ac12-972793b45f20)

**Day 18**: Kuwahara Shader example

![day18](https://github.com/thunderawesome/inktober/assets/4368017/ca355627-4aec-4afb-9a3a-57d923a9fa6b)
