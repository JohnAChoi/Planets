using System;
using System.Drawing;

namespace Planets
{
	//Planet class declaration
	class Planet
	{
		/*Private member data*/
		//x and y coordinates of the planet, stored as pixels
		private float x;
		private float y;
		//X and Y components of the velocity of the planet. Stored as meters/second.
		private double xvel;
		private double yvel;
		//Radius of the planet in pixels
		private int radius;
		//Mass of the object
		protected UInt64 mass;
		//Could use BigInts to accurately represent mass of larger celestial bodies, but this simulation is slow enough as it is

		//Planet Constructor
		public Planet(Point start)
		{
			//Set its initial position
			x = start.X - 2;
			y = start.Y - 2;

			//Set its radius
			radius = 5;

			//Set the mass, which is based off of the mass of the earth
			mass = 5972000000;

			//Initialize its velocity
			xvel = 0;
			yvel = 0;
		}

		//X Property to access its X-position
		public float X
		{
			get
			{
				return x;
			}
			set
			{
				x = value;
			}
		}

		//Y Property to access its Y-position
		public float Y
		{
			get
			{
				return y;
			}
			set
			{
				y = value;
			}
		}

		//Radius property to get the radius of the planet
		public int Radius
		{
			get
			{
				return radius;
			}
		}

		//Mass property to get its mass
		public UInt64 Mass
		{
			get
			{
				return mass;
			}
		}

		//XVel property to access its X-velocity
		public double XVel
		{
			get
			{
				return xvel;
			}
			set
			{
				xvel = value;
			}
		}

		//YVel property to access its Y-velocity
		public double YVel
		{
			get
			{
				return yvel;
			}
			set
			{
				yvel = value;
			}
		}
	}

	//Star class declaration
	class Star : Planet
	{
		//Private member data
		private int range;

		//Star constructor
		public Star(Point start) : base(start)
		{
			//Set the size of its "halo"
			range = 16;
			//Set the star's mass, which is based on the mass of the Sun
			mass = 1989000000000000;
		}

		//Range Property to get the range of the planet
		public int Range
		{
			get
			{
				return range;
			}
		}
	}

	//ImmovableMass class Declaration
	class ImmovableMass : Planet
	{
		//Private member Data
		private int range;

		//ImmovableMass constructor
		public ImmovableMass(Point start) : base(start)
		{
			//Set the size of its "halo"
			range = 36;
			//Set its mass, which is just an arbitrarily large number
			mass = 5000000000000000;
		}

		//Range property to get its range
		public int Range
		{
			get
			{
				return range;
			}
		}
	}
}
