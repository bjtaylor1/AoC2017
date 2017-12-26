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
		mpz_w n, nmod;
		int op = std::stoi(input);
		mpz_fib_ui(n.value, op);
		std::cout << n.value << std::endl;
	}
    return 0;
}

