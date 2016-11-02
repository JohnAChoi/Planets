using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Planets
{
	class Physics
	{
		//Gravitational constant for use in Newton's law of universal gravitation
		private static double GravConst = .00000000006673;
		//Lists to keep track of all the objects created by the user
		private List<Planet> m_planets;
		private List<Star> m_stars;
		private List<ImmovableMass> m_masses;

		//Time scale
		private double m_time_scale;
		//Conversion for meters in simulation to pixels on screen
		private int m_meters_per_pixel;
		//Length of time to use for each subsimulation
		private float m_sub_sim_time;
		//Keep track of how many times to run the simulation per frame
		private int m_num_sub_sim;

		//Keep a reference to the frame the simulation is drawn to
		//Need it only for the height and width, which could change depending on the user
		PictureBox m_render_window;

		public Physics(double ts, int mpp, PictureBox f)
		{
			m_time_scale = ts;
			m_meters_per_pixel = mpp;
			m_render_window = f;

			//Initialize all of the lists
			m_planets = new List<Planet>();
			m_stars = new List<Star>();
			m_masses = new List<ImmovableMass>();

			//Simulate m_sub_sim_time seconds of time per iteration of the simulation
			m_sub_sim_time = 0.1f;
			//Run the calculations in Simulate() m_num_sub_sim times per frame
			m_num_sub_sim = (int)(m_time_scale * .03 / m_sub_sim_time);
		}

		//Physics step called once per frame
		public void Simulate()
		{
			for (int i = 0; i < m_num_sub_sim; i++)
			{
				//Check each planet
				foreach (Planet earth in m_planets)
				{
					//keep track of the x- and y-components of the net force on the planet
					double xforce = 0.0;
					double yforce = 0.0;

					//Calculate the total force of the other planets affecting this planet
					foreach (Planet other in m_planets)
					{
						//First, find the distance between the two planets
						double dist = FindDistance(earth.X, earth.Y, other.X, other.Y);
						//Declare a double to store the magnitude of the force
						double subforce = 0.0;

						//If the planets are not colliding
						if (dist > 1.0)
						{
							//Calculate the force between the two planets with
							//Newton's Law of Universal Gravitation
							subforce = (GravConst * earth.Mass * other.Mass) / dist;
						}

						//Calculate the angle between the two planets
						double angle = FindAngle(earth.X, earth.Y, other.X, other.Y);
						//Break the force into it's x- and y-components and add it to the running sum
						xforce += subforce * Math.Cos(angle);
						yforce += subforce * Math.Sin(angle);
					}
					//Repeat the above, but for each star
					foreach (Planet other in m_stars)
					{
						double dist = FindDistance(earth.X, earth.Y, other.X, other.Y);
						double subforce = 0.0;

						if (dist > 1.0)
							subforce = (GravConst * earth.Mass * other.Mass) / dist;

						double angle = FindAngle(earth.X, earth.Y, other.X, other.Y);
						xforce += subforce * Math.Cos(angle);
						yforce += subforce * Math.Sin(angle);
					}
					//Round three, but this time with the immovable masses
					foreach (Planet other in m_masses)
					{
						double dist = FindDistance(earth.X, earth.Y, other.X, other.Y);
						double subforce = 0.0;

						if (dist > 1.0)
							subforce = (GravConst * earth.Mass * other.Mass) / dist;

						double angle = FindAngle(earth.X, earth.Y, other.X, other.Y);
						xforce += subforce * Math.Cos(angle);
						yforce += subforce * Math.Sin(angle);
					}

					//Calculate the x- and y-components of the planet's acceleration
					//Using the calculated force and the planet's mass. 
					//Force = Mass * Acceleration
					double xaccel = xforce / earth.Mass;
					double yaccel = yforce / earth.Mass;

					//Calculate the x- and y-components of the planet's change 
					//in velocity for the time step using the calculated acceleration. 
					//Velocity = Acceleration * Time elapsed
					earth.XVel += xaccel * m_sub_sim_time;
					earth.YVel += yaccel * m_sub_sim_time;

					//Calculated the planet's new position using 
					//the calculated velocity components
					//Divide by the distance factor to scale from meters to pixels
					earth.X += (float)((earth.XVel * m_sub_sim_time) / m_meters_per_pixel);
					earth.Y += (float)((earth.YVel * m_sub_sim_time) / m_meters_per_pixel);

					//Make sure the planet stays on screen
					CheckBounds(earth);
				}

				//Same thing as the previous block, just for each of the stars instead
				foreach (Planet star in m_stars)
				{
					double xforce = 0.0;
					double yforce = 0.0;

					foreach (Planet other in m_planets)
					{
						double dist = FindDistance(star.X, star.Y, other.X, other.Y);
						double subforce = 0.0;

						if (dist > 1.0)
							subforce = (GravConst * star.Mass * other.Mass) / dist;

						double angle = FindAngle(star.X, star.Y, other.X, other.Y);
						xforce += subforce * Math.Cos(angle);
						yforce += subforce * Math.Sin(angle);
					}

					foreach (Planet other in m_stars)
					{
						double dist = FindDistance(star.X, star.Y, other.X, other.Y);
						double subforce = 0.0;

						if (dist > 1.0)
							subforce = (GravConst * star.Mass * other.Mass) / dist;

						double angle = FindAngle(star.X, star.Y, other.X, other.Y);
						xforce += subforce * Math.Cos(angle);
						yforce += subforce * Math.Sin(angle);
					}

					foreach (Planet other in m_masses)
					{
						double dist = FindDistance(star.X, star.Y, other.X, other.Y);
						double subforce = 0.0;

						if (dist > 1.0)
							subforce = (GravConst * star.Mass * other.Mass) / dist;

						double angle = FindAngle(star.X, star.Y, other.X, other.Y);
						xforce += subforce * Math.Cos(angle);
						yforce += subforce * Math.Sin(angle);
					}

					double xaccel = xforce / star.Mass;
					double yaccel = yforce / star.Mass;

					star.XVel += xaccel * m_sub_sim_time;
					star.YVel += yaccel * m_sub_sim_time;

					star.X += (float)((star.XVel * m_sub_sim_time) / m_meters_per_pixel);
					star.Y += (float)((star.YVel * m_sub_sim_time) / m_meters_per_pixel);

					CheckBounds(star);
				}
			}
		}

		//Find the distance between two points using the Pythagorean Theorem
		private double FindDistance(float x1, float y1, float x2, float y2)
		{
			float dx = x1 - x2;
			float dy = y1 - y2;

			return Math.Sqrt(dx * dx + dy * dy) * m_meters_per_pixel;
		}

		//Find the angle between two points using arctangent()
		private double FindAngle(float x1, float y1, float x2, float y2)
		{
			float dx = x2 - x1;
			float dy = y2 - y1;

			return Math.Atan2(dy, dx);
		}

		//Check to make sure that a planet is in the bounds of the drawing area
		//Flips the sign on the correct velocity component to make it bounce off the wall
		private void CheckBounds(Planet planet)
		{
			//Check if the planet is going off the left side of the image
			if (planet.X < 0)
			{
				planet.X = 0;
				planet.XVel *= -1;
			}

			//Check if the planet is going off the right side of the image
			if (planet.X > m_render_window.Width)
			{
				planet.X = m_render_window.Width - 5;
				planet.XVel *= -1;
			}

			//Check if the planet is going off the top of the image
			if (planet.Y < 0)
			{
				planet.Y = 0;
				planet.YVel *= -1;
			}

			//Check if the planet is going off the bottom of the image
			if (planet.Y > m_render_window.Height)
			{
				planet.Y = m_render_window.Height - 5;
				planet.YVel *= -1;
			}
		}

		//Adds the given type of celestial body to the simulation
		public void AddBody(int type, int x, int y)
		{
			switch (type)
			{
				case 0:
					m_planets.Add(new Planet(new System.Drawing.Point(x, y)));
					break;
				case 1:
					m_stars.Add(new Star(new System.Drawing.Point(x, y)));
					break;
				case 2:
					m_masses.Add(new ImmovableMass(new System.Drawing.Point(x, y)));
					break;
				default:
					break;
			}
		}

		//Set the time factor
		public void SetTimeFactor(double time)
		{
			//Set the factor
			m_time_scale = time;
			//Recalculate how many times per frame to run the calculations in Simulate()
			m_num_sub_sim = (int)(m_time_scale * .03 / m_sub_sim_time);
		}

		//Set the distance factor
		public void SetDistanceFactor(int factor)
		{
			m_meters_per_pixel = factor;
		}

		//Clears all of the bodies from the simulation
		public void ClearBodies()
		{
			m_planets.Clear();
			m_stars.Clear();
			m_masses.Clear();
		}

		//Properties for the celestial objects
		public IList<Planet> Planets
		{
			get
			{
				return m_planets.AsReadOnly();
			}
		}

		public IList<Star> Stars
		{
			get
			{
				return m_stars.AsReadOnly();
			}
		}

		public IList<ImmovableMass> Masses
		{
			get
			{
				return m_masses.AsReadOnly();
			}
		}
	}
}
