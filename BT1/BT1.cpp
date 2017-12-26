// BT1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"

class mpz_w
{
public:
	mpz_t value;
	mpz_w() { mpz_init(value); }
	mpz_w(int i) : mpz_w() { mpz_set_ui(value, i); }
	~mpz_w() { mpz_clear(value); }
	
};

std::string getinput()
{
	std::string input;
	std::cin >> input;
	return input;
}

int main()
{
	std::string input;
	while ((input = getinput()).size() > 0)
	{
		auto start = std::chrono::high_resolution_clock::now();
		mpz_w n, nmod;
		int op = std::stoi(input);
		mpz_fac_ui(n.value, op);
		std::cout << "Calculated fac" << std::endl;

		//std::stringstream ss;
		//ss << n.value;
		//std::cout << "Written to stringstream" << std::endl;

		//auto str = ss.str();
		//int c = 0;
		//std::cout << "Got string - it's " << str.size() << " long" << std::endl;

		auto bitcnt = mpz_popcount(n.value);
		std::cout << "Bit count = " << bitcnt << std::endl;

		//std::string pattern = "123";
		//if (str.size() < 1000) std::cout << str << std::endl;
		//for (int pos = 0; pos < str.size() - (pattern.size()-1); pos++)
		//	if (str.substr(pos,pattern.size()) == pattern)
		//{
		//	c++;
		//}

		//std::cout << c << std::endl;

		auto finish = std::chrono::high_resolution_clock::now();
		std::chrono::duration<double> elapsed = finish - start;
		std::cout << "Elapsed: " << elapsed.count() << std::endl;
	}
    return 0;
}

