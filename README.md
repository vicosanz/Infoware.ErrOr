# Infoware.ErrOr
Wrap an exception to a type

[![NuGet Badge](https://buildstats.info/nuget/Infoware.ErrOr)](https://www.nuget.org/packages/Infoware.ErrOr/)


Implicit convertion automatically allow return type or exception
Match method handle success and failed result in the same expression

Usage:
```csharp
    public ErrOr<customer> GetCustomer(int id)
    {
        var customer = _customerRepository.Get(id);
        return customer != null ? customer : new Exception("Customer not found");
    }
    
    var customer = GetCustomer(50);
    Console.WriteLine (
        customer.Match<string>(
            success => $"Customer found name :{success.Value.Name}",
            exception => $"Error: {exception.Message}"
        )
    );
```

If you need additional error information, feel free to create a custom exception type

Example:
```csharp
    public class DomainException : Exception
    {
        private const int statusCodeDefault = StatusCodes.Status500InternalServerError;

        public DomainException(Localize localizeMessage, params object[] arguments) : base()
        {
            LocalizeMessage = localizeMessage;
            StatusCode = statusCodeDefault;
            Arguments = arguments;
        }

        public string GetErrorLocalized()
        {
            return LocalizeMessage.GetString(Arguments);
        }

        public DomainException(Localize localizeMessage) : base()
        {
            LocalizeMessage = localizeMessage;
            StatusCode = statusCodeDefault;
        }

        public Localize LocalizeMessage { get; }
        public int StatusCode { get; }
        public object[]? Arguments { get; }
    }

    public async Task<ErrOr<IPagedList<ItemView>>> Handle(ItemFindQuery request, CancellationToken cancellationToken)
    {
        var permission = await _permisoService.ValidateAsync(request.userId, cancellationToken: cancellationToken);
        if (permission.IsFaulted) return permission.Exception;

        if (string.IsNullOrWhiteSpace(request.Expression)){
            return new DomainException(_localize.ExpressionIsEmpty);
        }

        return new(await _itemRepository.GetAllItems(request!.Expresion!, cancellationToken: cancellationToken));
    }

```
