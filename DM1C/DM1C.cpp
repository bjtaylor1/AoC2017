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
	int numSleighs = sqrt(numbers.size());
	if (numSleighs * numSleighs != numbers.size()) throw runtime_error("Not a square number of inputs");
	map<int, map<int,int>> sleighs;
	int number;
	for (int sleighNum = 0; sleighNum < numSleighs; sleighNum++)
	{
		auto sleigh = sleighs.insert(pair<int, map<int,int>>(sleighNum, map<int,int>()));
		for (int colour = 0; colour < numSleighs; colour++)
		{
			sleigh.first->second.insert(pair<int, int>(colour, numbers[++number]));
		}
	}
	
    return 0;
}

