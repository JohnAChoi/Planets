# Planets
A 2D simulation of celestial bodies written in C# using Winforms

	This program uses Newton's Law of Universal Gravitation to simulate the movement of 
planets, stars, and immovable masses. It calculates the force on each celestial body from 
each other body. Then it calculates the acceleration, velocity, and then the new position 
of each body each frame. Planets and stars will move but, as their name implies, immovable 
masses will not. 

	Planets are relatively small, mass-wise. Stars are medium-sized and immovable masses 
are large. The "halos" around the stars and immovable masses are for visuals, only. They 
do not signify anything important in the simulation. Planets and stars will move around the 
screen and bounce off the sides of image instead of going beyond them. This is to prevent 
issues with them flying far off into the distance and causing overflow errors. Also, it's 
rather entertaining to think about planets and stars bouncing off of invisible walls out on 
the edges of space.

	The first text box shows the time scale used for simulation. This is how many seconds 
of time are simulated per second of real time. Setting the value to less than about 3.5 will 
effectively pause the simulation. Setting the value too large or having too many objects 
on-screen will cause the program to slow down. 

	The second text box shows the distance factor used for simulation. It is how many meters 
each pixel on screen represents. Changing this value will not visually change where bodies 
are currently placed. However, this will change the distance between each object.

	The easiest way to ensure that changes to either of these values goes through is to 
press tab after making changes. 

Known bugs:
	The physics contains no way of handling collisions, so some rather erratic behavior can 
occur when two bodies "collide". The best way to observe this is to place just two stars in 
the simulation and let them go at it. Sometimes one will slingshot past the other, other times 
the two stars will "chase" each other around the screen.
