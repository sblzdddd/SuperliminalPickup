# SuperliminalPickup
This Project is an example of object picking with perspective scaling in the game superliminal.

The basic principle is to Transform the object position (in a variable) along the view direction with a small unit, scale with the transform ratio until collision happens (`Physics.OverlapBox()`), and then push the object back until there are no collisions with walls.

The core codes are in `SuperEnginal.cs`

to create an object which can be picked up, change the layer of the object to `HoldLayer`, assign SuperProp Script to the object, and click `Setup Superliminal Object`
