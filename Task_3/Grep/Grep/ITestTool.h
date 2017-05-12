#pragma once
#include <experimental\filesystem>

namespace grep {
	class ITestTool {
	public:

		virtual void test(std::experimental::filesystem::directory_entry entry, std::string& word, bool ignoreCaseSensitivity) = 0;
		virtual ~ITestTool() {}

	};
}