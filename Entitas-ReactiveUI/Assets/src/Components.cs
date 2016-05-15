using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

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
	
public class ConsumeElixirComponent : IComponent
{
  public int amount;
}

[SingleEntity]
public class PauseComponent : IComponent {}

[SingleEntity]
public class JumpInTimeComponent : IComponent
{
	public long targetTick;
}


public class ConsumtionEntry
{
	public readonly long tick;
	public readonly int amount;

	public ConsumtionEntry(long tick, int amount)
	{
		this.tick = tick;
		this.amount = amount;
	}
}

[SingleEntity]
public class ConsumtionHistoryComponent : IComponent
{
	public List<ConsumtionEntry> entries;
}

[SingleEntity]
public class LogicSystemsComponent : IComponent
{
	public Systems systems;
}

[Pool]
public interface TickListener {
	void TickChanged();
}

[Pool]
public interface PauseListener {
	void PauseStateChanged();
}
	
[Pool]
public interface ElixirListener {
	void ElixirAmountChanged();
}
