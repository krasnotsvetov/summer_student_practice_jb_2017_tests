#include <mutex>
#include "ThreadPool.h"

namespace grep {
	class Worker
	{
	public:
		Worker(ThreadPool& pool) : pool(pool) {

		}
		void operator()();

	private:
		friend class ThreadPool;
		volatile bool isIdle;
		ThreadPool& pool;
	};
}
