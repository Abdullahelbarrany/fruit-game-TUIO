/*
	TUIO C# Demo - part of the reacTIVision project
	http://reactivision.sourceforge.net/

	Copyright (c) 2005-2009 Martin Kaltenbrunner <martin@tuio.org>

	This program is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	This program is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with this program; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
*/

using System;
using System.Drawing;
using TUIO;

	public class TuioDemoObject : TuioObject
	{

		SolidBrush black = new SolidBrush(Color.Black);
	Font d = new Font("Arial", 70);
		SolidBrush white = new SolidBrush(Color.White);

		public TuioDemoObject (long s_id, int f_id, float xpos, float ypos, float angle) : base(s_id,f_id,xpos,ypos,angle) {
		}

		public TuioDemoObject (TuioObject o) : base(o) {
		}
	int ct = 0;
	char glet ='G';
	public void checklevel1(int x, int y, int s, int p)
	{ if (p == 0)
		{
			if (x - s > 100 && x - s < 300)
			{
				ct = 1;
			}
		}
		else if (p == 1)
		{
			if (x - s > 320 && x - s < 545)
			{
				ct = 2;
			}

		}
		else if (p == 2)
		{
			if (x - s > 546 && x - s < 800)
			{
				ct = 3;
			}
		}
		else
		{ ct = 0; }



	}
	void checklevel2(int x, int y, int s, int p)
	{
		if (p == 0)
		{
			if (x - s > 100 && x - s < 300)
			{
				ct = 1;
			}
		}
		else if (p == 1)
		{
			if (x - s > 320 && x - s < 545)
			{
				ct = 2;
			}

		}
		else if (p == 2)
		{
			if (x - s > 546 && x - s < 800)
			{
				ct = 3;
			}
		}
		else if (p == 3)
		{
			if (x - s > 546)
			{
				ct = 4;
			}
		}
		else if (p == 4)
		{
			if (x - s > 546)
			{
				ct = 5;
			}
		}
		else
		{ ct = 0; }

	}
	void checklevel3(int x, int y, int s, int p)
	{
		if (p == 0)
		{
			if (x - s > 100 && x - s < 300)
			{
				ct = 1;
			}
		}
		else if (p == 1)
		{
			if (x - s > 320 && x - s < 545)
			{
				ct = 2;
			}

		}
		else if (p == 2)
		{
			if (x - s > 546 && x - s < 800)
			{
				ct = 3;
			}
		}
		else if (p == 3)
		{
			if (x - s > 546)
			{
				ct = 4;
			}
		}
		else if (p == 4)
		{
			if (x - s > 546)
			{
				ct = 5;
			}
		}
		else if (p == 5)
		{
			if (x - s > 546)
			{
				ct = 6;
			}
		}
		else
		{ ct = 0; }

	}
	public void paint(Graphics g,ref int p,ref int level) {

			int Xpos = (int)(xpos*TuioDemo.width);
			int Ypos = (int)(ypos*TuioDemo.height);
			int size = TuioDemo.height/10;

			g.TranslateTransform(Xpos,Ypos);
			g.RotateTransform((float)(angle/Math.PI*180.0f));
			g.TranslateTransform(-1*Xpos,-1*Ypos);
		if (symbol_id == 0)
		{
			char alet = 'A';
			g.DrawString(alet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 2)
			{
				checklevel2(Xpos, Ypos, size, p);
				if (ct == 1)
				{
					p = 1;  
				}
			}
			if (level == 3)
			{
				checklevel3(Xpos, Ypos, size, p);
				if (ct == 2)
				{
					p = 2;
				}
				else if (ct == 4)
				{ p = 4; }
				else if (ct == 6)
				{
					p = 6; 
				}
			}
		}
		if (symbol_id == 1)
		{
			char alet = 'B';
			g.DrawString(alet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 3)
			{
				checklevel3(Xpos, Ypos, size, p);
				if (ct == 1)
				{
					p = 1;
				}
			}
		}
		if (symbol_id == 13)
		{
			char alet = 'N';
			g.DrawString(alet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 3)
			{
				checklevel3(Xpos, Ypos, size, p);
				if (ct == 3)
				{
					p = 3;
				}
				else if (ct == 5)
				{ p = 5; }
			}
		}
		if (symbol_id == 4)
		{
			char llet = 'E';
			g.DrawString(llet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 2)
			{
				checklevel2(Xpos, Ypos, size, p);
				if (ct == 5)
				{
					p = 5;
				}

			}
		}
		if (symbol_id == 11)
		{
			char llet = 'L';
			g.DrawString(llet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 2)
			{
				checklevel2(Xpos, Ypos, size, p);
				if (ct == 4)
				{
					p = 4;
				}
				
			}
		}
		if (symbol_id == 15)
		{
			char alet = 'p';
			g.DrawString(alet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 2)
			{
				checklevel2(Xpos, Ypos, size, p);
				if (ct == 2)
				{
					p = 2;
				}
				else if (ct == 3)
				{
					p = 3; 
				}
			}

		}
		if (symbol_id == 5)
		{
			char flet = 'F';
			///Bitmap off = new Bitmap("f.png");
			///,//off.MakeTransparent(off.GetPixel(0, 0));
			//g.DrawImage(off, Xpos - size / 2, Ypos - size / 2, 100, 100);
			g.DrawString(flet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 1)
			{
				checklevel1(Xpos, Ypos, size, p);
				if (ct == 1)
				{
					p = 1;
				}
			}
		}
		if (symbol_id == 6)
		{
			//Bitmap off = new Bitmap("g.png");
		//	off.MakeTransparent(off.GetPixel(0, 0));
			//g.DrawImage(off, Xpos - size / 2, Ypos - size / 2, 100, 100);
			g.DrawString(glet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 1)
			{
				checklevel1(Xpos, Ypos, size, p);
				if (ct == 3)
				{

					p = 3;
				}
			}
		}
		if (symbol_id == 8)
		{
			char ilet = 'I';
			//Bitmap off = new Bitmap("i.png");
		///	off.MakeTransparent(off.GetPixel(0, 0));
			//g.DrawImage(off, Xpos - size / 2, Ypos - size / 2, 100, 100);
			g.DrawString(ilet.ToString(), d, black, Xpos - size / 2, Ypos - size / 2);
			if (level == 1)
			{
				checklevel1(Xpos, Ypos, size, p);
				if (ct == 2)
				{

					p = 2;
				}
			}
		}
		//g.FillRectangle(black, new Rectangle(Xpos-size/2,Ypos-size/2,size,size));

		g.TranslateTransform(Xpos,Ypos);
			g.RotateTransform(-1*(float)(angle/Math.PI*180.0f));
			g.TranslateTransform(-1*Xpos,-1*Ypos);

			Font font = new Font("Arial", 10.0f);
			//g.DrawString(symbol_id+"",font, white, new PointF(Xpos-10,Ypos-10));
		}

	}
