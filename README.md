# inktober
An attempt to catalog example scripts and unity-related tutorials in one repository for the Inktober 2018 trend.

IMPORTANT: This example project is using Unity 2018.2.10f1

Day 1: Simple GameObject animation system. I found this useful for animating my Voxel characters in Mobster Lobsters and Thug Slugs since I made each frame a new obj that I exported from MagicaVoxel into Unity. It made it easy to set up all the frames under a parent object then apply the animation script to control the speed of the animations and if they are looping/ping-ponging.

Spin script is useful for making objects spin over time and having control both locally and globally around which axis the object spins and which direction/speed it will spin.

SetRandomColor script is useful for debugging things mostly. I use it to distinguish multiple objects at the start of the app in editor. There are probably gizmos or other ways to do this, but I find this was a super simple way to prototype ideas without getting confused by similar objects with the same color.

Day 2: Converted the GameObject animation system and Spin script to utilize Coroutines. Also, cleaned up a bit and removed bad code.
