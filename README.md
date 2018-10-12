# inktober
An attempt to catalog example scripts and unity-related tutorials in one repository for the Inktober 2018 trend.

IMPORTANT: This example project is using Unity 2018.2.10f1

Day 1: Simple GameObject animation system. I found this useful for animating my Voxel characters in Mobster Lobsters and Thug Slugs since I made each frame a new obj that I exported from MagicaVoxel into Unity. It made it easy to set up all the frames under a parent object then apply the animation script to control the speed of the animations and if they are looping/ping-ponging.

Spin script is useful for making objects spin over time and having control both locally and globally around which axis the object spins and which direction/speed it will spin.

SetRandomColor script is useful for debugging things mostly. I use it to distinguish multiple objects at the start of the app in editor. There are probably gizmos or other ways to do this, but I find this was a super simple way to prototype ideas without getting confused by similar objects with the same color.

Day 2: Converted the GameObject animation system and Spin script to utilize Coroutines. Also, cleaned up a bit and removed bad code.

![Alt Text](https://media.giphy.com/media/3Xzdy2QzfsVZ44WkwW/giphy.gif)

Day 3: Made a voxel character and used all the previous day examples to showcase the animation framework and other scripts.

![Alt Text](https://media.giphy.com/media/fHfMf1fktaPnMJW4Ic/giphy.gif)

Day 4: Made gizmo spheres for easy locating and clicking of objects in the scene.

![Alt Text](https://media.giphy.com/media/8mboAtWRjMYXW2S2iC/giphy.gif)

Day 5: Pause/Resume using coroutines to affect TimeScale. Needs refinement, but the idea is there!

![Alt Text](https://media.giphy.com/media/dZ5jQyUCmyoEcQ0XU6/giphy.gif)

Day 6: Utility scripts to help find child objects. Much like GetComponentsInChildren; however, these examples are useful for excluding the parent and/or excluding/including descendants of children.

![Alt Text](https://media.giphy.com/media/4JXL0kKgJYMdBDJxNF/giphy.gif)

Day 7: Wave system for survival-type games. Manages waves, sub-waves, and enabling/disabling of enemies in the sub-waves.

![Alt Text](https://media.giphy.com/media/dYCgAr6ikiyayX4C6J/giphy.gif)

Day 8: BulletHell basic mechanics. Building off previous examples and scripts. Lots of new scripts added and refactoring done to the previous examples.

![Alt Text](https://media.giphy.com/media/c6VxjHYOjQaCgleWrV/giphy.gif)

Day 9: Enemy AI and explosion particles.

![Alt Text](https://media.giphy.com/media/d7p8vMkxtMIqJK5bHH/giphy.gif)

Day 10: Typewriter text. Sound fx and Wave Complete text triggered on sub-wave completion

![Alt Text](https://media.giphy.com/media/cJbnvxz8FCA59MSc3O/giphy.gif)

Day 11: Respawning. Health/Lives. Game Over. All Waves Clear. Probably should have split to future days. Moving onto a new game type for the following days! 

![Alt Text](https://media.giphy.com/media/etAFqFx6jhVip2QCuN/giphy.gif)

Day 12: Rudimentary multiplayer. Needs refinement/clean-up. But it works!

![Alt Text](https://media.giphy.com/media/5vYnL0XzqFOSe4Hbxq/giphy.gif)
