#pragma once
#include <vector>
#include <thread>
#include <mutex>
#include <queue>
#include <atomic>
#include <condition_variable>

namespace grep {

	class Worker;
	class ThreadPool
	{

	public:
		///
		///Create thread pool where the maximum number of threads is count
		///
		ThreadPool(int count);

		~ThreadPool();


		///Add a task, where T is functional
		void addTask(std::function<void()> func);


		bool HasTasks();
		std::condition_variable emptyNotifier;

	private:
		friend class Worker;
		std::vector<std::thread> threads;
		std::vector<Worker> workers;
		std::queue<std::function<void()>> tasks;
		std::condition_variable notifier;
		std::mutex mutex;
		std::atomic<bool> isRunning;
	};

}
