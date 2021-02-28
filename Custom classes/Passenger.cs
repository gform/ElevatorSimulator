using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Drawing;
using System.Timers;
using LiftSimulator.Custom_classes;

namespace LiftSimulator
{
    public class Passenger
    {
        #region FIELDS

        private readonly object locker = new object();

        static Bitmap[] ArrayOfAllPassengerGraphics =
        {
            new Bitmap(Properties.Resources.Bart_Simpson),
            new Bitmap(Properties.Resources.Batman),
            new Bitmap(Properties.Resources.Charlie_Brown),
            new Bitmap(Properties.Resources.Fred_Flintstone),
            new Bitmap(Properties.Resources.Homer_Sompson),
            new Bitmap(Properties.Resources.Jerry_mouse),
            new Bitmap(Properties.Resources.Mickey_Mouse),
            new Bitmap(Properties.Resources.Popeye),
            new Bitmap(Properties.Resources.Smurfette),
            new Bitmap(Properties.Resources.Spongebob),
            new Bitmap(Properties.Resources.Yogi_Bear)
        };

        private Building myBuilding;

        private Floor currentFloor;
        private int currentFloorIndex;
        public Direction PassengerDirection;
        private PassengerStatus passengerStatus;

        private Floor targetFloor;
        private int targetFloorIndex;

        public Point PassengerPosition;
        private Bitmap thisPassengerGraphic;
        private bool visible;
        private int passengerAnimationDelay;

        private Elevator myElevator;

        private System.Timers.Timer passengerTimer;

        public LogWriter logWriter;
        private static int objCounter = 0;
        public int Id { get; set; }

        #endregion


        #region METHODS

        public Passenger(Building MyBuilding, Floor CurrentFloor, int TargetFloorIndex)
        {
            this.myBuilding = MyBuilding;

            this.currentFloor = CurrentFloor;
            this.currentFloorIndex = CurrentFloor.FloorIndex;
            this.passengerStatus = PassengerStatus.WaitingForAnElevator;

            this.targetFloor = MyBuilding.ArrayOfAllFloors[TargetFloorIndex];
            this.targetFloorIndex = TargetFloorIndex;

            this.PassengerPosition = new Point();

            Random random = new Random();
            this.thisPassengerGraphic = new Bitmap(Passenger.ArrayOfAllPassengerGraphics[random.Next(ArrayOfAllPassengerGraphics.Length)]);

            this.visible = true;
            this.passengerAnimationDelay = 8;

            //Subscribe to events
            this.currentFloor.NewPassengerAppeared += new EventHandler(currentFloor.Floor_NewPassengerAppeared);
            this.currentFloor.NewPassengerAppeared += new EventHandler(this.Passenger_NewPassengerAppeared);
            this.currentFloor.ElevatorHasArrivedOrIsNotFullAnymore += new EventHandler(this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore);

            this.passengerTimer = new System.Timers.Timer(1000);
            this.passengerTimer.Elapsed += new ElapsedEventHandler(this.Passenger_TimerElapsed);
            this.passengerTimer.Start();

            logWriter = myBuilding.logWriter;
            this.Id = System.Threading.Interlocked.Increment(ref objCounter);
        }

        private void FindAnElevatorOrCallForANewOne()
        {
            if (myBuilding.Fire == false)
            {
                UpdatePassengerDirection();

                //Copy the list of elevators available now on current floor
                List<Elevator> ListOfElevatorsWaitingOnMyFloor = currentFloor.GetListOfElevatorsWaitingHere();

                //Search the right elevator on my floor
                foreach (Elevator elevator in ListOfElevatorsWaitingOnMyFloor)
                {
                    if (ElevatorsDirectionIsNoneOrOk(elevator))
                    {
                        if (elevator.AddNewPassengerIfPossible(this, this.targetFloor) && this.passengerStatus == PassengerStatus.WaitingForAnElevator)
                        {
                            //Update insideTheElevator
                            this.passengerStatus = PassengerStatus.GettingInToTheElevator;

                            ThreadPool.QueueUserWorkItem(delegate { GetInToTheElevator(elevator); });
                            return;
                        }
                    }
                }

                //Call for an elevator
                myBuilding.ElevatorManager.PassengerNeedsAnElevator(currentFloor, this.PassengerDirection);
            }
        }

        private void GetInToTheElevator(Elevator ElevatorToGetIn)
        {
            logWriter.Log($"Passenger ({Id}) is boarding elevator ({ElevatorToGetIn.Id}).");
            //Rise an event
            ElevatorToGetIn.OnPassengerEnteredTheElevator(new PassengerEventArgs(this));

            //Unsubscribe from an event for current floor
            this.currentFloor.ElevatorHasArrivedOrIsNotFullAnymore -= this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore;

            //Move the picture on the UI
            this.MovePassengersGraphicHorizontally(ElevatorToGetIn.GetElevatorXPosition());

            //Make PassengerControl invisible
            this.visible = false;

            //Update myElevator
            this.myElevator = ElevatorToGetIn;
        }

        public void ElevatorReachedNextFloor()
        {
            //For passengers, who are already inside an elevator:
            if (this.myElevator.GetCurrentFloor() == this.targetFloor || myBuilding.Fire == true)
            {
                //Set appropriate flag
                this.passengerStatus = PassengerStatus.LeavingTheBuilding;

                //Get out of the elevator
                ThreadPool.QueueUserWorkItem(delegate { GetOutOfTheElevator(this.myElevator); });
            }
        }

        private void GetOutOfTheElevator(Elevator ElevatorWhichArrived)
        {
            //Remove passenger from elevator
            ElevatorWhichArrived.RemovePassenger(this);

            //Leave the building
            this.LeaveTheBuilding();
        }

        private void UpdatePassengerDirection()
        {
            if (currentFloorIndex < targetFloorIndex)
            {
                this.PassengerDirection = Direction.Up;
            }
            else
            {
                this.PassengerDirection = Direction.Down;
            }
        }

        private bool ElevatorsDirectionIsNoneOrOk(Elevator ElevatorOnMyFloor)
        {
            //Check if elevator has more floors to visit            
            if (ElevatorOnMyFloor.GetElevatorDirection() == this.PassengerDirection)
            {
                return true; //Elevator direction is OK
            }
            else if (ElevatorOnMyFloor.GetElevatorDirection() == Direction.None)
            {
                return true; //If an elevator has no floors to visit, then it is always the right elevator
            }

            return false; //Elevator direction is NOT OK
        }

        private void LeaveTheBuilding()
        {
            logWriter.Log($"Passenger ({Id}) is leaving the building on floor ({currentFloor.FloorIndex})");
            //Update starting position
            this.PassengerPosition = new Point(PassengerPosition.X, myElevator.GetElevatorYPosition());

            //Flip the control
            this.FlipPassengerGraphicHorizontally();

            //Make the passenger visible            
            this.visible = true;

            //Move the passenger up to the exit
            this.MovePassengersGraphicHorizontally(myBuilding.ExitLocation);

            //Make the passenger invisible again 
            //TO DO: dispose object instead making it invisble
            this.visible = false;

            //No need to animate it
            myBuilding.ListOfAllPeopleWhoNeedAnimation.Remove(this);
        }
        public void EvacuateTheBuilding()
        {
            //Unsubscribe from an event for current floor
            this.currentFloor.ElevatorHasArrivedOrIsNotFullAnymore -= this.Passenger_ElevatorHasArrivedOrIsNoteFullAnymore;
            
            this.passengerStatus = PassengerStatus.LeavingTheBuilding;
            
            logWriter.Log($"Passenger ({Id}) is fleeing the building on floor ({currentFloor.FloorIndex})");
            
            currentFloor.LampDown = false;
            currentFloor.LampUp = false;
            
            int PassengerToRemoveIndex = Array.IndexOf<Passenger>(currentFloor.GetArrayOfPeopleWaitingForElevator(), this);
            if (PassengerToRemoveIndex != -1)
            {
                currentFloor.GetArrayOfPeopleWaitingForElevator()[PassengerToRemoveIndex] = null;
            }
            
            //Flip the control
            this.visible = false;
            this.FlipPassengerGraphicHorizontally();
            this.visible = true;

            ////Make the passenger visible            
            //this.visible = true;

            //Move the passenger up to the exit
            this.MovePassengersGraphicHorizontally(myBuilding.ExitLocation);

            //Make the passenger invisible again 
            //TO DO: dispose object instead making it invisble
            this.visible = false;

            //No need to animate it
            myBuilding.ListOfAllPeopleWhoNeedAnimation.Remove(this);
        }

        private void MovePassengersGraphicHorizontally(int DestinationPosition)
        {
            if (this.PassengerPosition.X > DestinationPosition) //go left
            {
                for (int i = this.PassengerPosition.X; i > DestinationPosition; i--)
                {
                    Thread.Sleep(this.passengerAnimationDelay);
                    this.PassengerPosition = new Point(i, this.PassengerPosition.Y);
                }
            }
            else //go right
            {
                for (int i = this.PassengerPosition.X; i < DestinationPosition; i++)
                {
                    Thread.Sleep(this.passengerAnimationDelay);
                    this.PassengerPosition = new Point(i, this.PassengerPosition.Y);
                }
            }
        }

        private void FlipPassengerGraphicHorizontally()
        {
            this.thisPassengerGraphic.RotateFlip(RotateFlipType.Rotate180FlipY);
        }

        public Floor GetTargetFloor()
        {
            return this.targetFloor;
        }

        public bool GetPassengerVisibility()
        {
            return this.visible;
        }

        public int GetAnimationDelay()
        {
            return this.passengerAnimationDelay;
        }

        public Bitmap GetCurrentFrame()
        {
            return this.thisPassengerGraphic;
        }



        #endregion


        #region EVENT HANDLERS

        public void Passenger_NewPassengerAppeared(object sender, EventArgs e)
        {
            //Unsubscribe from this event (not needed anymore)            
            this.currentFloor.NewPassengerAppeared -= this.Passenger_NewPassengerAppeared;

            //Search an elevator
            FindAnElevatorOrCallForANewOne();
        }

        public void Passenger_ElevatorHasArrivedOrIsNoteFullAnymore(object sender, EventArgs e)
        {
            lock (locker) //Few elevators (on different threads) can rise this event at the same time
            {
                Elevator ElevatorWhichRisedAnEvent = ((ElevatorEventArgs)e).ElevatorWhichRisedAnEvent;

                //For passengers who are getting in to the elevator and may not be able to unsubscribe yet                
                if (this.passengerStatus == PassengerStatus.GettingInToTheElevator)
                {
                    return;
                }

                //For passengers, who await for an elevator
                if (this.passengerStatus == PassengerStatus.WaitingForAnElevator && myBuilding.Fire == false)
                {
                    if ((ElevatorsDirectionIsNoneOrOk(ElevatorWhichRisedAnEvent) && (ElevatorWhichRisedAnEvent.AddNewPassengerIfPossible(this, targetFloor))))
                    {
                        //Set passengerStatus
                        passengerStatus = PassengerStatus.GettingInToTheElevator;

                        //Get in to the elevator
                        ThreadPool.QueueUserWorkItem(delegate { GetInToTheElevator(ElevatorWhichRisedAnEvent); });
                    }
                    else
                    {
                        FindAnElevatorOrCallForANewOne();
                    }
                }
            }
        }

        public void Passenger_TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if(this.passengerStatus == PassengerStatus.WaitingForAnElevator && myBuilding.Fire == true)
            {
                ThreadPool.QueueUserWorkItem(delegate { EvacuateTheBuilding(); });
            }
        }

        #endregion EVENT HANDLERS
    }
}
