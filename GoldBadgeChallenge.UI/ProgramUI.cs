using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;
using GoldBadgeChallenge.Repository;
using static System.Console;

public class ProgramUI
{
    private readonly DeliveryRepository _deliveryRepo = new DeliveryRepository();
    private bool IsRunning = true;

    public void RunApplication()
    {
        Seed();
        Run();
    }

    private void Run()
    {
        Clear();
        while(IsRunning)
        {
            WriteLine("========== Warner Transit Federal ==========\n"+
                      "======== Delivery Tracking DataBase ========\n"+"\n"+
                        "1. Add new Delivery\n"+
                        "2. View all deliveries\n"+
                        "3. View Completed deliveries\n"+
                        "4. View Active deliveries\n"+
                        "5. View Scheduled deliveries\n"+
                        "6. View Canceled deliveries\n"+
                        "7. Update a delivery's Information\n"+
                        "8. Update the Status of a delivery\n"+
                        "9. Remove delivery\n"+"\n"+
                        "0. Close Application\n");
            
            var userInput = int.Parse(ReadLine()!);

            switch (userInput)
            {
                case 1:
                AddDelivery();
                break;
                case 2:
                GetAllDeliveries();
                break;
                case 3:
                GetAllCompletedDeliveries();
                break;
                case 4:
                GetAllActiveDeliveries();
                break;
                case 5:
                GetAllScheduledDeliveries();
                break;
                case 6:
                GetAllCanceledDeliveries();
                break;
                case 7:
                UpdateDeliveryInfo();
                break;
                case 8:
                UpdateDeliveryStatus();
                break;
                case 9:
                RemoveDelivery();
                break;
                case 0:
                IsRunning = CloseApplication();
                break;
                default:
                WriteLine("Invalid Selection\n"+
                "Press any key to continue...");
                ReadKey();
                break;
            }
        }
    }

    private void AddDelivery()
    {
        Clear();
        Delivery deliveryInfo = FillOutDeliveryForm();

        if(_deliveryRepo.AddDelivery(deliveryInfo))
            WriteLine("Delivery has been successfully added!");
        else
            WriteLine("Failed to add Delivery, Please try again.");

        PressAnyKeyToContinue();
    }

    private Delivery FillOutDeliveryForm()
    {
        Clear();
        Delivery delivery = new Delivery();

        Write("Order Date (MM/DD/YYYY): ");
        string userInputOrderDate = ReadLine()!;
        delivery.OrderDate = userInputOrderDate;

        Write("Item Number: ");
        int userInputItemNumber = int.Parse(ReadLine()!);
        delivery.ItemNumber = userInputItemNumber;

        Write("Item Quantity: ");
        int userInputItemQuantity = int.Parse(ReadLine()!);
        delivery.ItemQuantity = userInputItemQuantity;

        Write("Customer Id: ");
        int userInputCustomerId = int.Parse(ReadLine()!);
        delivery.CustomerId = userInputCustomerId;

        Write("Order Status (Scheduled, EnRoute, Complete, Canceled): ");
        string userInputStatus = ReadLine()!;
        delivery.OrderStatus = userInputStatus;
        if(CheckComplete(userInputStatus))
        {
            Write("Delivery Date (MM/DD/YYYY): ");
            string userInputDeliveryDate = ReadLine()!;
            delivery.DeliveryDate = userInputDeliveryDate;
        }
        else if(CheckCanceled(userInputStatus))
        {
            delivery.DeliveryDate = "N/A";
        }
        else 
        {
            delivery.DeliveryDate = "pending";
        }
        
        return delivery;
    }

    private bool CheckCanceled(string userInputStatus)
    {
        if (userInputStatus.ToLower() == "Canceled".ToLower())
            return true;
        else
            return false;
    }

    private bool CheckComplete(string userInputStatus)
    {
        if (userInputStatus.ToLower() == "Complete".ToLower())
            return true;
        else
            return false;
    }

    private void GetAllDeliveries()
    {
        Clear();
        WriteLine("=== Deliveries Listed ===\n");
        RetrieveDeliveryListInfo();
        PressAnyKeyToContinue();
    }

    private void RetrieveDeliveryListInfo()
    {
        var deliveriesInDb = _deliveryRepo.GetDeliveries();
        if(deliveriesInDb.Count == 0)
        {
            WriteLine("Sorry, there are no available deliveries.");
        }
        else
        {
            foreach (var delivery in deliveriesInDb)
            {
                DisplayDeliveryInfo(delivery);
            }
        }
    }

    private void DisplayDeliveryInfo(Delivery delivery)
    {
        WriteLine($"Id: {delivery.Id} - Order Date: {delivery.OrderDate} - Item Number: {delivery.ItemNumber} - Item Quantity: {delivery.ItemQuantity} - Customer Id: {delivery.CustomerId} - Order Status: {delivery.OrderStatus} - Delivery Date: {delivery.DeliveryDate}");
    }

    private void GetAllCompletedDeliveries()
    {
        Clear();
        if(_deliveryRepo.GetCompletedDeliveries().Count() == 0)
        {
            WriteLine("Sorry, there are no Completed Deliveries.");
        }
        else
        {
            List<Delivery> completedDeliveries = _deliveryRepo.GetCompletedDeliveries();
            foreach (Delivery delivery in completedDeliveries)
            {
                DisplayDeliveryInfo(delivery);
            }
        }
        PressAnyKeyToContinue();
    }

    private void GetAllActiveDeliveries()
    {
        Clear();
        if(_deliveryRepo.GetActiveDeliveries().Count() == 0)
        {
            WriteLine("There are no Active Deliveries right now.");
        }
        else
        {
            List<Delivery> activeDeliveries = _deliveryRepo.GetActiveDeliveries();
            foreach (var delivery in activeDeliveries)
            {
                DisplayDeliveryInfo(delivery);
            }
        }
        PressAnyKeyToContinue();
    }

    private void GetAllScheduledDeliveries()
    {
        Clear();
        if(_deliveryRepo.GetScheduledDeliveries().Count() == 0)
        {
            WriteLine("There are no Scheduled Deliveries right now.");
        }
        else
        {
            List<Delivery> scheduledDeliveries = _deliveryRepo.GetScheduledDeliveries();
            foreach (var delivery in scheduledDeliveries)
            {
                DisplayDeliveryInfo(delivery);
            }
        }
        PressAnyKeyToContinue();
    }

    private void GetAllCanceledDeliveries()
    {
        Clear();
        if(_deliveryRepo.GetCanceledDeliveries().Count() == 0)
        {
            WriteLine("There are no Canceled Deliveries.");
        }
        else
        {
            List<Delivery> canceledDeliveries = _deliveryRepo.GetCanceledDeliveries();
            foreach (var delivery in canceledDeliveries)
            {
                DisplayDeliveryInfo(delivery);
            }
        }
        PressAnyKeyToContinue();
    }

    private void UpdateDeliveryInfo()
    {
        Clear();
        RetrieveDeliveryListInfo();

        WriteLine("Please select a Delivery by entering it's Id.");
        int userInputDeliveryId = int.Parse(ReadLine()!);
        Delivery deliveryData = RetrieveDeliveryData(userInputDeliveryId);

        if(deliveryData is null)
            DisplayDataValidationError(userInputDeliveryId);
        else
        {
            Clear();
            WriteLine("=== Update Delivery Form ===\n");

            Delivery newDeliveryData = FillOutDeliveryForm();
            if(_deliveryRepo.EditDeliveryInfo(userInputDeliveryId, newDeliveryData))
                WriteLine("Delivery Info successfully updated!");
            else
                WriteLine("Failure to update Delivery Info.");
        }
        PressAnyKeyToContinue();
    }

    private void UpdateDeliveryStatus()
    {
        Clear();
        RetrieveDeliveryListInfo();

        WriteLine("Please select a Delivery by entering it's Id.");
        int userInputDeliveryId = int.Parse(ReadLine()!);
        Delivery selectedDelivery = RetrieveDeliveryData(userInputDeliveryId);
        if(selectedDelivery is null)
            DisplayDataValidationError(userInputDeliveryId);
        else
        {
            Clear();
            WriteLine($"Current status: {selectedDelivery.OrderStatus}");

            Write("Updated status: ");
            string newDeliveryStatus = ReadLine()!.ToLower();
            switch (newDeliveryStatus)
            {
                case "complete":
                    selectedDelivery.OrderStatus = newDeliveryStatus;

                    Write("Delivery Date (MM/DD/YYYY): ");
                    string userInputDeliveryDate = ReadLine()!;
                    selectedDelivery.DeliveryDate = userInputDeliveryDate;

                    WriteLine($"Delivery {selectedDelivery.Id}'s status is now: '{selectedDelivery.OrderStatus}'.");
                    break;
                case "enroute":
                    selectedDelivery.OrderStatus = newDeliveryStatus;
                    WriteLine($"Delivery {selectedDelivery.Id}'s status is now: '{selectedDelivery.OrderStatus}'.");
                    selectedDelivery.DeliveryDate = "pending";
                    break;
                case "scheduled":
                    selectedDelivery.OrderStatus = newDeliveryStatus;
                    WriteLine($"Delivery {selectedDelivery.Id}'s status is now: '{selectedDelivery.OrderStatus}'.");
                    selectedDelivery.DeliveryDate = "pending";
                    break;
                case "canceled":
                    selectedDelivery.OrderStatus = newDeliveryStatus;
                    WriteLine($"Delivery {selectedDelivery.Id}'s status is now: '{selectedDelivery.OrderStatus}'.");
                    selectedDelivery.DeliveryDate = "N/A";
                    break;
                default:
                    WriteLine("Invalid Order Status");
                    break;
            }
        PressAnyKeyToContinue();
        }
    }

    private void DisplayDataValidationError(int userInputDeliveryId)
    {
        ForegroundColor = ConsoleColor.Red;
        WriteLine($"Invalid Id Entry: {userInputDeliveryId}!");
        ResetColor();
        return;
    }

    private Delivery RetrieveDeliveryData(int userInputDeliveryId)
    {
        Delivery delivery = _deliveryRepo.GetDeliveryById(userInputDeliveryId);
        return delivery;
    }

    private void RemoveDelivery()
    {
        Clear();
        RetrieveDeliveryListInfo();

        WriteLine("Please select a delivery by it's Id.");
        int userInputDeliveryId = int.Parse(ReadLine()!);
        Delivery deliveryData = RetrieveDeliveryData(userInputDeliveryId);

        if (deliveryData == null)
            DisplayDataValidationError(userInputDeliveryId);
        else
        {
            if (_deliveryRepo.DeleteDelivery(deliveryData))
                WriteLine("Delivery successfully deleted!");
            else
                WriteLine("Failure to delete delivery.");
        }
        PressAnyKeyToContinue();
    }

    private bool CloseApplication()
    {
        Clear();
        return false;
    }

    private void PressAnyKeyToContinue()
    {
        WriteLine("Press any key to continue...");
        ReadKey();
        Clear();
    }

    private void Seed()
    {
        Delivery deliveryA = new Delivery("09/22/2023", 111111, 2, 4296, "Complete", "09/24/2023");
        Delivery deliveryB = new Delivery("09/23/2023", 222222, 10, 5111, "EnRoute", "pending");
        Delivery deliveryC = new Delivery("09/15/2023", 333333, 1, 5399, "Scheduled", "pending");
        Delivery deliveryD = new Delivery("04/15/2022", 444444, 1, 2024, "Complete", "05/14/2022");
        Delivery deliveryE = new Delivery("06/20/2023", 555555, 50, 1111, "Canceled", "N/A");
        Delivery deliveryF = new Delivery("08/11/2023", 666666, 5, 1111, "EnRoute", "pending");

        _deliveryRepo.AddDelivery(deliveryA);
        _deliveryRepo.AddDelivery(deliveryB);
        _deliveryRepo.AddDelivery(deliveryC);
        _deliveryRepo.AddDelivery(deliveryD);
        _deliveryRepo.AddDelivery(deliveryE);
        _deliveryRepo.AddDelivery(deliveryF);
    }
}
