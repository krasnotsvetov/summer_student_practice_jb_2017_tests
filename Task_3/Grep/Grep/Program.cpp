#include <iostream>
#include <thread>
#include <future>
#include <vector>
#include <experimental/filesystem>
#include <string>
#include "ThreadPool.h"
#include "Grep.h"

int main(int argc, char* argv[])
{

	if (argc < 3) {
		std::cout << "Usage <path> <word> <additional args>\n";
		std::cout << "Where <additional args> is \'-i\' for toggle off case sensitivity";
		return 0;
	}
	bool ignoreCaseSensitivity = false;
	
	if (argc == 4) {
		ignoreCaseSensitivity = std::string(argv[3]).compare("-i") == 0;
	}
	grep::Grep grep(argv[1], argv[2], ignoreCaseSensitivity);
	grep.Execute();
	return 0;
}
