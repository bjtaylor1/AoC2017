// DM1C.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
using namespace std;

int main()
{
	string input;
	vector<int> numbers;
	while (getline(cin, input) && !input.empty())
	{
		istringstream iss(input);
		int n;
		while (iss >> n)
		{
			numbers.push_back(n);
		}
	}
    return 0;
}

