using Entitas;
using Entitas.CodeGenerator;

[SingleEntity]
public class TickComponent : IComponent
{
  public long currentTick;
}

[SingleEntity]
public class ElixirComponent : IComponent
{
  public float amount;
}

[SingleEntity]
public class PauseComponent : IComponent {}

public class ConsumeComponent : IComponent
{
  public int amount;
}