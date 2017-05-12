#include "Grep.h"
#include <iostream>
#include "PTestTool.h"

namespace filesystem = std::experimental::filesystem;

namespace grep {
	///Start grep process
	void Grep::Execute()
	{
		std::error_code errCode;
		if (filesystem::exists(path, errCode)) {
			filesystem::path p(path);
			std::mutex m;
			std::unique_lock<std::mutex> lock(m);
			
			visitFS(p);
			notifier.wait(lock, [this]() {return taskCount == 0; });


		}
		else {
			std::cout << "The path is wrong";
		}
	}

	void Grep::visitFS(filesystem::path rootPath) {
		this->taskCount++;
		pool.addTask(
			[rootPath, this]() {
			for (auto& p : filesystem::directory_iterator(rootPath)) {
				auto status = p.status();
				if (filesystem::is_directory(status)) {
					this->visitFS(p.path());
				}
				else if (filesystem::is_regular_file(status) && !filesystem::is_symlink(status)) {
					testFile(p);
				}
			}
			this->taskCount--;
			if (taskCount == 0) {
				this->notifier.notify_all();
			}
		}
		);
	}
	void Grep::testFile(std::experimental::filesystem::directory_entry de)
	{
		this->taskCount++;
		
		PTestTool tool;
		tool.test(de, word, ignoreCaseSensitivity);

		taskCount--;
		if (taskCount == 0) {
			this->notifier.notify_all();
		}
	}
}
