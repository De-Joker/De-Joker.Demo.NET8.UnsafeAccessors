using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

Console.WriteLine("*** Program Start ***");
Console.WriteLine();
Console.WriteLine();

Demo demo = new();

Stopwatch stopwatch = new ();

Console.WriteLine("Reflection - DemoMethod");
stopwatch.Start();
typeof(Demo).GetMethod("DemoMethod", BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(demo, Array.Empty<object>());
stopwatch.Stop();
Console.WriteLine($"Reflection - DemoMethod - End - Time Elapsed: {stopwatch.Elapsed}");

Console.WriteLine();
Console.WriteLine($"UnsafeAccessor - DemoMethod");
stopwatch.Restart();
Caller.GetDemoMethod(demo);
stopwatch.Stop();
Console.WriteLine($"UnsafeAccessor - DemoMethod - End - Time Elapsed: {stopwatch.Elapsed}");

Console.WriteLine();
Console.WriteLine("Reflection - DemoField - Set");
stopwatch.Restart();
var reflectionField = typeof(Demo).GetField("DemoField", BindingFlags.Instance | BindingFlags.NonPublic)!;
reflectionField.SetValue(demo, "DemoFieldChangedReflection");
stopwatch.Stop();
Console.WriteLine($"Reflection - DemoField - Set - End - Value: {reflectionField.GetValue(demo)} - Time Elapsed: {stopwatch.Elapsed}");

Console.WriteLine();
Console.WriteLine($"UnsafeAccessor - DemoField - Set");
stopwatch.Restart();
string demoFieldUnsafeValue = Caller.GetDemoField(demo) = "DemoFieldChangedUnsafe";
stopwatch.Stop();
Console.WriteLine($"UnsafeAccessor - DemoField - Set - End - Value: {demoFieldUnsafeValue} - Time Elapsed: {stopwatch.Elapsed}");

Console.WriteLine();
Console.WriteLine();
Console.WriteLine("*** Program End ***");

public class Caller
{
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "DemoMethod")]
    public static extern void GetDemoMethod(Demo demo);

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "DemoField")]
    public static extern ref string GetDemoField(Demo demo);
}
public class Demo
{
    private string DemoField = "DemoFieldUnchanged";
    private void DemoMethod()
    {
        Console.WriteLine("Now in DemoMethod");
    }
}