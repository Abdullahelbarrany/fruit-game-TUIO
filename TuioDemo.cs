/*
	TUIO C# Demo - part of the reacTIVision project
	Copyright (c) 2005-2014 Martin Kaltenbrunner <martin@tuio.org>

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/

using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using TUIO;

	public class TuioDemo : Form , TuioListener
	{
		private TuioClient client;
		private Dictionary<long,TuioDemoObject> objectList;
		private Dictionary<long,TuioCursor> cursorList;
		private Dictionary<long,TuioBlob> blobList;
		private object cursorSync = new object();
		private object objectSync = new object();
		private object blobSync = new object();

		public static int width, height;
		private int window_width =  640;
		private int window_height = 480;
		private int window_left = 0;
		private int window_top = 0;
		private int screen_width = Screen.PrimaryScreen.Bounds.Width;
		private int screen_height = Screen.PrimaryScreen.Bounds.Height;

		private bool fullscreen;
		private bool verbose;

		SolidBrush blackBrush = new SolidBrush(Color.Black);
		SolidBrush whiteBrush = new SolidBrush(Color.White);

		SolidBrush grayBrush = new SolidBrush(Color.Gray);
		Pen fingerPen = new Pen(new SolidBrush(Color.Blue), 1);

		public TuioDemo(int port) {
		
			verbose = true;
			fullscreen = false;
			width = window_width;
			height = window_height;

			this.ClientSize = new System.Drawing.Size(width, height);
			this.Name = "TuioDemo";
			this.Text = "TuioDemo";
			
			this.Closing+=new CancelEventHandler(Form_Closing);
			this.KeyDown +=new KeyEventHandler(Form_KeyDown);

			this.SetStyle( ControlStyles.AllPaintingInWmPaint |
							ControlStyles.UserPaint |
							ControlStyles.DoubleBuffer, true);

			objectList = new Dictionary<long,TuioDemoObject>(128);
			cursorList = new Dictionary<long,TuioCursor>(128);
			
			client = new TuioClient(port);
			client.addTuioListener(this);

			client.connect();
		}

		private void Form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {

 			if ( e.KeyData == Keys.F1) {
	 			if (fullscreen == false) {

					width = screen_width;
					height = screen_height;

					window_left = this.Left;
					window_top = this.Top;

					this.FormBorderStyle = FormBorderStyle.None;
		 			this.Left = 0;
		 			this.Top = 0;
		 			this.Width = screen_width;
		 			this.Height = screen_height;

		 			fullscreen = true;
	 			} else {

					width = window_width;
					height = window_height;

		 			this.FormBorderStyle = FormBorderStyle.Sizable;
		 			this.Left = window_left;
		 			this.Top = window_top;
		 			this.Width = window_width;
		 			this.Height = window_height;

		 			fullscreen = false;
	 			}
 			} else if ( e.KeyData == Keys.Escape) {
				this.Close();

 			} else if ( e.KeyData == Keys.V ) {
 				verbose=!verbose;
 			}

 		}

		private void Form_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			client.removeTuioListener(this);

			client.disconnect();
			System.Environment.Exit(0);
		}

		public void addTuioObject(TuioObject o) {
			lock(objectSync) {
				objectList.Add(o.SessionID,new TuioDemoObject(o));
			} if (verbose) Console.WriteLine("add obj "+o.SymbolID+" ("+o.SessionID+") "+o.X+" "+o.Y+" "+o.Angle);
		}

		public void updateTuioObject(TuioObject o) {
			lock(objectSync) {
				objectList[o.SessionID].update(o);
			}
			if (verbose) Console.WriteLine("set obj "+o.SymbolID+" "+o.SessionID+" "+o.X+" "+o.Y+" "+o.Angle+" "+o.MotionSpeed+" "+o.RotationSpeed+" "+o.MotionAccel+" "+o.RotationAccel);
		}

		public void removeTuioObject(TuioObject o) {
			lock(objectSync) {
				objectList.Remove(o.SessionID);
			}
			if (verbose) Console.WriteLine("del obj "+o.SymbolID+" ("+o.SessionID+")");
		}

		public void addTuioCursor(TuioCursor c) {
			lock(cursorSync) {
				cursorList.Add(c.SessionID,c);
			}
			if (verbose) Console.WriteLine("add cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y);
		}

		public void updateTuioCursor(TuioCursor c) {
			if (verbose) Console.WriteLine("set cur "+c.CursorID + " ("+c.SessionID+") "+c.X+" "+c.Y+" "+c.MotionSpeed+" "+c.MotionAccel);
		}

		public void removeTuioCursor(TuioCursor c) {
			lock(cursorSync) {
				cursorList.Remove(c.SessionID);
			}
			if (verbose) Console.WriteLine("del cur "+c.CursorID + " ("+c.SessionID+")");
 		}

		public void addTuioBlob(TuioBlob b) {
			lock(blobSync) {
				blobList.Add(b.SessionID,b);
			}
		if (verbose) Console.WriteLine("add blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area);
		}

		public void updateTuioBlob(TuioBlob b) {
		if (verbose) Console.WriteLine("set blb "+b.BlobID + " ("+b.SessionID+") "+b.X+" "+b.Y+" "+b.Angle+" "+b.Width+" "+b.Height+" "+b.Area+" "+b.MotionSpeed+" "+b.RotationSpeed+" "+b.MotionAccel+" "+b.RotationAccel);
		}

		public void removeTuioBlob(TuioBlob b) {
			lock(blobSync) {
				blobList.Remove(b.SessionID);
			}
			if (verbose) Console.WriteLine("del blb "+b.BlobID + " ("+b.SessionID+")");
		}

		public void refresh(TuioTime frameTime) {
			Invalidate();
		}
	int level = 1;int progress = 0;
	Font d = new Font("Arial", 70);
	SolidBrush black = new SolidBrush(Color.Black);
	SolidBrush green = new SolidBrush(Color.Green);
	protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// Getting the graphics object
			Graphics g = pevent.Graphics;

		this.WindowState = FormWindowState.Maximized;
		
		g.FillRectangle(whiteBrush, new Rectangle(0,0,width,height));
		//Image img1 = Image.FromFile("back.jpg");
		//g.DrawImage(img1, 0, 0, this.Width, this.Height);
		g.Clear(Color.Gray);
		if (level == 1)
		{
			Bitmap off = new Bitmap("1.jpg");
			Bitmap fig = new Bitmap("index.png");
			off.MakeTransparent(off.GetPixel(0, 0));
			fig.MakeTransparent(fig.GetPixel(0, 0));
			//MessageBox.Show("in");
			g.DrawImage(off, ClientSize.Width -300, 5, 200, 200);
			
			g.DrawImage(fig, 500, 50, 300, 300);
			int x = 100;
			for (int i = 0; i < 3; i++)
			{
				g.FillRectangle(black, new Rectangle(x, 600, 200, 20));
				x += 245;
			}
			if (progress == 1)
			{
				char flet = 'F';
			//	char ilet = 'I';
				g.DrawString(flet.ToString(), d, black, 120, 510);
			}
			if (progress == 2)
			{
				//	MessageBox.Show("doone");
				char flet = 'F';
				char ilet = 'I';
				g.DrawString(flet.ToString(), d, black, 120, 510);
				g.DrawString(ilet.ToString(), d, black, 346, 510);
			}
			if (progress == 3)
			{
				//	MessageBox.Show("doone");
				char glet = 'G';
				char ilet = 'I';
				char flet = 'F';
				//Bitmap off3 = new Bitmap("f.png");
				//	off3.MakeTransparent(off3.GetPixel(0, 0));
				//g.DrawImage(off3, 120, 530, 120, 60);
				g.DrawString(flet.ToString(), d, green, 120, 510);
				//	Bitmap off4 = new Bitmap("i.png");
				//off4.MakeTransparent(off4.GetPixel(0, 0));
				//	g.DrawImage(off4, 346, 510, 120, 100);
				g.DrawString(ilet.ToString(), d, green, 346, 510);
				//Bitmap g2 = new Bitmap("g.png");
				//g2.MakeTransparent(g2.GetPixel(0, 0));
				//g.DrawImage(g2, 625, 510, 120, 100);
				g.DrawString(glet.ToString(), d,green, 625,510);
				level = 2;
				progress = 0;
				MessageBox.Show("Congratulations you passed level 1");
			}


		}
		if (level == 2)
		{
			Bitmap off = new Bitmap("2.jpg");
			Bitmap fig = new Bitmap("apple.jpg");
			off.MakeTransparent(off.GetPixel(0, 0));
			fig.MakeTransparent(fig.GetPixel(0, 0));
			//MessageBox.Show("in");
			g.DrawImage(off, ClientSize.Width - 300, 5, 200, 200);

			g.DrawImage(fig, 500, 50, 300, 300);
			int x = 100;
			for (int i = 0; i < 5; i++)
			{
				g.FillRectangle(black, new Rectangle(x, 600, 200, 20));
				x += 245;
			}
			if (progress == 1)
			{
				char flet = 'A';
				//	char ilet = 'I';
				g.DrawString(flet.ToString(), d, black, 120, 510);
			}
			if (progress == 2)
			{

				char flet = 'A';
				char ilet = 'P';
				g.DrawString(flet.ToString(), d, black, 120, 510);
				g.DrawString(ilet.ToString(), d, black, 346, 510);
			}
			if (progress == 3)
			{

				char glet = 'P';
				char ilet = 'P';
				char flet = 'A';

				g.DrawString(flet.ToString(), d, black, 120, 510);

				g.DrawString(ilet.ToString(), d, black, 346, 510);

				g.DrawString(glet.ToString(), d, black, 625, 510);

			}
			if (progress == 4)
			{
				char glet = 'P';
				char ilet = 'P';
				char flet = 'A';
				char llet = 'L';
				g.DrawString(flet.ToString(), d, black, 120, 510);

				g.DrawString(ilet.ToString(), d, black, 346, 510);

				g.DrawString(glet.ToString(), d, black, 625, 510);
				g.DrawString(llet.ToString(), d, black, 860, 510);

			}
			if (progress == 5)
			{
				char glet = 'P';
				char ilet = 'P';
				char flet = 'A';
				char llet = 'L';
				char elet = 'E';
				g.DrawString(flet.ToString(), d, green, 120, 510);

				g.DrawString(ilet.ToString(), d, green, 346, 510);
				g.DrawString(elet.ToString(), d, green, 1095, 510);
				g.DrawString(glet.ToString(), d, green, 625, 510);
				g.DrawString(llet.ToString(), d, green, 860, 510);
				level = 3;
				progress = 0;
				MessageBox.Show("Congratulations you passed level 2");

			}
		}
			if (level == 3)
			{
				Bitmap off = new Bitmap("3.jpg");
				Bitmap fig = new Bitmap("banana.png");
				off.MakeTransparent(off.GetPixel(0, 0));
				fig.MakeTransparent(fig.GetPixel(0, 0));
				//MessageBox.Show("in");
				g.DrawImage(off, ClientSize.Width - 300, 5, 200, 200);

				g.DrawImage(fig, 500, 50, 300, 300);
				int x = 100;
				for (int i = 0; i < 6; i++)
				{
					g.FillRectangle(black, new Rectangle(x, 600, 200, 20));
					x += 245;
				}
				if (progress == 1)
				{
					char flet = 'B';
					//	char ilet = 'I';
					g.DrawString(flet.ToString(), d, black, 120, 510);
				}
				if (progress == 2)
				{

					char flet = 'B';
					char ilet = 'A';
					g.DrawString(flet.ToString(), d, black, 120, 510);
					g.DrawString(ilet.ToString(), d, black, 346, 510);
				}
				if (progress == 3)
				{

					char glet = 'B';
					char ilet = 'A';
					char flet = 'N';

					g.DrawString(glet.ToString(), d, black, 120, 510);

					g.DrawString(ilet.ToString(), d, black, 346, 510);

					g.DrawString(flet.ToString(), d, black, 625, 510);

				}
				if (progress == 4)
				{
					char glet = 'B';
					char ilet = 'A';
					char flet = 'N';
					//char llet = 'A';
					g.DrawString(glet.ToString(), d, black, 120, 510);

					g.DrawString(ilet.ToString(), d, black, 346, 510);

					g.DrawString(flet.ToString(), d, black, 625, 510);
					g.DrawString(ilet.ToString(), d, black, 860, 510);

				}
				if (progress == 5)
				{
					char glet = 'B';
					char ilet = 'A';
					char flet = 'N';
					
				
					g.DrawString(glet.ToString(), d, black, 120, 510);

					g.DrawString(ilet.ToString(), d, black, 346, 510);
					
					g.DrawString(flet.ToString(), d, black, 625, 510);
					g.DrawString(ilet.ToString(), d, black, 860, 510);
				g.DrawString(flet.ToString(), d, black, 1095, 510);



			}
			if (progress == 6)
			{
				char glet = 'B';
				char ilet = 'A';
				char flet = 'N';
				
			
				g.DrawString(glet.ToString(), d, green, 120, 510);

				g.DrawString(ilet.ToString(), d, green, 346, 510);
				
				g.DrawString(flet.ToString(), d, green, 625, 510);
				g.DrawString(ilet.ToString(), d, green, 860, 510);
				g.DrawString(flet.ToString(), d, green, 1095, 510);
				g.DrawString(ilet.ToString(), d, green, 1300, 510);


			}



		}
		// draw the cursor path
		if (cursorList.Count > 0) {
 			 lock(cursorSync) {
			 foreach (TuioCursor tcur in cursorList.Values) {
					List<TuioPoint> path = tcur.Path;
					TuioPoint current_point = path[0];

					for (int i = 0; i < path.Count; i++) {
						TuioPoint next_point = path[i];
						g.DrawLine(fingerPen, current_point.getScreenX(width), current_point.getScreenY(height), next_point.getScreenX(width), next_point.getScreenY(height));
						current_point = next_point;
					}
					g.FillEllipse(grayBrush, current_point.getScreenX(width) - height / 100, current_point.getScreenY(height) - height / 100, height / 50, height / 50);
					Font font = new Font("Arial", 10.0f);
					g.DrawString(tcur.CursorID + "", font, blackBrush, new PointF(tcur.getScreenX(width) - 10, tcur.getScreenY(height) - 10));
				}
			}
		 }

			// draw the objects
			if (objectList.Count > 0)
			{
 				lock(objectSync) {
					foreach (TuioDemoObject tobject in objectList.Values) {
						tobject.paint(g,ref progress, ref level);
					}
				}
			}
		}

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // TuioDemo
            // 
            this.ClientSize = new System.Drawing.Size(757, 546);
            this.Name = "TuioDemo";
            this.ResumeLayout(false);

    }

    public static void Main(String[] argv) {
	 		int port = 0;
			switch (argv.Length) {
				case 1:
					port = int.Parse(argv[0],null);
					if(port==0) goto default;
					break;
				case 0:
					port = 3333;
					break;
				default:
					Console.WriteLine("usage: java TuioDemo [port]");
					System.Environment.Exit(0);
					break;
			}
			
			TuioDemo app = new TuioDemo(port);
			Application.Run(app);
		}
	}
