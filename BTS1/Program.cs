namespace BTS1;

public static class Program
{
    static void ExecuteSystem(int year, List<Entity> entities,
        MultiValueDictionary<int, (List<Type> Components, ISystem System)> systems)
    {
        // extract phases
        List<int> phases = systems.Keys.ToList();
        phases.Sort();

        Console.WriteLine("Current State");
        foreach (var entity in entities)
        {
            Console.WriteLine($"Entity ID: {entity.Id}");
            foreach (var aspect in entity.GetAllComponents())
            {
                Console.WriteLine($"{aspect.GetType().Name}: {aspect}");
            }
            Console.WriteLine();
        }
        
        foreach (int phase in phases)
        {
            Console.WriteLine($"Executing Year {year}, Phase {phase}");

            foreach (var (components, system) in systems[phase])
            {
                foreach (var entity in entities.Where(e => e.HasAnyComponent(components)))
                {
                    Console.WriteLine($"System {system.GetType().Name} Entity {entity.Id}");
                    system.Run(entity, entities);
                }
            }
        }
        
        Console.WriteLine();
        
        Console.WriteLine("Current State");
        foreach (var entity in entities)
        {
            Console.WriteLine($"Entity ID: {entity.Id}");
            foreach (var aspect in entity.GetAllComponents())
            {
                Console.WriteLine($"{aspect.GetType().Name}: {aspect}");
            }
            Console.WriteLine();
        }

        
    }
    
    // ReSharper disable once UnusedParameter.Local
    static void Main(string[] args)
    {
        Entity origin = new();
        origin.AddComponent(new CName("Origin"));
        origin.AddComponent(new CPositionLocation(50, 50));
        
        Entity destination = new();
        destination.AddComponent(new CName("Destination"));
        destination.AddComponent(new CPositionLocation(150, 50));

        Entity ship = new();
        ship.AddComponent(new CPositionEntity(origin.Id));
        ship.AddComponent(new CDestinationEntity(destination.Id, 5));

        List<Entity> entities = [origin, destination, ship];

        MultiValueDictionary<int, (List<Type> Components, ISystem System)> systems = new();

        Movement movement = new Movement();
        var (phase, registeredComponents) = movement.Register();
        systems.AddOrUpdate(phase, (registeredComponents, movement));

        for (int year = 2400; year < 2405; ++year)
        {
            ExecuteSystem(year, entities, systems); 
        }
    }
}