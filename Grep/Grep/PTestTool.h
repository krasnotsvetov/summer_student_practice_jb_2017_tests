#pragma one
#include <experimental\filesystem>
#include "ITestTool.h"
#include <fstream>
#include <string>
#include <iostream>
#include <vector>

namespace grep {
	class PTestTool : ITestTool {
	public:

		void test(std::experimental::filesystem::directory_entry entry, std::string& word, bool ignoreCaseSensitivity) override;
		~PTestTool() override {}
	private:
		const int BUFF_SIZE = 16384;
		bool pFunctionTest(std::string & str, std::string & word, bool caseSensitivity);
		char grep::PTestTool::transformChar(char const c, bool const caseSensitivity);
	};
}