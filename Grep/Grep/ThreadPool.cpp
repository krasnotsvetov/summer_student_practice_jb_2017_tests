#include "Worker.h"

namespace grep {
	ThreadPool::ThreadPool(int count) 
	{
		isRunning = true;
		for (int i = 0; i < count; i++) {
			workers.push_back(Worker(*this));
			threads.push_back(std::thread(workers[i]));
		}
	}

	ThreadPool::~ThreadPool()
	{
		isRunning = false;
		notifier.notify_all();
		for (auto& t : threads) {
			t.join();
		}
	}

	void ThreadPool::addTask(std::function<void()> func)
	{
		if (!isRunning) return;
		{
			std::unique_lock<std::mutex> lock(mutex);
			tasks.push(func);
		}
		notifier.notify_one();
	}
	bool ThreadPool::HasTasks()
	{
		if (!tasks.empty()) {
			return true;
		}
		for (auto& w : workers) {
			if (!w.isIdle) {
				return true;
			}
		}
		return false;
	}
}
