namespace ScooterRentalDomain;
internal class ReturnScooterService
{
    void returnScooter(long clientId, long scooterId, Position where, int minutes, float batteryLevel, Object[] scooterData,
                       float clientCredit, bool clientWithImmediatePayment, int immediateTransactionsCounter)
    {
        //metoda returnScooter ma 4 parametry - clientId, scooterId, where, minutes
        //resztę pobieramy na podstawię clientId i scooterId z bazy
        //(batteryLevel, Object[] scooterData, float clientCredit, bool clientWithImmediatePayment, int immediateTransactionsCounter)
        //kod celowo nie jest najpiękniejszy

        float unlocking = 0.0f;
        float pricePerMinute = 0.0f;
        if (scooterData[0].Equals("not_fast"))
        {
            unlocking = (float)scooterData[1];
            pricePerMinute = (float)scooterData[2];
        }
        else
        {
            unlocking = (float)scooterData[3];
            pricePerMinute = (float)scooterData[4];
        }

        float chargeAmount;
        float priceAmountClientMultiplicationFactor = 0.9f;
        if (clientWithImmediatePayment)
        {
            priceAmountClientMultiplicationFactor = 0.9f;
        }
        else
        {
            priceAmountClientMultiplicationFactor = 1;
        }
        float price = unlocking + pricePerMinute * minutes * priceAmountClientMultiplicationFactor;
        chargeAmount = Math.Max(price - clientCredit, 0);
        chargeClient(clientId, chargeAmount);
        bool needsToChargeBattery = false;
        if (clientWithImmediatePayment)
        {
            immediateTransactionsCounter++;
        }
        if (batteryLevel < 0.07)
        {
            needsToChargeBattery = true;
        }
        int loyaltyPoints = 0;
        if (minutes > 15 && minutes < 50)
        {
            loyaltyPoints = 4;
            if (priceAmountClientMultiplicationFactor < 1)
            {
                loyaltyPoints = 2;
            }
        }

        if (minutes >= 50 && chargeAmount > 30)
        {
            loyaltyPoints = 20;
        }
        saveInDatabase(loyaltyPoints, chargeAmount, needsToChargeBattery, immediateTransactionsCounter);
    }

    private void saveInDatabase(int loyaltyPoints, float chargeAmount, bool needsToChargeBattery, int immediateTransactionsCounter)
    {
        //zapis wszystkigo do bazy danych
    }

    private void chargeClient(long clientId, float chargeAmount)
    {
        //obciążenie karty kredytowej
    }

}

class Position
{
    private readonly float latitude;
    private readonly float longitude;
}