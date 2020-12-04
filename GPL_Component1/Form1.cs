using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPL_Component1
{
    public partial class Form1 : Form
    {
      
        //  graphic object defined 
        Graphics panelGraphics;
        Graphics workingGraphics;
        

        // defined stroke size
        private int strokeSize = 1;

        // initialized color object
        Color currentColor;


        // default selected shape button 
        Button selectedShapeButton;

        //  checkbox for is Filled or not 
        Boolean isFilled;


        // defined Point Object
        Point firstPoint;

        // defined firstpoint variable 
        Boolean haveFirstPoint;
        

        // object of Color Dialog 
        ColorDialog colorPicker = new ColorDialog();


    // defined BitMap object 
        Bitmap paintImage;

        // defined bitmap working Image
        Bitmap workingImage;
        // declare variable 
        Point lastPoint;
        bool isMouseDown = false;

        // Defined ShapesFactory object
        ShapesFactory sf;

        public Form1()
        {
            InitializeComponent();
            this.loadPictureBox();
            this.currentColor = Color.Black;
            selectedShapeButton = penButton;
            sf = new ShapesFactory();

        }

    
        public void EnableDrawing()
        {
            this.MouseDown += new MouseEventHandler(Form1_MouseDownDrawing);
        }

        /**
         Mouse DownDrawing event listener 
        */
        void Form1_MouseDownDrawing(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (haveFirstPoint)
            {
                panelGraphics = pictureBox1.CreateGraphics();


                Shape s = sf.getShape("pen");
                s.setColor(this.currentColor);
                s.setPoint(firstPoint, e.Location);
                s.draw(panelGraphics);
                haveFirstPoint = false;
                
            }
            else
            {
                firstPoint = e.Location;
                haveFirstPoint = true;
            }

            
        }

        private Shape DrawShapeInWorkingImage(Point currentPoint)
        {
            Pen pen = new Pen(this.currentColor, strokeSize);

            // Draw outline while drawing shapes
            Pen outLinePen = new Pen(Color.Black);
            outLinePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            workingImage = new Bitmap(paintImage);
            workingGraphics = Graphics.FromImage(workingImage);


            int startPointX = lastPoint.X < currentPoint.X ? lastPoint.X : currentPoint.X;
            int startPointY = lastPoint.Y < currentPoint.Y ? lastPoint.Y : currentPoint.Y;

            int shapeWidth = (lastPoint.X > currentPoint.X ? lastPoint.X : currentPoint.X) - startPointX;
            int shapeHeight = (lastPoint.Y > currentPoint.Y ? lastPoint.Y : currentPoint.Y) - startPointY;

            int[] coOrdinates = { startPointX, startPointY, shapeWidth, shapeHeight };

            Shape myShape = sf.getShape(selectedShapeButton.Text);

            myShape.setStroke(strokeSize);
            
            if(myShape is Line)
            {
                myShape.setPoint(lastPoint, currentPoint);
            }

            myShape.set(colorPicker.Color, coOrdinates);

            if (isMouseDown)
            {
                workingGraphics.DrawRectangle(outLinePen, startPointX, startPointY, shapeWidth, shapeHeight);
            }
            myShape.setFill(isFilled);
            myShape.draw(workingGraphics);
            pictureBox1.Refresh();
            pictureBox1.Image = workingImage;
            return myShape;

        }


        void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            Shape s = DrawShapeInWorkingImage(e.Location);
            s.draw(panelGraphics);
            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();
            
            
        }



        private void TrackCoordinates()
        {
            ToolTip trackTip = new ToolTip();
            this.MouseMove += new MouseEventHandler(pictureBox1_MouseMove);
        }

      
     


        // pictureBox1 MouseDown event
        void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            lastPoint = e.Location;
            isMouseDown = true;
            



        }

        // make canvas to drawlines in pictureBox1
        private void DrawLineInCanvas(Point currentPoint)
        {
            
            // Form pen with the color selected and the size value in tracker
            

            Shape line = sf.getShape("pen");
            line.setColor(colorPicker.Color);
            line.setStroke(strokeSize);
            line.setPoint(lastPoint, currentPoint);
            
            line.draw(panelGraphics);
            lastPoint = currentPoint;

            /*pictureBox1.Image = workingImage;*/
            pictureBox1.Refresh();
            
        }

 


        private void loadPictureBox()
        {

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;

            paintImage = new Bitmap(width, height);

            panelGraphics = Graphics.FromImage(paintImage);

            panelGraphics.FillRectangle(Brushes.White,0, 0, width, height);

            pictureBox1.Image = paintImage;

            pictureBox1.MouseDown += new MouseEventHandler(pictureBox1_MouseDown); // picture Box mouse down
            pictureBox1.MouseMove += new MouseEventHandler(pictureBox1_MouseMove); // picture box mouse move
            pictureBox1.MouseUp += new MouseEventHandler(pictureBox1_MouseUp); // picture box mouse up

            workingImage = paintImage;


        }
    

        private void runButton_Click(object sender, EventArgs e)
        {
            string commandValue = commandInputBox.Text;
            InputParser ip = new InputParser(panelGraphics, commandValue);
            pictureBox1.Image = workingImage;
            pictureBox1.Refresh();
            commandInputBox.Text = "";


        }

        private void resetBtn_Click(object sender, EventArgs e)
        {
            loadPictureBox();
        }

        private void colorPickBtn_Click(object sender, EventArgs e)
        {

           colorPicker.ShowDialog();
        }

        

        private void penButton_Click(object sender, EventArgs e)
        {
            selectedShapeButton = penButton;
        }

        private void circleButtonSelect_Click(object sender, EventArgs e)
        {
            selectedShapeButton = circleButtonSelect;
        }

        private void rectangleBtnSelect_Click(object sender, EventArgs e)
        {
            selectedShapeButton = rectangleBtnSelect;
        }

        private void squareBtnSelect_Click(object sender, EventArgs e)
        {
            selectedShapeButton = squareBtnSelect;
        }

        private void triangleBtnSelect_Click(object sender, EventArgs e)
        {
            selectedShapeButton = triangleBtnSelect;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            String tipText = String.Format("({0}, {1})", e.X, e.Y);

            if (isMouseDown)
            {
                toolTip1.Show(tipText, this, e.Location);
                if (selectedShapeButton.Text == "Pen")
                {
                    DrawLineInCanvas(e.Location);
                }
                else
                {
                   DrawShapeInWorkingImage(e.Location);
                
                }
            }
        }

        private void penTrackBar_ValueChanged(object sender, EventArgs e)
        {
            strokeSize = penTrackBar.Value;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.ShowDialog();
            
            paintImage = new Bitmap(fd.FileName);
            panelGraphics = Graphics.FromImage(paintImage);

            workingImage = new Bitmap(fd.FileName);
            panelGraphics = Graphics.FromImage(workingImage);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.ShowDialog();
            workingImage.Save(sf.FileName);
            /*paintImage.Save(sf.FileName);*/
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            isFilled = checkBox1.Checked;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
