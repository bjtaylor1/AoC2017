// Day17r.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
int N;
int Pn(int n)
{
	if (n == 0) return 0;
	return (Pn(n - 1) + N) % n;
}

int main()
{
	std::cin >> N;
	std::cout << Pn(50000000);
    return 0;
}

