namespace EventSourcing.Aggregates;

public static class Events
{
    public static T Apply<T>(params IEnumerable<object> events)
    {
        var aggregate = Activator.CreateInstance<T>();
        
        // find apply method that takes in only one parameter of type of the event, named apply
        foreach (var @event in events)
        {
            var method = typeof(T).GetMethods().SingleOrDefault(p =>
            {
                return p.Name == "Apply" && p.GetParameters().Any(p => p.ParameterType == @event.GetType());
            });

            if (method != null)
            {
                method.Invoke(aggregate, [@event]);
            }
            
        }

        return aggregate;
    }
}