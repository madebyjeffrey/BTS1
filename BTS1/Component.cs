namespace BTS1;


public interface Aspect
{
}

public record struct CName(string Name) : Aspect;

public record struct CPositionLocation(long X, long Y) : Aspect;

public record struct CPositionEntity(Guid Owner) : Aspect;

public record struct CDestinationEntity(Guid Target, float Warp) : Aspect;

public record struct CDestinationLocation(long X, long Y, float Warp): Aspect;