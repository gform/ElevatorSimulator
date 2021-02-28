using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using LiftSimulator.Custom_classes;

namespace LiftSimulator
{
    public partial class NewPassengerButton : Button
    {
        #region FIELDS

        private int floorIndex; //index of floor which button is assigned to
        [Description("Floor which button is assigned to.")]
        public int FloorIndex
        {
            get { return floorIndex; }
            set { floorIndex = value; }
        }
                
        public Form1 MyForm 
        { 
            get { return (Form1)this.FindForm(); }
            private set {} 
        }

        public Floor MyFloor
        {
            get { return (MyForm.MyBuilding.ArrayOfAllFloors[this.floorIndex]); }
            private set { }
        }

        public LogWriter logWriter;

        #endregion FIELDS


        #region METHODS

        public NewPassengerButton()
        {
            InitializeComponent();
            logWriter = new LogWriter();
        }               

        #endregion METHODS


        #region EVENT HANDLERS
        
        private void NewPassengerButton_Click(object sender, EventArgs e)
        {
            if (sender is NewPassengerButton) //upcast sender to NewPassengerButton
            {
                NewPassengerButton ThisPassengerButton = (NewPassengerButton)sender;

                //Check if there is enough space to add another passenger to the floor
                if (MyFloor.GetCurrentAmmountOfPeopleInTheQueue() >= MyFloor.GetMaximumAmmountOfPeopleInTheQueue()) 
                {
                    MessageBox.Show("It looks like the corridor is too crowdy now. Please, wait a while until elevators take few passengers away.", "Your passenger has to wait");
                    return;
                }

                //Where the passenger is going to?
                FloorSelectionDialog dialog = new FloorSelectionDialog(); //create new dialog
                List<int> allFloorsButThis = new List<int>();
                Random random = new Random();
                for (int i = 0; i < MyForm.MyBuilding.ArrayOfAllFloors.Length; i++) //populate combo box with list of available floors
                {
                    if (i != FloorIndex) //skip current floor
                    {
                        dialog.ListOfFloorsInComboBox.Add(i);
                        allFloorsButThis.Add(i);
                    }                    
                }

                //Select random floor by default
                dialog.SelectedFloorIndex = allFloorsButThis[random.Next(0, 3)];

                ////Select "0" floor by default (or "1", if floorIndex is "0")
                //if (FloorIndex == 0)
                //{
                //    dialog.SelectedFloorIndex = 1;
                //}
                //else
                //{
                //    dialog.SelectedFloorIndex = 0;
                //}   

                DialogResult result = dialog.ShowDialog(); //check dialog result
                if (result == DialogResult.OK)
                {
                    //Create new Passenger object                    
                    Passenger NewPassenger = new Passenger(MyForm.MyBuilding, this.MyFloor, dialog.SelectedFloorIndex);
                    //Rise an event
                    this.MyFloor.OnNewPassengerAppeared(new PassengerEventArgs(NewPassenger));
                    //logWriter.Log($"New passenger ({NewPassenger.Id}) appeared");
                }                
            }
        }

        #endregion EVENT HANDLERS
    }
}
