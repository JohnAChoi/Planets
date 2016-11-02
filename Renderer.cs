using System.Drawing;
using System.Windows.Forms;

namespace Planets
{
	class Renderer
	{
		//A blank white bitmap image to serve as the background image
		private Bitmap background;

		//Bitmap image that represents the current frame
		private Bitmap frame;

		//Keep a reference to the physics engine to get locations of stars
		private Physics physics;

		//Keep a reference to the PictureBox that this renderer draws to
		private PictureBox m_render_window;

		//Constructor
		public Renderer(PictureBox rw, Physics p)
		{
			//Save the parameters passed in
			m_render_window = rw;
			physics = p;

			//Create a new background that is the size of the picture box
			background = new Bitmap(m_render_window.Width, m_render_window.Height);
			//Create a graphics object to draw the background with
			using (Graphics scene = Graphics.FromImage(background))
				//Create a solid brush and dispose of it when it's unneeded
				using (Brush WhiteBack = new SolidBrush(Color.White))
				{
					//Designate a rectangle that is the size of the picture box
					Rectangle all = new Rectangle(0, 0, m_render_window.Width, m_render_window.Height);
					//Fill in the background as pure white
					scene.FillRectangle(WhiteBack, all);
					scene.DrawImage(background, 0, 0);
					//Set the picture box to be the all-white image
					m_render_window.Image = background;
				}
		}

		//Render function
		//Draws all the heavenly bodies and their ranges, if necessary
		public void RefreshScreen()
		{
			//Take a reference to the old frame to free it up later
			Bitmap oldframe = frame;
			//Create a new frame, starting from the blank white background
			frame = new Bitmap(background);
			//Prepare to draw on it
			using (Graphics scene = Graphics.FromImage(frame))
			{
				//Declare a gray solid brush and draw the circles around the Immovable Masses
				using (Brush gray = new SolidBrush(Color.Gray))
					foreach (ImmovableMass mass in physics.Masses)
						scene.FillEllipse(gray, mass.X - (mass.Range - 4) / 2, mass.Y - (mass.Range - 4) / 2, mass.Range, mass.Range);

				//Declare a light gray solid brush and draw the circles around the Stars
				using (Brush lightgrey = new SolidBrush(Color.LightGray))
					foreach (Star star in physics.Stars)
						scene.FillEllipse(lightgrey, star.X - (star.Range - 4) / 2, star.Y - (star.Range - 4) / 2, star.Range, star.Range);

				//Declare a black solid brush and draw the Immovable Masses
				using (Brush black = new SolidBrush(Color.Black))
					foreach (ImmovableMass fat in physics.Masses)
						scene.FillEllipse(black, fat.X, fat.Y, fat.Radius, fat.Radius);

				//Declare a red solid brush and draw the Stars
				using (Brush red = new SolidBrush(Color.Red))
					foreach (Star hole in physics.Stars)
						scene.FillEllipse(red, hole.X, hole.Y, hole.Radius, hole.Radius);

				//Declare a blue solid brush and draw the planets
				using (Brush blue = new SolidBrush(Color.Blue))
					foreach (Planet earth in physics.Planets)
						scene.FillEllipse(blue, earth.X, earth.Y, earth.Radius, earth.Radius);
			}
			//Render the image to the screen by setting the picture box image to it
			m_render_window.Image = frame;

			//Get rid of the old frame to prevent a low-level memory leak.
			if (null != oldframe)
				oldframe.Dispose();
		}
	}
}
