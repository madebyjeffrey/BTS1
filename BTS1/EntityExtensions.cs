namespace BTS1;

public static class EntityExtensions
{
    public static (long X, long Y) GetPosition(this Entity entity, List<Entity> entities)
    {
        if (entity.GetComponent<CPositionLocation>(out var pos))
        {
            return (pos.X, pos.Y);
        }
        else if (entity.GetComponent<CPositionEntity>(out var owner))
        {
            Entity? ownerEntity = entities.Find(e => e.Id == owner.Owner);

            if (ownerEntity != null)
            {
                if (ownerEntity.GetComponent<CPositionLocation>(out var pos2))
                {
                    return (pos2.X, pos2.Y);
                }
            }
        }

        throw new Exception($"Entity {entity.Id} has no position");
    }

    public static bool HasPosition(this Entity entity, long X, long Y, List<Entity> entities)
    {
        if (entity.GetComponent<CPositionLocation>(out var pos))
        {
            return X == pos.X && Y == pos.Y;
        }

        return false;
    }

    public static (long X, long Y, float Warp, Guid? DestinationEntity) GetDestination(this Entity entity, List<Entity> entities)
    {
        long X;
        long Y;
        float Warp;
        Guid? DestinationEntity = null;
        
        if (entity.GetComponent<CDestinationEntity>(out var entityDest))
        {
            Warp = entityDest.Warp;
            // get entity
            var destination = entities.FirstOrDefault(x => x.Id == entityDest.Target);

            if (destination is not null && destination.GetComponent<CPositionLocation>(out var position))
            {
                DestinationEntity = destination.Id;
                X = position.X;
                Y = position.Y;
            }
            else
            {
                throw new Exception("Entity Destination has no position");
            }
        } else if (entity.GetComponent<CDestinationLocation>(out var positionDest))
        {
            Warp = positionDest.Warp;
            X = positionDest.X;
            Y = positionDest.Y;
        }
        else
        {
            throw new Exception("Entity has no destination");
        }

        return (X, Y, Warp, DestinationEntity);
    }
}