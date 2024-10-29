using System;
using System.Threading;
using System.Threading.Tasks;
class Program
{
    static async Task Main(string[] args)
    {
        using CancellationTokenSource cts = new CancellationTokenSource();
        Task task = Task.Run(() => DoWork(cts.Token), cts.Token);
        await Task.Delay(2000);
        Console.WriteLine("Скасування завдання...");
        cts.Cancel();
        try
        {
            await task;
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Завдання було скасовано.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Сталася помилка: {ex.Message}");
        }
        Console.WriteLine("Програма завершена.");
    }
    static void DoWork(CancellationToken token)
    {
        for (int i = 0; i < 10; i++)
        {
            if (token.IsCancellationRequested)
            {
                Console.WriteLine("Виконання скасовано!");
                token.ThrowIfCancellationRequested();
            }
            Console.WriteLine($"Виконання {i + 1}/10...");
            Thread.Sleep(1000);
        }
        Console.WriteLine("Завдання виконано успішно!");
    }
}
