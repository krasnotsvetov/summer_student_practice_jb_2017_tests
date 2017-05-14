#pragma once
#include <string>
#include "ThreadPool.h"
#include <experimental/filesystem>

namespace grep {
	class Grep {
	public:
		Grep(std::string path, std::string word, bool ignoreCaseSensitivity, int threadCount = 8) : pool(threadCount)
		{
			this->taskCount = 0;
			this->path = path;
			this->word = word;
			this->ignoreCaseSensitivity = ignoreCaseSensitivity;
		}

		void Execute();


		//TODO: should be private, but lambda use them

		ThreadPool pool;
		void addVisitor(std::experimental::filesystem::path path);
		void addTester(std::experimental::filesystem::directory_entry de);

		//TODO:: implent better solution for notifying
		std::atomic<int> taskCount;
		std::condition_variable notifier;

	private:
		std::string path;
		std::string word;
		bool ignoreCaseSensitivity;

	};
}
