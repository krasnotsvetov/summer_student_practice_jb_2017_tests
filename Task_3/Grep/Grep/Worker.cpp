#include "Worker.h"

void grep::Worker::operator()()
{
	while (pool.isRunning)
	{
		std::function<void()> task;
		{
			std::unique_lock<std::mutex> lock(pool.mutex);
			isIdle = true;

			pool.notifier.wait(lock, [this]() {return !(pool.isRunning && pool.tasks.empty()); });

			isIdle = false;
			if (!pool.isRunning) return;
			task = pool.tasks.front();
			pool.tasks.pop();
		}
		task();
		{
			std::unique_lock<std::mutex> lock(pool.mutex);
			if (!pool.HasTasks()) {
				pool.emptyNotifier.notify_all();
			}
		}
	}
}
