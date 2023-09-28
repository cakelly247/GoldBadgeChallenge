using GoldBadgeChallenge.Repository;
using GoldBadgeChallenge.Data;
using static System.Console;

namespace GBRepositoryTests;

public class GBRepositoryTest
{
    private DeliveryRepository deliveryRepo;
    private Delivery deliveryA;
    private Delivery deliveryB;
    private Delivery deliveryC;
    private List<Delivery> testDeliveries;

    public GBRepositoryTest()
    {
        deliveryRepo = new DeliveryRepository();
        deliveryA = new Delivery("04/15/2022", 101122, 1, 2024, "Complete", "05/14/2022");
        deliveryB = new Delivery("06/20/2023", 330677, 10, 1111, "Canceled", "N/A");
        deliveryC = new Delivery("08/11/2023", 447733, 5, 1111, "EnRoute", "pending");
        testDeliveries = new List<Delivery>
        {
            new Delivery("09/22/2023", 313, 2, 4296, "Complete", "09/24/2023"),
            new Delivery("09/23/2023", 505, 10, 1111, "EnRoute", "pending"),
            new Delivery("09/15/2023", 666, 1, 5399, "Scheduled", "pending"),
        };

        deliveryRepo.AddDelivery(deliveryA);
        deliveryRepo.AddDelivery(deliveryB);
        deliveryRepo.AddDelivery(deliveryC);
        deliveryRepo.AddDelivery(testDeliveries[0]);
        deliveryRepo.AddDelivery(testDeliveries[1]);
        deliveryRepo.AddDelivery(testDeliveries[2]);
    }

    [Fact]
    public void Add_Delivery_To_Db_Return_True()
    {
        Delivery newDelivery = new Delivery();

        bool returnBool = deliveryRepo.AddDelivery(newDelivery);

        Assert.True(returnBool);
    }

    [Fact]
    public void Get_Total_Delivery_Count()
    {
        int expectedCount = 6;
        int actualCount = deliveryRepo.GetDeliveries().Count();
    
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public void Get_Delivery_By_Id_Return_Correct_Delivery()
    {
        Delivery deliveryResult = deliveryRepo.GetDeliveryById(2);

        Assert.Equal(deliveryResult, deliveryB);
    }

    [Fact]
    public void Get_Total_Count_Of_Complete_Deliveries_Return_Correct_Count()
    {
        int expectedCount = 2;
        int actualCount = deliveryRepo.GetCompletedDeliveries().Count();

        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public void Update_Delivery_Info_Check_Return_True()
    {
        Delivery updatedDeliveryLD3 = new Delivery("09/15/2023", 244779, 2, 5399, "Complete", "09/20/2023");

        bool updatedResult = deliveryRepo.EditDeliveryInfo(6, updatedDeliveryLD3);

        Assert.True(updatedResult);
    }

    [Fact]
    public void Delete_Delivery_Check_True()
    {
        Delivery selectedDelivery = deliveryRepo.GetDeliveryById(4);

        bool deleteResult = deliveryRepo.DeleteDelivery(selectedDelivery);
        int expectedCount = 5;
        int actualCount = deliveryRepo.GetDeliveries().Count();

        Assert.True(deleteResult);
        Assert.Equal(expectedCount, actualCount);
    }

    [Fact]
    public void Delete_Deliveries_By_Id_Get_Count_And_Check_If_True()
    {
        // int customerId = 1111;
        bool deleteResult = deliveryRepo.DeleteDeliveriesByCustomerId(1111);

        int expectedCount = 3;
        int actualCount = deliveryRepo.GetDeliveries().Count();

        Assert.True(deleteResult);
        Assert.Equal(expectedCount, actualCount);
    }
}