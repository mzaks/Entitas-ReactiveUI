using Entitas;
using Entitas.CodeGenerator;
using System.Collections.Generic;

[SingleEntity]
public class TickComponent : IComponent
{
  public long currentTick;
}

[SingleEntity]
public class JumpInTimeComponent : IComponent
{
	public long targetTick;
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


public class ConsumptionEntry
{
	public ConsumptionEntry(long tick, int amount)
	{
		this.tick = tick;
		this.amount = amount;
	}
	public readonly long tick;
	public readonly int amount;
}

[SingleEntity]
public class ConsumtionHistoryComponent : IComponent
{
	public List<ConsumptionEntry> entires;
}

[SingleEntity]
public class LogicSystemsComponent : IComponent
{
	public Systems systems;
}


public interface ITickListener {
	void TickChanged();
}

public class TickListenerComponent : IComponent {
	public ITickListener listener;
}

public interface IPauseListener {
	void PauseStateChanged();
}

public class PauseListenerComponent : IComponent {
	public IPauseListener listener;
}

public interface IElixirListener {
	void ElixirAmountChanged();
}

public class ElixirListenerComponent : IComponent {
	public IElixirListener listener;
}
