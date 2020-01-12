# A Star Pathfinding for Unity

This is an A* Pathfinder for Unity. It inlcudes 3 axis pathfinding and multi-threading.

  
# Usage
* Drop the components Gridbase and PathfindMaster on a gameObject in your scene.
* Call the PathfinderMaster.RequestPathFind(startingNode, targetNode, callback).
* For callback, use a function that takes a list of Nodes
* The returned list is your path
  
