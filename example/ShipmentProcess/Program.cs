class Program
{
    static void Main(string[] args)
    {
        var process = new ShipmentProcess.ShipmentProcess(Guid.NewGuid());            

        process.OnProcessShipment(Guid.NewGuid());
        process.Print();
        
        process.OnInvoiceGenerationRequested();
        process.Print();
        
        process.OnManifestationRequested();
        process.Print();
        
        process.OnCollectionScheduleRequested();
        process.Print();

        var path = new Random().Next(0, 1);
        
        switch (path)
        {
            case 0:
                process.OnLabelGenerationRequested();
                break;
            case 1:
                process.OnQrCodeGenerationRequested();
                break;
        }

        process.Print();
        
        process.OnReceiptGenerationRequested(); 
        process.Print();

        process.OnDocumentsPackRequested();
        process.Print();
        
        process.OnCombinedDocumentRequested();
        process.Print();
        
        process.OnCollectionBookingRequested();
        process.Print();
        
        process.OnCollectionCancellationRequested();
        process.Print();

        process.OnShipmentCancellationRequested();
        process.Print();
        // Console.WriteLine("yo");
        // Console.WriteLine(process.ToDotGraph());
        //
        Console.WriteLine("Press any key...");
        Console.ReadKey(true);
    }        
}