using System;
using System.Windows.Forms;

//Implementation by John Choi

namespace Planets
{
    public partial class Form1 : Form
    {
        //Member variables

        //Integer to keep track of which type of object to create and draw
        int body_type;

		//Timer object to control the framerate
		System.Windows.Forms.Timer refresh;

        //Time factor to keep track of how many seconds to simulate per frame
        double time_factor;
        //Distance factor to keep track of how many meters a pixel represents
        int distance_factor;

		//Reference to the physics engine
		Physics physics;

		//Reference to the renderer
		Renderer renderer;

        public Form1()
        {
            InitializeComponent();
        }

        //Function to call when the form loads. Kind of like an initializer, but not really
        private void Form1_Load(object sender, EventArgs e)
        {
            //Initialize the time factor and display it in the text box on the form
            time_factor = 100.0;
            textBox1.Text = time_factor.ToString();

            //Initialize the distance factor and display it in the text box on the form
            distance_factor = 1000;
            textBox2.Text = distance_factor.ToString();

            //Start out creating planets and disable the planet button on the form
            body_type = 0;
            button1.Enabled = false;

			//Initialize the physics engine and renderer
			physics = new Physics(time_factor, distance_factor, pictureBox1);
			renderer = new Renderer(pictureBox1, physics);

            //Begin a timer
            refresh = new System.Windows.Forms.Timer();
            //Assign it a delegate and refresh rate
            refresh.Tick += new EventHandler(Update);
            refresh.Interval = 30; 
            //Start the clock
            refresh.Start();
        }        
        
        //When the Planet button is clicked
        private void button1_Click(object sender, EventArgs e)
        {
            //Change the type of body to planets
            body_type = 0;

            //Disable the planet button
            button1.Enabled = false;

            //And enable the other two buttons
            button2.Enabled = true;
            button3.Enabled = true;
        }

        //When the Star button is clicked
        private void button2_Click(object sender, EventArgs e)
        {
            //Change the type of body to stars
            body_type = 1;

            //Disable the star button
            button2.Enabled = false;

            //Enable the other two buttons
            button1.Enabled = true;
            button3.Enabled = true;
        }

        //When the Immovable Mass button is clicked
        private void button3_Click(object sender, EventArgs e)
        {
            //Change the type of body to create to ImmovableMasses
            body_type = 2;

            //Enable the other two buttons
            button1.Enabled = true;
            button2.Enabled = true;

            //Disable the Immovable Mass button
            button3.Enabled = false;
        }
        
        //When the mouse is released inside of the picture box object
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
			//Tell the physics engine to create a new body of the chosen type at the location of the mouse
			physics.AddBody(body_type, e.X, e.Y);
        }

        //Update function to call when the Timer ticks
        private void Update(object sender, EventArgs e)
        {
            //Run the physics simulation
            physics.Simulate();
            //Render the results to the screen
            renderer.RefreshScreen();
        }

        //When the time factor text box loses focus
        private void textBox1_Leave(object sender, EventArgs e)
        {
            //Create a temporary double to use
            double temp = 0.0;
			//Attempt to parse the value in the text box as a double
			if (Double.TryParse(textBox1.Text.ToString(), out temp))
				if (temp >= 0) //Accept the value if it parsed to something 0 or positive
				{
					physics.SetTimeFactor(temp);
					return;
				}
			
			//Otherwise, reset it to the original value
			textBox1.Text = time_factor.ToString();
        }

        //When the distance factor box loses focus
        private void textBox2_Leave(object sender, EventArgs e)
        {
            //Create a temporary double to use
            int temp = 0;
            //Attempt to parse the value in the text box as a double
            if (Int32.TryParse(textBox2.Text.ToString(), out temp))
                if (temp > 0) //Accept the value if it is greater than 0
				{
					physics.SetDistanceFactor(temp);
					return;
				}
			
			//Otherwise, reset the value in the text box
			textBox2.Text = time_factor.ToString();
        }

        //Function to reset the simulation
        private void resetSimulationToolStripMenuItem_Click(object sender, EventArgs e)
        {
			//Clear out all the lists of heavenly bodies
			physics.ClearBodies();
            //Reset the time factor and the text box to the default value
            time_factor = 100.0;
            textBox1.Text = time_factor.ToString();
            //Reset the distance factor and the text box to the default value
            distance_factor = 1000;
            textBox2.Text = distance_factor.ToString();
        }

        //Print out a help message to the user when the Help item is clicked
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
			MessageBox.Show("This is a simple program to simulate the movement of planets around stars.\r\n"
							+ "Click on the white space to place a planet, star, or immovable mass.\r\n"
							+ "Click on a button to select which type of celestial body to place.\r\n"
							+ "Planets are blue, stars are red, and immovable masses are black.\r\n"
							+ "Stars have a light gray circle around them.\r\n"
							+ "Immovable masses have a gray circle around them.\r\n"
							+ "Stars and planets will bounce off of the sides of the window.\r\n"
							+ "Time scale is how many seconds are simulated per second of real time.\r\n"
							+ "Setting this value too high will result poor simulations.\r\n"
							+ "Setting this value to a value < 3.5 will effectively pause the simulation.\r\n"
                            + "Distance factor is how many meters each pixel represents.\r\n"
                            + "The \"Reset Simluation\" menu item will remove all bodies from\r\n"
                            + "the simulation and reset the time and distance factors");
        }
    }
}
