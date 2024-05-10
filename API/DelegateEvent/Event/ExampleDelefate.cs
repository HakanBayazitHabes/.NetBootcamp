namespace API.DelegateEvent.Event;

public class ExampleDelefate
{
    public static void Main()
    {
        Program3.Main();
    }
}
public delegate void StatusChanged(string status);

public class Process
{
    public event StatusChanged OnStatusChanged;

    public void StartProcess()
    {
        // İşlem başladı
        OnStatusChanged?.Invoke("Process Started");

        // İşlem devam ediyor
        System.Threading.Thread.Sleep(1000); // 1 saniye bekletiyoruz
        OnStatusChanged?.Invoke("Process Running");

        // İşlem bitti
        OnStatusChanged?.Invoke("Process Completed");
    }
}

public class Program3
{
    public static void Main()
    {
        Process myProcess = new Process();
        myProcess.OnStatusChanged += ProcessStatusChanged;
        myProcess.StartProcess();
    }

    static void ProcessStatusChanged(string status)
    {
        Console.WriteLine("Status: " + status);
    }
}

