namespace KazusaHSR.GameServer;

public enum TaskExecutorState
{
	Idle = 0,
	Running = 1,
	Disposed = 2,
}

public interface ITaskExecutorRuntime : IDisposable
{
	TaskExecutorState GetTaskState();
	void OnTaskBegin();
	void OnTaskReset();
}