// See https://aka.ms/new-console-template for more information
using Infoware.ErrOr;
using System.Net.Http.Headers;

Console.WriteLine("Hello, World!");

ErrOr<Example> value1 = ErrOr<Example>.Ok(new Example());
ErrOr<Example> value2 = new(new Exception("Error"));

Example value1implicit = value1.Value;
Example value2implicit = value2.Value;

var a = value2implicit != null ? value2implicit : ErrOr<Example>.Fail(Method());

string Method()
{
    return "hello";
}
class Example
{

}
static class Example2
{
    public static ErrOr<Example> Method()
    {
        return new Example();
    }

    public static ErrOr<Example> Method2()
    {
        return new Exception("Error");
    }


    public static ErrOr<Example> Method3()
    {
        Example? result = null;
        return result!;
    }


}