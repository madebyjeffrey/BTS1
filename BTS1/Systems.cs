using System.Numerics;

namespace BTS1;

public interface ISystem
{
    public (int Phase, List<Type> registeredComponents) Register();
    public void Run(Entity entity, List<Entity> entities);
}

public class Movement : ISystem
{
    public (int Phase, List<Type> registeredComponents) Register()
    {
        return (6, [typeof(CDestinationEntity), typeof(CDestinationLocation)]);
    }

    public void Run(Entity entity, List<Entity> entities)
    {
        Vector2 origin = new Vector2();
        Vector2 dest = new Vector2();
        float speed = 0.0f;

        (origin.X, origin.Y) = entity.GetPosition(entities);
        
        (dest.X, dest.Y, speed, Guid? destinationEntity) = entity.GetDestination(entities);
        
        float distanceTravelledMaximum = speed * speed;
        float distanceToDestination = Vector2.Distance(origin, dest);

        // The old location isn't valid anymore
        entity.ClearComponent<CPositionLocation>();        
        entity.ClearComponent<CPositionEntity>();
        
        // case 1: we are not going to make it to the destination
        if (distanceToDestination > distanceTravelledMaximum)
        {
            Vector2 direction = Vector2.Normalize(Vector2.Subtract(dest, origin));
            Vector2 finalDest = Vector2.Multiply(direction, speed * speed);

            long X = (long)(origin.X + finalDest.X);
            long Y = (long)(origin.Y + finalDest.Y);

            // checks to see if there is an entity that exists at the new position
            Entity? newOwner = entities.Find(x => x.HasPosition(X, Y, entities));

            if (newOwner != null)
            {
                entity.AddComponent(new CPositionEntity(newOwner.Id));
            }
            else
            {
                entity.AddComponent(new CPositionLocation(X, Y));    
            }
        }
        else // case 2 is if we have reached OR exceeded the destination, set position to destination
        {
            entity.ClearComponent<CDestinationEntity>();
            entity.ClearComponent<CDestinationLocation>();
            
            if (destinationEntity.HasValue)
            {
                entity.AddComponent(new CPositionEntity(destinationEntity.Value));
            }
            else
            {
                entity.AddComponent(new CPositionLocation((long)dest.X, (long)dest.Y));
            }

        }
    }
}