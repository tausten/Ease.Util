# Ease.Util.Disposably
This namespace is to gather anything that augments or closely relies upon the 
[`IDisposable` pattern](https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-dispose).

## Implementation of the `IDisposable` Pattern
Most of the time, you should not need to worry about implementing `IDisposable` on your classes - the 
garbage collector will do the right thing. For those rare occurrences where you do find you need to 
implement it, the [`SafeDisposable`](xref:Ease.Util.Disposably.SafeDisposable) and 
[`SafeDisposableWithFinalizer`](xref:Ease.Util.Disposably.SafeDisposableWithFinalizer) classes can save you from 
falling into some pitfalls. 

### [`SafeDisposable`](xref:Ease.Util.Disposably.SafeDisposable)
Here's an example of implementation of the `IDisposable` pattern at its easiest:

```csharp
public class MyDisposable : SafeDisposable
{
    protected override void DisposeManagedObjects()
    {
        // TODO: Explicitly dispose of any *managed* objects your class has 
        // allocated / taken ownership of
    }
}
```

With that, you have a thread-safe implementation of the pattern that will gracefully handle repeated calls to `Dispose()`, and 
provide your class some additional facilities such as the `IsDisposed` property, and the `CheckDisposed()` method. 

If your class holds references to large objects, and you want a way to nullify the references explicitly, then you can 
override the `NullifyLargeFields()` method. It will be called after `DisposeManagedObjects()`, so you don't need to 
be nullifying references in there if you wish to separate the two operations.

### [`SafeDisposableWithFinalizer`](xref:Ease.Util.Disposably.SafeDisposableWithFinalizer)

If your class works with unmanaged resources and you wish to use the `IDisposable` pattern instead of wrapping them 
in `SafeHandle`, then you can do this easily with the following:

```csharp
public class MyDisposable : SafeDisposableWithFinalizer
{
    protected override void DisposeUnmanagedObjects()
    {
        // TODO: Explicitly dispose of any *unmanaged* objects your class has 
        // allocated / taken ownership of
    }
}
```

By doing this, your class will have implemented the `Finalizer`, and will appropriately handle either explicit `Dispose()`
or finalization, protect against repeated calls / execution of this cleanup, and do so in a threadsafe manner. If your class 
has a mixture of managed and unmanaged resources needing such cleanup, then you may also override the `DisposeManagedObjects` 
method. In fact, it is useful that the two be differentiated because managed resources are automatically Finalized... and therefore 
your managed resource cleanup does not need to be executed during finalization - only your unmanaged resource cleanup does. 

## Scoped Helpers
### [`Scoped`](xref:Ease.Util.Disposably.Scoped)
Leaning on the `IDisposable` pattern, the [`Scoped`](xref:Ease.Util.Disposably.Scoped) class provides a quick and dirty way to 
manage scope enter-exit behavior. Let's say you wanted to log something on entry of a code block, and then log again on exit 
(regardless of how the exit is happening), you could do the following:

```csharp
// ... some code
using (new Scoped(() => Console.WriteLine(">> ENTERING"), () => Console.WriteLine("<< EXITING"))
{
    // The stuff to execute after the `entryAction` has been executed, 
    // and before executing the `exitAction` 
}
// ... some more code
```

Coupled with closures, you can leverage state from the outer scope in your `enterAction` and `exitAction` to accomplish 
less trivial things.

### [`FactoryScoped<T>`](xref:Ease.Util.Disposably.FactoryScoped`1)
If you're working with an implementation of the _factory pattern_ that includes an explicit release, you can lean on
the [`FactoryScoped<T>`](xref:Ease.Util.Disposably.FactoryScoped`1) class to map this into the `IDisposable` pattern so that you
won't have to worry about what happens if exceptions are thrown, etc...  

```csharp
// We've obtained `widgetFactory` somehow, and want to use its `.Allocate(...)` and `.Release(...)`
// methods to manage our use of an instance of `Widget`
var widgetFactory;

using (var scope = new FactoryScoped<Widget>(
    () => widgetFactory.Allocate(x, y, z), 
    theWidget => widgetFactory.Release(theWidget))
{
    // Some code...
    var stuff = scope.Instance.DoStuff("with the widget instance obtained from the factory");
    // Other code...
}
// ... code that doesn't need the widget
```

If the factory methods themselves are trivial, the using statement can even boil down to something like this:

```csharp
using (var scope = new FactoryScoped<Widget>(widgetFactory.Allocate, widgetFactory.Release)
```
i.e. if the `allocateFunction` is signature-compatible with `Func<T>`...  and if the `releaseAction` 
is signature-compatible with `Action<T>`. Often times, the second is true.