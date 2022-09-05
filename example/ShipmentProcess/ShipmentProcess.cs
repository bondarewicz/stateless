using Stateless;

namespace ShipmentProcess;

public class ShipmentProcess
{

    enum Trigger
    {
        ProcessingRequested,
        InvoiceGenerationRequested,
        ManifestationRequested,
        CollectionScheduleRequested,
        CollectionBookingRequested,
        CollectionCancellationRequested,
        LabelGenerationRequested,
        QrCodeGenerationRequested,
        ReceiptGenerationRequested,
        DocumentsPackRequested,
        CombinedDocumentRequested,
        ShipmentCancellationRequested
    }

    enum State
    {
        Draft,
        Initialized,
        InvoiceCreated,
        Manifested,
        LabelsCreated,
        QrCodeCreated,
        ReceiptCreated,
        DocumentPackCreated,
        CombinedDocumentCreated,
        CombinedDocumentFailed,
        CollectionScheduled,
        CollectionBooked,
        CollectionCancelled,
        CancellationCompleted,
        Processed
    }

    State _state = State.Draft;
    readonly StateMachine<State, Trigger> _process;
    private Guid _id;

    public ShipmentProcess(Guid shipmentId)
    {
        _id = shipmentId;
        _process = new StateMachine<State, Trigger>(() => _state, s => _state = s);
        
        ConfigureProcess();
    }

    private void ConfigureProcess()
    {
        _process.Configure(State.Draft)
            .Permit(Trigger.ProcessingRequested, State.Initialized);

        _process.Configure(State.Initialized)
            .Permit(Trigger.InvoiceGenerationRequested, State.InvoiceCreated);

        _process.Configure(State.InvoiceCreated)
            .Permit(Trigger.ManifestationRequested, State.Manifested);
        // .PermitReentry(Trigger.ManifestationRequested);

        _process.Configure(State.Manifested)
            .Permit(Trigger.CollectionScheduleRequested, State.CollectionScheduled);

        _process.Configure(State.CollectionScheduled)
            .Permit(Trigger.LabelGenerationRequested, State.LabelsCreated)
            .Permit(Trigger.QrCodeGenerationRequested, State.QrCodeCreated);

        _process.Configure(State.LabelsCreated)
            .Permit(Trigger.ReceiptGenerationRequested, State.ReceiptCreated);

        _process.Configure(State.QrCodeCreated)
            .Permit(Trigger.ReceiptGenerationRequested, State.ReceiptCreated);

        _process.Configure(State.ReceiptCreated)
            .Permit(Trigger.DocumentsPackRequested, State.DocumentPackCreated);

        _process.Configure(State.DocumentPackCreated)
            .Permit(Trigger.CombinedDocumentRequested, State.CombinedDocumentCreated);
        // .Permit(Trigger.CombinedDocumentRequested, State.CombinedDocumentFailed);

        _process.Configure(State.CombinedDocumentCreated)
            .Permit(Trigger.CollectionBookingRequested, State.CollectionBooked);

        _process.Configure(State.CollectionBooked)
            .Permit(Trigger.CollectionCancellationRequested, State.CollectionCancelled);

        _process.Configure(State.CollectionBooked)
            .Permit(Trigger.CollectionCancellationRequested, State.CollectionCancelled);

        // cancellation
        _process.Configure(State.Manifested)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.CollectionScheduled)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.CollectionBooked)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.LabelsCreated)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.QrCodeCreated)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.ReceiptCreated)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.DocumentPackCreated)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.CombinedDocumentCreated)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.CollectionBooked)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.CombinedDocumentCreated)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);

        _process.Configure(State.CollectionCancelled)
            .Permit(Trigger.ShipmentCancellationRequested, State.CancellationCompleted);
    }

    public void Print()
    {
        Console.WriteLine("Transitioning to [State:] {0}", _process.State);
    }

    public void OnProcessShipment(Guid id)
    {
        _id = id;
        Console.WriteLine("[ProcessShipment] started ShipmentId: [{0}]", _id.ToString());
        _process.Fire(Trigger.ProcessingRequested);
    }
    
    public void OnInvoiceGenerationRequested()
    {
        Console.WriteLine("Observed [InvoiceCreated]");
        _process.Fire(Trigger.InvoiceGenerationRequested);
    }
    
    public void OnManifestationRequested()
    {
        Console.WriteLine("Observed [ManifestationRequested]");
        _process.Fire(Trigger.ManifestationRequested);
    }

    public void OnCollectionScheduleRequested()
    {
        Console.WriteLine("Observed [CollectionScheduleRequested]");
        _process.Fire(Trigger.CollectionScheduleRequested);
    }
    
    public void OnLabelGenerationRequested()
    {
        Console.WriteLine("Observed [LabelGenerationRequested]");
        _process.Fire(Trigger.LabelGenerationRequested);
    }
    
    public void OnQrCodeGenerationRequested()
    {
        Console.WriteLine("Observed [QrCodeGenerationRequested]");
        _process.Fire(Trigger.QrCodeGenerationRequested);
    }
    
    public void OnReceiptGenerationRequested()
    {
        Console.WriteLine("Observed [ReceiptGenerationRequested]");
        _process.Fire(Trigger.ReceiptGenerationRequested);
    }
    
    public void OnDocumentsPackRequested()
    {
        Console.WriteLine("Observed [DocumentsPackRequested]");
        _process.Fire(Trigger.DocumentsPackRequested);
    }
    
    public void OnCombinedDocumentRequested()
    {
        Console.WriteLine("Observed [CombinedDocumentRequested]");
        _process.Fire(Trigger.CombinedDocumentRequested);
    }

    public void OnCollectionBookingRequested()
    {
        Console.WriteLine("Observed [CollectionBookingRequested]");
        _process.Fire(Trigger.CollectionBookingRequested);
    }
    
    public void OnCollectionCancellationRequested()
    {
        Console.WriteLine("Observed [CollectionCancellationRequested]");
        _process.Fire(Trigger.CollectionCancellationRequested);
    }
    
    public void OnShipmentCancellationRequested()
    {
        Console.WriteLine("Observed [ShipmentCancellationRequested]");
        _process.Fire(Trigger.ShipmentCancellationRequested);
    }
    
    
}