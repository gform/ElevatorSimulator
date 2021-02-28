using LiftSimulator.Custom_classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LiftSimulator
{
    public partial class Form1 : Form
    {
        #region FIELDS

        public Building MyBuilding;
        public LogWriter logWriter;

        #endregion FIELDS


        #region METHODS

        public Form1()
        {
            InitializeComponent();

            //Initialize Building object
            MyBuilding = new Building();
            logWriter = new LogWriter();
        }

        private void PaintBuilding(Graphics g)
        {
            g.DrawImage(Properties.Resources.Building, 12, 12, 983, 659);
        }
        private void PaintFire(Graphics g)
        {
            g.DrawImage(Properties.Resources.Fire, 323, 55, 133, 164);
            g.DrawImage(Properties.Resources.Ladder, 743, 234, 47, 423);
            g.DrawImage(Properties.Resources.Fire_truck, 811, 533, 184, 128);
        }


        private void PaintElevators(Graphics g)
        {
            for (int i = 0; i < MyBuilding.ArrayOfAllElevators.Length; i++)
            {
                Elevator ElevatorToPaint = MyBuilding.ArrayOfAllElevators[i];
                g.DrawImage(ElevatorToPaint.GetCurrentFrame(), ElevatorToPaint.GetElevatorXPosition(), ElevatorToPaint.GetElevatorYPosition(), 54, 90);
            }
        }

        private void PaintPassengers(Graphics g)
        {
            List<Passenger> CopyOfListOfAllPeopleWhoNeedAnimation = new List<Passenger>(MyBuilding.ListOfAllPeopleWhoNeedAnimation);

            foreach (Passenger PassengerToPaint in CopyOfListOfAllPeopleWhoNeedAnimation)
            {
                if ((PassengerToPaint != null) && (PassengerToPaint.GetPassengerVisibility()))
                {
                    g.DrawImage(PassengerToPaint.GetCurrentFrame(), PassengerToPaint.PassengerPosition.X, PassengerToPaint.PassengerPosition.Y + 26, 32, 64); // (35, 75) Y + 15, because passenger is 15 pixels lower than elevator
                }
            }
        }

        #endregion METHODS


        #region EVENT HANDLERS

        private void timerRefresh_Tick(object sender, EventArgs e)
        {
            //Invalidate the form, so it fires Paint event
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            //Redraw all
            PaintBuilding(g);
            PaintElevators(g);
            PaintPassengers(g);
            if (MyBuilding.Fire == true) PaintFire(g);
        }

        #endregion EVENT HANDLERS

        private void fireButton_Click(object sender, EventArgs e)
        {
            if (MyBuilding.Fire == false)
            {
                MyBuilding.Fire = true;
                logWriter.Log("BUILDING IS ON FIRE");
            }
            else
            {
                MyBuilding.Fire = false;
                logWriter.Log("FIRE HAS BEEN EXTINGUISHED");
            } 
                
        }
    }
}
