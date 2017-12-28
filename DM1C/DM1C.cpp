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
	vector<map<int,int>> sleighs(numSleighs);
	generate(sleighs.begin(), sleighs.end(), [sleighNum = 0, numColours = numSleighs, numbers] () mutable {
		map<int, int> sleigh;
		for (int colour = 0; colour < numColours; colour++)
		{
			sleigh.insert(pair<int, int>(colour, numbers[sleighNum * numColours + colour]));
		}
		sleighNum++;
		return sleigh;
	});
	vector<int> permutations(10);
	iota(permutations.begin(), permutations.end(), 0);
	int minNumMoves = accumulate(numbers.begin(), numbers.end(), 0);
	do
	{
		int numMoves = accumulate(permutations.begin(), permutations.end(), 0, [sleighs](int total, int sleighNum)
		{
			return total + accumulate(sleighs.begin(), sleighs.end(), 0, 
			[innerSleighNum = 0, sleighNum] (int subTotal, map<int, int> sleigh) mutable 
			{
				return subTotal + (innerSleighNum++ == sleighNum ? 0 : sleigh[sleighNum]);
			});
		});
		if (numMoves < minNumMoves) minNumMoves = numMoves;
	} while (next_permutation(permutations.begin(), permutations.end()));
	cout << minNumMoves << endl;
    return 0;
}

