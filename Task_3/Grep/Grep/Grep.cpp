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
			
			addVisitor(p);
			notifier.wait(lock, [this]() {return taskCount == 0; });
			

		}
		else {
			std::cout << "The path is wrong";
		}
	}

	///add task for folder visiting
	void Grep::addVisitor(filesystem::path rootPath) {
		this->taskCount++;
		pool.addTask(
			[rootPath, this]() {
			for (auto& p : filesystem::directory_iterator(rootPath)) {
				auto status = p.status();
				if (filesystem::is_directory(status)) {
					this->addVisitor(p.path());
				}
				else if (filesystem::is_regular_file(status) && !filesystem::is_symlink(status)) { 
					addTester(p); 
				}
			}
			this->taskCount--;
			if (taskCount == 0) {
				this->notifier.notify_all();
			}
		}
		);
	}
		 
	///add task for file checking
	void Grep::addTester(std::experimental::filesystem::directory_entry de)
	{  
		this->taskCount++;
  		pool.addTask([de, this]() {
			PTestTool tool;
			tool.test(de, word, ignoreCaseSensitivity);
			this->taskCount--;
			if (taskCount == 0) {
				this->notifier.notify_all();
			}
		}
		);
 		
	}
}
