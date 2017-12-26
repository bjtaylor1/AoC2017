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
	mpz_w tot(0);
	mpz_w mill(1000000);
	mpz_w limit;
	mpz_w mod;
	long i = 0;
	do
	{
		mpz_w fac;
		mpz_fac_ui(fac.value, ++i);
		mpz_mod(mod.value, fac.value, mill.value);
		mpz_add(tot.value, tot.value, mod.value);
		mpz_mod(tot.value, tot.value, mill.value);
	} while (mpz_cmp_ui(mod.value, 0) > 0);
	std::cout << tot.value << std::endl;
	std::cout << i << std::endl;
	std::cin.get();
    return 0;
}

